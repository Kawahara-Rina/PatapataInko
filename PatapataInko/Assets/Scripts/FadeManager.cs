/*
    FadeManager.cs 
    
    画面遷移時のフェード処理を行うクラス。
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField]private float speed;   // フェードするスピード
    private float red, green, blue;        // イメージの各色要素
    private Image fadeImage;               //フェードするパネル

    public float alfa;        // イメージの透明度

    public bool fadeOut;      // フェードアウトに使用するフラグ
    public bool fadeIn;       // フェードインに使用するフラグ
    public bool isNextScene;  // シーン遷移時に使用するフラグ
    public bool isBackScene;  // シーン遷移時に使用するフラグ


    // フェードイン時の処理
    void FadeIn()
    {
        // アルファ値を減らしていく(透明に)
        alfa -= speed * Time.deltaTime;

        // 色をセット
        SetColor();

        // アルファ値が0以下(透明)になった場合
        if (alfa <= 0)
        {
            fadeIn = false;
            fadeImage.enabled = false;
        }
    }

    // フェードアウト時の処理
    void FadeOut()
    {
        fadeImage.enabled = true;

        // アルファ値を増やす(不透明に)
        alfa += speed * Time.deltaTime;

        // 色をセット
        SetColor();

        // アルファ値が1以上(不透明)になった場合
        if (alfa >= 1)
        {

            if (isNextScene)
            {
                // ゲームシーンに遷移
                LoadScene(Define.SCENE_GAME);
            }
            if (isBackScene)
            {
                // タイトルシーンに遷移
                LoadScene(Define.SCENE_TITLE);
            }

            fadeOut = false;
        }
    }

    void SetColor()
    {
        // 色をセット
        fadeImage.color = new Color(red, green, blue, alfa);
    }

    // シーン遷移処理
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 初期化処理
    private void Init()
    {
        // Imageコンポーネントを取得
        fadeImage = GetComponent<Image>();

        // fadeImageを表示
        fadeImage.enabled = true;

        // fadeImageの各色の要素を取得
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;

        // 各フラグ初期化
        fadeOut = false;
        fadeIn = false;
        isNextScene = false;
        isBackScene = false;
    }

    void Start()
    {
        // 初期化処理
        Init();
    }

    void Update()
    {
        // フェードイン時の処理
        if (fadeIn)
        {
            FadeIn();
        }

        // フェードアウト時の処理
        if (fadeOut)
        {
            FadeOut();
        }
    }

    
}

