/*
    BgMove.cs 
    
    �w�i�摜�̃X�N���[���������s���N���X�B
*/

using UnityEngine;
using UnityEngine.UI;

public class BgMove : MonoBehaviour
{
    // �萔
    private const float MAX_LENGTH = 1f;
    private const string PROP_NAME = "_MainTex";

    // �X�N���[���X�s�[�h
    [SerializeField] private Vector2 speed;

    // �}�e���A���擾�p
    private Material m_material;

    // �w�i�摜���[�v����
    private void BgLoop()
    {
        // �Q�[���I�[�o�[�łȂ���Ή��Z
        // �w�i�摜�̃��[�v����
        if (!GameManager.isGameOver)
        {

            // x��y�̒l��0 �` 1�Ń��s�[�g����悤�ɂ���
            var x = Mathf.Repeat(Time.time * (speed.x * SettingsManager.speedValue), MAX_LENGTH);
            var y = Mathf.Repeat(Time.time * (speed.y * SettingsManager.speedValue), MAX_LENGTH);
            var offset = new Vector2(x, y);
            m_material.SetTextureOffset(PROP_NAME, offset);

        }
    }

    // ����������
    private void Init()
    {
        // �}�e���A���擾�p
        if (GetComponent<Image>() is Image i)
        {
            m_material = i.material;
        }
    }

    private void Start()
    {
        // ����������
        Init();
    }

    private void Update()
    {
        // �w�i�̃��[�v����
        BgLoop();
    }
    
}
