/*
    AnimationEvent.cs 
    
    �A�j���[�V�����C�x���g�ŌĂяo���������܂Ƃ߂��N���X�B
*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    // AudioSource�AAudioClip�擾
    AudioSource audioSource;
    public AudioClip swishSE;
    public AudioClip dropSE;

    // �A�j���[�V�������s���Ɍ��ʉ���炷�֐�
    private void PlaySwishSE()
    {
        audioSource.PlayOneShot(swishSE);
    }

    // �A�j���[�V�������s���Ɍ��ʉ���炷�֐�
    private void PlayDropSE()
    {
        audioSource.PlayOneShot(dropSE);
    }

    // ����������
    private void Init()
    {
        // AudioSource�擾
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // ����������
        Init();
    }
}
