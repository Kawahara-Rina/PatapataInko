/*
    TapEffect.cs 
    
    タップ時のエフェクトを管理するクラス。
*/
using UnityEngine;

public class TapEffect : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;   // エフェクトのプレファブ
    [SerializeField] private float deleteTime = 2.0f;   // エフェクトを消すまでの時間

    // タップ時、その位置にエフェクトを生成
    private void ShowTapEffect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //マウスカーソルの位置を取得。
            var mousePosition = Input.mousePosition;
            mousePosition.z = 3f;

            // 取得した座標にエフェクトを生成
            GameObject clone = Instantiate(effectPrefab, Camera.main.ScreenToWorldPoint(mousePosition), Quaternion.identity);

            // deleteTime後にエフェクトを削除
            Destroy(clone, deleteTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // タップ時、その位置にエフェクトを生成
        ShowTapEffect();

    }
}
