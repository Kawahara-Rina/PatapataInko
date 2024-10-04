/*
    FadeManager.cs 
    
    ��ʑJ�ڎ��̃t�F�[�h�������s���N���X�B
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField]private float speed;   // �t�F�[�h����X�s�[�h
    private float red, green, blue;        // �C���[�W�̊e�F�v�f
    private Image fadeImage;               //�t�F�[�h����p�l��

    public float alfa;        // �C���[�W�̓����x

    public bool fadeOut;      // �t�F�[�h�A�E�g�Ɏg�p����t���O
    public bool fadeIn;       // �t�F�[�h�C���Ɏg�p����t���O
    public bool isNextScene;  // �V�[���J�ڎ��Ɏg�p����t���O
    public bool isBackScene;  // �V�[���J�ڎ��Ɏg�p����t���O


    // �t�F�[�h�C�����̏���
    void FadeIn()
    {
        // �A���t�@�l�����炵�Ă���(������)
        alfa -= speed * Time.deltaTime;

        // �F���Z�b�g
        SetColor();

        // �A���t�@�l��0�ȉ�(����)�ɂȂ����ꍇ
        if (alfa <= 0)
        {
            fadeIn = false;
            fadeImage.enabled = false;
        }
    }

    // �t�F�[�h�A�E�g���̏���
    void FadeOut()
    {
        fadeImage.enabled = true;

        // �A���t�@�l�𑝂₷(�s������)
        alfa += speed * Time.deltaTime;

        // �F���Z�b�g
        SetColor();

        // �A���t�@�l��1�ȏ�(�s����)�ɂȂ����ꍇ
        if (alfa >= 1)
        {

            if (isNextScene)
            {
                // �Q�[���V�[���ɑJ��
                LoadScene(Define.SCENE_GAME);
            }
            if (isBackScene)
            {
                // �^�C�g���V�[���ɑJ��
                LoadScene(Define.SCENE_TITLE);
            }

            fadeOut = false;
        }
    }

    void SetColor()
    {
        // �F���Z�b�g
        fadeImage.color = new Color(red, green, blue, alfa);
    }

    // �V�[���J�ڏ���
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // ����������
    private void Init()
    {
        // Image�R���|�[�l���g���擾
        fadeImage = GetComponent<Image>();

        // fadeImage��\��
        fadeImage.enabled = true;

        // fadeImage�̊e�F�̗v�f���擾
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;

        // �e�t���O������
        fadeOut = false;
        fadeIn = false;
        isNextScene = false;
        isBackScene = false;
    }

    void Start()
    {
        // ����������
        Init();
    }

    void Update()
    {
        // �t�F�[�h�C�����̏���
        if (fadeIn)
        {
            FadeIn();
        }

        // �t�F�[�h�A�E�g���̏���
        if (fadeOut)
        {
            FadeOut();
        }
    }

    
}

