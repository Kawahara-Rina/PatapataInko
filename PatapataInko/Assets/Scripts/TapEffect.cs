/*
    TapEffect.cs 
    
    �^�b�v���̃G�t�F�N�g���Ǘ�����N���X�B
*/
using UnityEngine;

public class TapEffect : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;   // �G�t�F�N�g�̃v���t�@�u
    [SerializeField] private float deleteTime = 2.0f;   // �G�t�F�N�g�������܂ł̎���

    // �^�b�v���A���̈ʒu�ɃG�t�F�N�g�𐶐�
    private void ShowTapEffect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //�}�E�X�J�[�\���̈ʒu���擾�B
            var mousePosition = Input.mousePosition;
            mousePosition.z = 3f;

            // �擾�������W�ɃG�t�F�N�g�𐶐�
            GameObject clone = Instantiate(effectPrefab, Camera.main.ScreenToWorldPoint(mousePosition), Quaternion.identity);

            // deleteTime��ɃG�t�F�N�g���폜
            Destroy(clone, deleteTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �^�b�v���A���̈ʒu�ɃG�t�F�N�g�𐶐�
        ShowTapEffect();

    }
}
