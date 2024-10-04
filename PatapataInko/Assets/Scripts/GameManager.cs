/*
    GameManager.cs 
    
    �Q�[�����̃��C���������s���N���X�B
*/

using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // �g�p����UI�֘A
    // �X�R�A�A���U���g�A�n�C�X�R�A�̃e�L�X�g
    [SerializeField] private Text scoreText;
    [SerializeField] private Text scoreResultText;
    [SerializeField] private Text highScoreText;

    [SerializeField] private Text countdownText;        // ���f����ĊJ���ɕ\������J�E���g�_�E��
    [SerializeField] private GameObject startGuide;     // ����K�C�hUI
    [SerializeField] private GameObject gameoverPanel;  // �Q�[���I�[�o�[���ɕ\������p�l��
    [SerializeField] private GameObject bestImage;      // �x�X�g�X�R�A�X�V���ɕ\������摜
    [SerializeField] private GameObject pauseButton;    // �|�[�Y�{�^��

    // �g�p������ʉ��֘A
    // ��ʑJ�ڎ��̌��ʉ��A�����ɓ����������̌��ʉ�
    [SerializeField] private AudioClip transSE;
    [SerializeField] private AudioClip hitSE;
    // ���ʂ𒲐߂��邽�߂�AudioMixer
    [SerializeField] private AudioMixer audioMixer;

    private FadeManager fm;          // FadeManager�擾�p(FadeManager:��ʑJ�ڎ��̏���)
    private PlayerManager pm;        // PlayerManager�擾�p(PlayerManager:�v���C���[�̐��䏈��)

    private float startCount;        // ���f����ĊJ���Ɏg�p����J�E���g
    private bool isBest;             // �x�X�g�X�R�A���X�V�������ǂ��̃t���O

    private Animator animator;       // Animator�擾�p
    private Animator goAnimator;     // �Q�[���I�[�o�[�p�l���̃A�j���[�^�[�擾�p
    private AudioSource audioSource; // AudioSource�擾�p

    private JsonPlayerData playerData;         // �ۑ�����Ă���v���C���[�f�[�^
    private JsonPlayerData playerSettingsData; // �ۑ�����Ă���v���C���[�ݒ�f�[�^

    // ���{��A�p��ł̃e�L�X�g�擾�p�z��
    private GameObject[] japaneseText;
    private GameObject[] englishText;

    public static bool isGameOver;             // �Q�[���I�[�o�[���ǂ����̃t���O
    public static bool isJapanese = true;      // �I�𒆂̌���͓��{�ꂩ�ǂ����̃t���O
    public bool isStart;                       // �Q�[���J�n�t���O
    public int score;                          // �Q�[�����̃X�R�A
    public int highScore;                      // �n�C�X�R�A
    public bool isPause;                       // ��~�{�^����������Ă��邩�ǂ����̃t���O
    public bool isRestart;                     // ���X�^�[�g�{�^����������Ă��邩�ǂ����̃t���O

    // ����ݒ�̓��{��{�^������������
    public void TapJapaneseButton()
    {
        isJapanese = true;
    }

    // ����ݒ�̉p��{�^������������
    public void TapEnglishButton()
    {
        isJapanese = false;
    }

    // �|�[�Y�{�^�������ňꎞ��~
    public void TapPauseButton()
    {
        isPause = true;
    }

    // �X�^�[�g�{�^�������ōĊJ
    public void TapStartButton()
    {
        startCount = 3.5f;
        isRestart = true;
        //isPause = false;

    }

    // �v���C�{�^�������Ń^�C�g����ʂ���Q�[����ʂ֑J��
    public void TapPlayButton()
    {
        fm.fadeOut = true;
        fm.isNextScene = true;
    }

    // �^�C�g���ɖ߂�{�^�������ŃQ�[����ʂ���^�C�g����ʂ֑J��
    public void TapBackButton()
    {
        fm.fadeOut = true;
        fm.isBackScene = true;
    }

    // ��ʑJ�ڎ��̌��ʉ���炷����
    public void PlayTransSE()
    {
        audioSource.PlayOneShot(transSE);
    }

    // �����ɓ����������̌��ʉ���炷����
    public void PlayHitSE()
    {
        audioSource.PlayOneShot(hitSE);
    }

    // AudioMixer�̉��ʂ�ύX���鏈��
    public void SetAudioMixerSE(float value)
    {
        //-80~0�ɕϊ�
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
        //audioMixer�ɑ��
        audioMixer.SetFloat("SE", volume);
    }


    // Start is called before the first frame update
    void Start()
    {
        // �J�[�\����\��
        //Cursor.visible = false;

        // ���{��A�p��ł̃e�L�X�g���擾
        japaneseText = GameObject.FindGameObjectsWithTag("JaText");
        englishText = GameObject.FindGameObjectsWithTag("EnText");

        gameoverPanel.SetActive(false);

        countdownText.enabled = false;

        fm =GameObject.Find("fadePanel").GetComponent<FadeManager>();
        pm =GameObject.Find("SpPlayer").GetComponent<PlayerManager>();
        
        // �V�[���J�ڎ��t�F�[�h�C��
        fm.fadeIn = true;

        //isPause = true;

        score = 0;
        startCount = 3.5f;

        isStart = false;
        isGameOver = false;
        isBest = false;

        animator =startGuide.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        goAnimator=gameoverPanel.transform.GetChild(0).GetComponent<Animator>();

        // �n�C�X�R�A���ۑ�����Ă��邩�m�F
        if (PlayerPrefs.HasKey("HighScore"))
        {
            // �ۑ�����Ă�����l���擾
            var jsonpData = PlayerPrefs.GetString("HighScore");

            // �擾����Json�f�[�^���I�u�W�F�N�g�ɕϊ�
            playerData = JsonUtility.FromJson<JsonPlayerData>(jsonpData);

            // �v���C���[�f�[�^����
            SetPlayerData();
        }

        // ����ݒ肪�ۑ�����Ă��邩�m�F
        if (PlayerPrefs.HasKey("Languages"))
        {
            // �ۑ�����Ă�����l���擾
            var jsonpData = PlayerPrefs.GetString("Languages");

            // �擾����Json�f�[�^���I�u�W�F�N�g�ɕϊ�
            playerSettingsData = JsonUtility.FromJson<JsonPlayerData>(jsonpData);

            // �v���C���[�f�[�^����
            SetPlayerData();
        }

    }

    // �ۑ�����Ă����l���Z�b�g
    void SetPlayerData()
    {
        if (playerData != null)
        {
            highScore=  playerData.highScore;
        }
        if (playerSettingsData != null)
        {
            isJapanese = playerSettingsData.isJapanese;
        }
    }

    // �f�[�^�L�^����
    void PlayerDataSave()
    {
        // �I�u�W�F�N�g�ɒl���i�[
        playerData = new JsonPlayerData();

        playerData.highScore = highScore;     // �n�C�X�R�A�i�[

        // �I�u�W�F�N�g��JSON�`���ɕϊ�
        PlayerPrefs.SetString("HighScore", JsonUtility.ToJson(playerData));
    }

    // �ݒ�L�^����
    public void PlayerSettingsSave()
    {
        // �I�u�W�F�N�g�ɒl���i�[
        playerSettingsData = new JsonPlayerData();

        playerSettingsData.isJapanese = isJapanese;   // ����ݒ�i�[

        // �I�u�W�F�N�g��JSON�`���ɕϊ�
        PlayerPrefs.SetString("Languages", JsonUtility.ToJson(playerSettingsData));
    }

    // �Q�[���ĊJ������
    private void GameRestart()
    {
        if (isRestart)
        {
            // �J�E���g�_�E����\������
            countdownText.enabled = true;
            startCount -= Time.deltaTime;
            countdownText.text = startCount.ToString("F0");

            // ��莞�Ԍo�ߌ�Q�[���ĊJ
            if (startCount <= 0.5f)
            {
                //countdownText.text = "0";
                countdownText.enabled = false;
                pauseButton.SetActive(true);
                isRestart = false;
                isPause = false;
            }
        }
    }

    // �X�^�[�g�K�C�h�̔�\������
    private void StartGuideFadeOut()
    {
        // �Q�[���X�^�[�g���Ă��Ȃ��Ƃ��Ƀ^�b�v
        if (!isStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // �K�C�h���t�F�[�h�A�E�g�����A�Q�[���J�n�t���O���I��
                animator.SetTrigger("FadeOut");
                isStart = true;
            }
        }
    }

    // UI�\������
    private void DisplayUI()
    {
        // �X�R�A�֘A�̃e�L�X�g�\��
        scoreText.text = String.Format("{0}", score);
        scoreResultText.text = String.Format("{0}", score);
        highScoreText.text = String.Format("{0}", highScore);

        // �e�L�X�g�͓��{�ꂩ�p�ꂩ�𔻒�A�\����\��
        if (isJapanese)
        {
            foreach (GameObject jaText in japaneseText)
            {
                jaText.SetActive(true);
            }
            foreach (GameObject enText in englishText)
            {
                enText.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject jaText in japaneseText)
            {
                jaText.SetActive(false);
            }
            foreach (GameObject enText in englishText)
            {
                enText.SetActive(true);
            }
        }
    }

    // �n�C�X�R�A�X�V����
    private void HighScoreUpdate()
    {
        // �n�C�X�R�A�X�V���Ă���΁A�t���O���I���ɂ�
        // �n�C�X�R�A���L�^
        if (score > highScore)
        {
            isBest = true;
            highScore = score;
            PlayerDataSave();
        }
    }

    // �Q�[���I�[�o�[������
    private void GameOver()
    {
        if (pm.isDie && !isGameOver)
        {
            // ��ʂ��h��鉉�o
            var impulseSource = GetComponent<CinemachineImpulseSource>();
            impulseSource.GenerateImpulse();

            gameoverPanel.SetActive(true);

            // �x�X�g�X�V���Ă����ꍇ�̓A�j���[�V�����g���K�[�𔭉�
            if (isBest)
            {
                goAnimator.SetTrigger("Best");
            }

            pauseButton.SetActive(false);
            isGameOver = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �X�^�[�g�O�̃K�C�h��\������
        StartGuideFadeOut();

        // �Q�[���ĊJ������
        GameRestart();

        // UI�\������
        DisplayUI();

        // �n�C�X�R�A�X�V����
        HighScoreUpdate();

        // �Q�[���I�[�o�[������
        GameOver();
    }
}
