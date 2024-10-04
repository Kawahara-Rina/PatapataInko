/*
    JsonPlayerData.cs 
    
    �v���C���[�̃f�[�^��[�����Ƃɕۑ����邽�߂̃N���X�B
*/

using UnityEngine;

[System.Serializable]
public class JsonPlayerData
{
    public int highScore;    // �v���C���[�̃n�C�X�R�A
    public int[] score=new int[10];
    public string[] name=new string[10];
    public int num;
    
    // �ݒ�֘A
    public bool isJapanese;  // ���ꂪ���{�ꂩ�ǂ���
    public float speValue;   // �X�s�[�h�̒l
    public float volValue;   // �{�����[���̒l

}
