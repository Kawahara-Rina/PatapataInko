/*
    WallManager.cs 
    
    ��Q���̊Ǘ�������N���X�B
*/
using UnityEngine;

public class WallManager : MonoBehaviour
{
    // �ړ����x�ƃf�t�H���g�|�W�V����
    [SerializeField] private float speed;
    [SerializeField] private float defPos;

    // ���g�̃|�W�V����
    private Vector3 position;

    // �J�����̍��[���W
    private Vector3 screenLeftBottom;

    // �������G�ꂽ���ǂ����̃t���O
    private bool isTouch = false;

    // GameManager�擾�p
    GameManager gm;

    // �����ɐG�ꂽ���̏���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �G�ꂽ��������A1�x�������s
        if (!isTouch)
        {
            // �X�R�A�����Z
            gm.score++;
            isTouch = true;
        }
    }

    // �������O�ꂽ�Ƃ��̏���
    private void OnTriggerExit2D(Collider2D collision)
    {
        // �t���O��������
        isTouch = false;
    }

    // �y�ǂ𓮂�������
    private void ClayPipeMove()
    {
        // �Q�[���I�[�o�[�A�|�[�Y�ȊO�̏ꍇ�Ɏ��s
        if (!GameManager.isGameOver && !gm.isPause)
        {
            // �Q�[���X�^�[�g���Ă���Ό��Z����
            if (gm.isStart)
            {
                // x���W��speed�̑����Ԃ񌸎Z
                position.x -= (speed * SettingsManager.speedValue) * Time.deltaTime;
            }
        }

        // x���W�����Z�b�g�|�W�V�����ɓ��B�����ꍇ
        if (position.x < screenLeftBottom.x - 1f)
        {
            // x���W�Ƀf�t�H���g�|�W�V��������
            position.x = defPos;

            // y���W�������_���̒l�ɕύX
            position.y = Random.Range(5.5f, 15.5f);

        }

        // �|�W�V������ύX
        transform.position = new Vector3(position.x, position.y, position.z);
    }

    // ����������
    private void Init()
    {
        // �I�u�W�F�N�g�̏����ʒu���擾
        position = transform.position;

        // �J�����̍��[���W�擾
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);

        // GameManager�X�N���v�g�擾
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

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
        // �y�ǂ𓮂�������
        ClayPipeMove();
    }
}
