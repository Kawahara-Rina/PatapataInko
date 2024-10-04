/*
    RankingList.cs 
    
    �����L���O�����Z�b�g���A�\�����s���N���X�B�B
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingList : MonoBehaviour
{
    // �����L���O�̃e���v���[�g
    [SerializeField] private GameObject prefabRankingTemplate;

    // �v���t�@�u��\������X�N���[���̈�
    public GameObject imageScroll;

    // �����L���O���擾��A�����L���O�e���v���\�g�v���t�@�u�𐶐����鏈��
    public void SetList(int rank,string nameText,string scoreText)
    {
        // �v���t�@�u�𐶐�
        var temp = Instantiate(prefabRankingTemplate, imageScroll.transform);

        // ���ʁA���O�A�X�R�A���e�e�L�X�g�ɔ��f
        temp.transform.Find("rankText").GetComponent<Text>().text = rank.ToString();
        temp.transform.Find("nameText").GetComponent<Text>().text = nameText;
        temp.transform.Find("scoreText").GetComponent<Text>().text = scoreText;
    }

}
