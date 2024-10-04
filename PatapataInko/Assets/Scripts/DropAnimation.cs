/*
    DropAnimation.cs 
    
    UIの落下アニメーションを管理するクラス。
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropAnimation : MonoBehaviour
{
    // 定数
    private const float WAIT_TIME = 0.2f;  // 数秒待つ処理に使用する時間
    private const int BUTTONS_NUMBER = 2;  // アニメーションを実行したいボタンの数


    // アニメーションを実行したいボタンを格納する配列
    [SerializeField] private GameObject[] buttons = new GameObject[BUTTONS_NUMBER];
    // Animator取得用配列
    private Animator[] animator = new Animator[BUTTONS_NUMBER];
    

    // ゲームオーバー時のボタンアニメーションの再生
    private IEnumerator SetShowTrigger()
    {
        // アニメーションの再生　順にアニメーションを再生
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(true);
            animator[i].SetTrigger("Show");

            // WAIT_TIME秒待った後、次の処理を実行
            yield return new WaitForSeconds(WAIT_TIME);

        }

    }

    // アニメーション実行処理
    private void PlayAnimation()
    {
        // ゲームオーバーの場合
        if (GameManager.isGameOver)
        {
            StartCoroutine(SetShowTrigger());
        }
    }

    // 初期化処理
    private void Init()
    {
        // 各ボタンのアニメーターを取得
        for (int i = 0; i < buttons.Length; i++)
        {
            animator[i] = buttons[i].GetComponent<Animator>();
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
        // アニメーション実行
        PlayAnimation();
    }
}
