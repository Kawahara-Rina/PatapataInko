/*
    SettingsManager.cs 
    
    音量やスクロール速度、言語などの設定を管理するクラス。
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // スピード、ボリューム設定のスライダー
    [SerializeField]private Slider speedSlider;
    [SerializeField]private Slider volumeSlider;

    // スピード、ボリューム設定の値を表示するテキスト
    [SerializeField]private Text speedText;
    [SerializeField]private Text volumeText;

    // 言語設定のトグル
    [SerializeField]private Toggle jaToggle;
    [SerializeField]private Toggle enToggle;

    // 保存されているプレイヤーデータ
    private JsonPlayerData playerSpeedData;
    private JsonPlayerData playerVolumeData;

    // スピード、ボリュームの値
    public static float speedValue;
    public static float volumeValue;

    // プラスボタン押下で値を増やす関数 - スピード
    public void AddSpeedValue()
    {
        speedSlider.value += 0.1f;
    }

    // マイナスボタン押下で値を減らす関数 - スピード
    public void ReduceSpeedValue()
    {
        speedSlider.value -= 0.1f;
    }

    // プラスボタン押下で値を増やす関数 - ボリューム
    public void AddVolumeValue()
    {
        volumeSlider.value += 0.1f;
    }

    // マイナスボタン押下で値を減らす関数 - ボリューム
    public void ReduceVolumeValue()
    {
        volumeSlider.value -= 0.1f;
    }

    // 設定記録処理(値変更時に記録) - スピード
    public void PlayerSpeedSettingsSave()
    {
        // オブジェクトに値を格納
        playerSpeedData = new JsonPlayerData();

        // スピードの値を格納
        playerSpeedData.speValue = speedSlider.value;

        // オブジェクトをJSON形式に変換
        PlayerPrefs.SetString("SpeedValue", JsonUtility.ToJson(playerSpeedData));

        // スライダーの値を表示
        speedText.text = speedSlider.value.ToString("N1");
    }

    // 設定記録処理(値変更時に記録) - ボリューム
    public void PlayerVolumeSettingsSave()
    {
        // オブジェクトに値を格納
        playerVolumeData = new JsonPlayerData();

        // ボリュームの値を格納
        playerVolumeData.volValue = volumeSlider.value;

        // オブジェクトをJSON形式に変換
        PlayerPrefs.SetString("VolumeValue", JsonUtility.ToJson(playerVolumeData));

        // スライダーの値を表示
        volumeText.text = volumeSlider.value.ToString("N1");
    }

    // 保存されていた値を取得してセットする処理
    private void SetPlayerData()
    {
        // playerSpeedDataが保存されていれば取得
        if (playerSpeedData != null)
        {
            speedValue = playerSpeedData.speValue;
            speedSlider.value = speedValue;
        }
        // playerVolumeDataが保存されていれば取得
        if (playerVolumeData != null)
        {
            volumeValue = playerVolumeData.volValue;
            volumeSlider.value = volumeValue;
        }
    }

    // スピード、ボリュームの値をセットする処理
    private void SetValue()
    {
        // スピード、ボリュームの代入
        var val = speedSlider.value * 10f;  // 小数点1桁以下切り捨て
        speedValue = Mathf.Floor(val) / 10f;

        volumeValue = volumeSlider.value;
    }

    private void Init()
    {
        // スピード、ボリュームの値をスライダーの値に設定
        speedValue = speedSlider.value;
        volumeValue = volumeSlider.value;

        // トグルの設定
        if (GameManager.isJapanese)
        {
            jaToggle.isOn = true;
        }
        else
        {
            enToggle.isOn = true;
        }

        // スピードの値が保存されているか確認
        if (PlayerPrefs.HasKey("SpeedValue"))
        {
            // 保存されていたら値を取得
            var jsonpData = PlayerPrefs.GetString("SpeedValue");

            // 取得したJsonデータをオブジェクトに変換
            playerSpeedData = JsonUtility.FromJson<JsonPlayerData>(jsonpData);

            // プレイヤーデータを代入
            SetPlayerData();
        }

        // ボリュームの値が保存されているか確認
        if (PlayerPrefs.HasKey("VolumeValue"))
        {
            // 保存されていたら値を取得
            var jsonpData = PlayerPrefs.GetString("VolumeValue");

            // 取得したJsonデータをオブジェクトに変換
            playerVolumeData = JsonUtility.FromJson<JsonPlayerData>(jsonpData);

            // プレイヤーデータを代入
            SetPlayerData();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // スピード、ボリュームの値をセット
        SetValue();
    }
}
