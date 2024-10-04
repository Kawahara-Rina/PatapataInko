/*
    GameManager.cs 
    
    ゲーム中のメイン処理を行うクラス。
*/

using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 使用するUI関連
    // スコア、リザルト、ハイスコアのテキスト
    [SerializeField] private Text scoreText;
    [SerializeField] private Text scoreResultText;
    [SerializeField] private Text highScoreText;

    [SerializeField] private Text countdownText;        // 中断から再開時に表示するカウントダウン
    [SerializeField] private GameObject startGuide;     // 操作ガイドUI
    [SerializeField] private GameObject gameoverPanel;  // ゲームオーバー時に表示するパネル
    [SerializeField] private GameObject bestImage;      // ベストスコア更新時に表示する画像
    [SerializeField] private GameObject pauseButton;    // ポーズボタン

    // 使用する効果音関連
    // 画面遷移時の効果音、何かに当たった時の効果音
    [SerializeField] private AudioClip transSE;
    [SerializeField] private AudioClip hitSE;
    // 音量を調節するためのAudioMixer
    [SerializeField] private AudioMixer audioMixer;

    private FadeManager fm;          // FadeManager取得用(FadeManager:画面遷移時の処理)
    private PlayerManager pm;        // PlayerManager取得用(PlayerManager:プレイヤーの制御処理)

    private float startCount;        // 中断から再開時に使用するカウント
    private bool isBest;             // ベストスコアを更新したかどかのフラグ

    private Animator animator;       // Animator取得用
    private Animator goAnimator;     // ゲームオーバーパネルのアニメーター取得用
    private AudioSource audioSource; // AudioSource取得用

    private JsonPlayerData playerData;         // 保存されているプレイヤーデータ
    private JsonPlayerData playerSettingsData; // 保存されているプレイヤー設定データ

    // 日本語、英語版のテキスト取得用配列
    private GameObject[] japaneseText;
    private GameObject[] englishText;

    public static bool isGameOver;             // ゲームオーバーかどうかのフラグ
    public static bool isJapanese = true;      // 選択中の言語は日本語かどうかのフラグ
    public bool isStart;                       // ゲーム開始フラグ
    public int score;                          // ゲーム中のスコア
    public int highScore;                      // ハイスコア
    public bool isPause;                       // 停止ボタンが押されているかどうかのフラグ
    public bool isRestart;                     // リスタートボタンが押されているかどうかのフラグ

    // 言語設定の日本語ボタン押下時処理
    public void TapJapaneseButton()
    {
        isJapanese = true;
    }

    // 言語設定の英語ボタン押下時処理
    public void TapEnglishButton()
    {
        isJapanese = false;
    }

    // ポーズボタン押下で一時停止
    public void TapPauseButton()
    {
        isPause = true;
    }

    // スタートボタン押下で再開
    public void TapStartButton()
    {
        startCount = 3.5f;
        isRestart = true;
        //isPause = false;

    }

    // プレイボタン押下でタイトル画面からゲーム画面へ遷移
    public void TapPlayButton()
    {
        fm.fadeOut = true;
        fm.isNextScene = true;
    }

    // タイトルに戻るボタン押下でゲーム画面からタイトル画面へ遷移
    public void TapBackButton()
    {
        fm.fadeOut = true;
        fm.isBackScene = true;
    }

    // 画面遷移時の効果音を鳴らす処理
    public void PlayTransSE()
    {
        audioSource.PlayOneShot(transSE);
    }

    // 何かに当たった時の効果音を鳴らす処理
    public void PlayHitSE()
    {
        audioSource.PlayOneShot(hitSE);
    }

    // AudioMixerの音量を変更する処理
    public void SetAudioMixerSE(float value)
    {
        //-80~0に変換
        var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
        //audioMixerに代入
        audioMixer.SetFloat("SE", volume);
    }


    // Start is called before the first frame update
    void Start()
    {
        // カーソル非表示
        //Cursor.visible = false;

        // 日本語、英語版のテキストを取得
        japaneseText = GameObject.FindGameObjectsWithTag("JaText");
        englishText = GameObject.FindGameObjectsWithTag("EnText");

        gameoverPanel.SetActive(false);

        countdownText.enabled = false;

        fm =GameObject.Find("fadePanel").GetComponent<FadeManager>();
        pm =GameObject.Find("SpPlayer").GetComponent<PlayerManager>();
        
        // シーン遷移時フェードイン
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

        // ハイスコアが保存されているか確認
        if (PlayerPrefs.HasKey("HighScore"))
        {
            // 保存されていたら値を取得
            var jsonpData = PlayerPrefs.GetString("HighScore");

            // 取得したJsonデータをオブジェクトに変換
            playerData = JsonUtility.FromJson<JsonPlayerData>(jsonpData);

            // プレイヤーデータを代入
            SetPlayerData();
        }

        // 言語設定が保存されているか確認
        if (PlayerPrefs.HasKey("Languages"))
        {
            // 保存されていたら値を取得
            var jsonpData = PlayerPrefs.GetString("Languages");

            // 取得したJsonデータをオブジェクトに変換
            playerSettingsData = JsonUtility.FromJson<JsonPlayerData>(jsonpData);

            // プレイヤーデータを代入
            SetPlayerData();
        }

    }

    // 保存されていた値をセット
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

    // データ記録処理
    void PlayerDataSave()
    {
        // オブジェクトに値を格納
        playerData = new JsonPlayerData();

        playerData.highScore = highScore;     // ハイスコア格納

        // オブジェクトをJSON形式に変換
        PlayerPrefs.SetString("HighScore", JsonUtility.ToJson(playerData));
    }

    // 設定記録処理
    public void PlayerSettingsSave()
    {
        // オブジェクトに値を格納
        playerSettingsData = new JsonPlayerData();

        playerSettingsData.isJapanese = isJapanese;   // 言語設定格納

        // オブジェクトをJSON形式に変換
        PlayerPrefs.SetString("Languages", JsonUtility.ToJson(playerSettingsData));
    }

    // ゲーム再開時処理
    private void GameRestart()
    {
        if (isRestart)
        {
            // カウントダウンを表示する
            countdownText.enabled = true;
            startCount -= Time.deltaTime;
            countdownText.text = startCount.ToString("F0");

            // 一定時間経過後ゲーム再開
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

    // スタートガイドの非表示処理
    private void StartGuideFadeOut()
    {
        // ゲームスタートしていないときにタップ
        if (!isStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // ガイドをフェードアウトさせ、ゲーム開始フラグをオン
                animator.SetTrigger("FadeOut");
                isStart = true;
            }
        }
    }

    // UI表示処理
    private void DisplayUI()
    {
        // スコア関連のテキスト表示
        scoreText.text = String.Format("{0}", score);
        scoreResultText.text = String.Format("{0}", score);
        highScoreText.text = String.Format("{0}", highScore);

        // テキストは日本語か英語かを判定、表示非表示
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

    // ハイスコア更新処理
    private void HighScoreUpdate()
    {
        // ハイスコア更新していれば、フラグをオンにし
        // ハイスコアを記録
        if (score > highScore)
        {
            isBest = true;
            highScore = score;
            PlayerDataSave();
        }
    }

    // ゲームオーバー時処理
    private void GameOver()
    {
        if (pm.isDie && !isGameOver)
        {
            // 画面が揺れる演出
            var impulseSource = GetComponent<CinemachineImpulseSource>();
            impulseSource.GenerateImpulse();

            gameoverPanel.SetActive(true);

            // ベスト更新していた場合はアニメーショントリガーを発火
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
        // スタート前のガイド非表示処理
        StartGuideFadeOut();

        // ゲーム再開時処理
        GameRestart();

        // UI表示処理
        DisplayUI();

        // ハイスコア更新処理
        HighScoreUpdate();

        // ゲームオーバー時処理
        GameOver();
    }
}
