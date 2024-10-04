/*
    DropAnimation.cs 
    
    UI�̗����A�j���[�V�������Ǘ�����N���X�B
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropAnimation : MonoBehaviour
{
    // �萔
    private const float WAIT_TIME = 0.2f;  // ���b�҂����Ɏg�p���鎞��
    private const int BUTTONS_NUMBER = 2;  // �A�j���[�V���������s�������{�^���̐�


    // �A�j���[�V���������s�������{�^�����i�[����z��
    [SerializeField] private GameObject[] buttons = new GameObject[BUTTONS_NUMBER];
    // Animator�擾�p�z��
    private Animator[] animator = new Animator[BUTTONS_NUMBER];
    

    // �Q�[���I�[�o�[���̃{�^���A�j���[�V�����̍Đ�
    private IEnumerator SetShowTrigger()
    {
        // �A�j���[�V�����̍Đ��@���ɃA�j���[�V�������Đ�
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(true);
            animator[i].SetTrigger("Show");

            // WAIT_TIME�b�҂�����A���̏��������s
            yield return new WaitForSeconds(WAIT_TIME);

        }

    }

    // �A�j���[�V�������s����
    private void PlayAnimation()
    {
        // �Q�[���I�[�o�[�̏ꍇ
        if (GameManager.isGameOver)
        {
            StartCoroutine(SetShowTrigger());
        }
    }

    // ����������
    private void Init()
    {
        // �e�{�^���̃A�j���[�^�[���擾
        for (int i = 0; i < buttons.Length; i++)
        {
            animator[i] = buttons[i].GetComponent<Animator>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // ����������
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // �A�j���[�V�������s
        PlayAnimation();
    }
}
