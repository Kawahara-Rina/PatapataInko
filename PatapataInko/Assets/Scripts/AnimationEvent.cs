/*
    AnimationEvent.cs 
    
    アニメーションイベントで呼び出す処理をまとめたクラス。
*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    // AudioSource、AudioClip取得
    AudioSource audioSource;
    public AudioClip swishSE;
    public AudioClip dropSE;

    // アニメーション実行時に効果音を鳴らす関数
    private void PlaySwishSE()
    {
        audioSource.PlayOneShot(swishSE);
    }

    // アニメーション実行時に効果音を鳴らす関数
    private void PlayDropSE()
    {
        audioSource.PlayOneShot(dropSE);
    }

    // 初期化処理
    private void Init()
    {
        // AudioSource取得
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        Init();
    }
}
