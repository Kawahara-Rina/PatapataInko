/*
    BgMove.cs 
    
    背景画像のスクロール処理を行うクラス。
*/

using UnityEngine;
using UnityEngine.UI;

public class BgMove : MonoBehaviour
{
    // 定数
    private const float MAX_LENGTH = 1f;
    private const string PROP_NAME = "_MainTex";

    // スクロールスピード
    [SerializeField] private Vector2 speed;

    // マテリアル取得用
    private Material m_material;

    // 背景画像ループ処理
    private void BgLoop()
    {
        // ゲームオーバーでなければ加算
        // 背景画像のループ処理
        if (!GameManager.isGameOver)
        {

            // xとyの値が0 ～ 1でリピートするようにする
            var x = Mathf.Repeat(Time.time * (speed.x * SettingsManager.speedValue), MAX_LENGTH);
            var y = Mathf.Repeat(Time.time * (speed.y * SettingsManager.speedValue), MAX_LENGTH);
            var offset = new Vector2(x, y);
            m_material.SetTextureOffset(PROP_NAME, offset);

        }
    }

    // 初期化処理
    private void Init()
    {
        // マテリアル取得用
        if (GetComponent<Image>() is Image i)
        {
            m_material = i.material;
        }
    }

    private void Start()
    {
        // 初期化処理
        Init();
    }

    private void Update()
    {
        // 背景のループ処理
        BgLoop();
    }
    
}
