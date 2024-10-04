/*
    PlayerManager.cs 
    
    �v���C���[�̓����𐧌䂷��N���X�B
*/

using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // �萔
    // �v���C���[�̍ő�E�ŏ��p�x�A������p�x
    private const int MAX_ANGLE = 45;
    private const int MIN_ANGLE = -90;
    private const float ADD_ANGLE = 150f;
    private const float ADD_UP_ANGLE = 100f;

    [SerializeField]private float jump;   // �W�����v��

    [SerializeField]private GameObject damageImage;  // �Q�[���I�[�o�[���\������C���[�W

    // �|�W�V�����A���[�e�[�V�����ARigidbody�Ȃǃp�����[�^�[�擾�p
    private Vector3 startPos; // �X�^�[�g���̃|�W�V����
    private Vector3 pos;      // �|�W�V����
    private Rigidbody2D rb;   // Rigidbody�擾�p
    private float lotZ;       // z���̊p�x

    private bool isStart;     // �Q�[���J�n���̃t���O
    public bool isDie;        // ��Q���ɓ����������ǂ����̃t���O

    // AudioSource�AAudioClip�擾�p
    private AudioSource audioSource;
    public AudioClip flySE;

    // GameManager�擾
    private GameManager gm;

    // �����ɓ��������ꍇ�̏���
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // ��Q���ɓ��������ꍇ
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            // �Q�[���I�[�o�[���̏��������s
            GameOver();
        }

    }

    // �Q�[���I�[�o�[���̏���
    private void GameOver()
    {
        // ��x�������s
        if (!isDie)
        {
            isDie = true;

            // �Q�[���I�[�o�[���̃C���[�W��\��
            damageImage.SetActive(true);

            // ���ʉ���炷
            gm.PlayHitSE();

            // �������ɗ͂�������
            rb.AddForce(Vector2.down * jump, ForceMode2D.Impulse);
        }
    }

    // �|�W�V�������擾���鏈��
    private void GetPos()
    {
        // ���݂̃|�W�V�������擾
        pos = transform.position;
    }

    // flySE��炷����
    private void PlayFlySE()
    {
        // �|�[�Y���łȂ���Ό��ʉ���炷
        if (!gm.isPause)
        {
            audioSource.PlayOneShot(flySE);
        }
    }

    // ����������
    private void init()
    {
        // �e�R���|�[�l���g�擾
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // �e�l������
        lotZ = 0;
        isStart = false;

        // �������W����
        startPos = transform.position;

    }

    // �X�^�[�g�O�̏���
    private void BeforeStarting()
    {
        // ��]�����Ȃ����ߊp�x��0�x
        lotZ = 0;

        // �Q�[���X�^�[�g�܂�y���W�̓X�^�[�g�|�W�V�����Œ�
        transform.position = new Vector3(pos.x, startPos.y, pos.z);

        // �^�b�v�ŏ㏸ ��x�������s
        if (Input.GetMouseButtonDown(0))
        {
            // �Q�[���X�^�[�g
            isStart = true;
        }
    }

    // �p�x�A��������
    private void PlayerMove()
    {
        // �ꎞ��~���ꂽ�ꍇ
        if (gm.isPause)
        {
            //Rigidbody���~
            rb.velocity = Vector3.zero;

            //�d�͂��~������
            rb.isKinematic = true;

        }
        // �ꎞ��~���Ă��Ȃ��ꍇ
        else
        {
            // �d�͂��ĊJ�A����
            rb.isKinematic = false;

            // ���~��
            if (rb.velocity.y < 0)
            {
                // ���݂̊p�x���ŏ��̊p�x�łȂ��ꍇ�p�x�����炷
                if (lotZ > MIN_ANGLE)
                {
                    lotZ -= ADD_ANGLE * Time.deltaTime;
                }
            }
            // �㏸��
            else
            {
                // ���݂̊p�x���ő�̊p�x�łȂ��ꍇ�p�x�𑝂₷
                if (lotZ < MAX_ANGLE)
                {
                    // ����������x���������߂邽��+ADD_UP_ANGLE
                    lotZ += (ADD_ANGLE + ADD_UP_ANGLE) * Time.deltaTime;
                }
            }
        }

        // �Q�[���I�[�o�[���Ă��Ȃ��ꍇ
        if (!isDie)
        {
            // �p�x��ύX
            transform.eulerAngles = new Vector3(0, 0, lotZ);

            // �^�b�v�ŏ㏸
            if (Input.GetMouseButtonDown(0))
            {
                // �|�[�Y���łȂ���Ό��ʉ���炷
                PlayFlySE();

                // �������x����x���Z�b�g����(�����������Ȃ�����)
                rb.velocity = Vector2.zero;

                // ������ɗ͂�������
                rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // ����������
        init();
    }

    // Update is called once per frame
    void Update()
    {
        // �|�W�V�������擾
        GetPos();

        // �Q�[���X�^�[�g�܂ł̏���
        if (!isStart)
        {
            BeforeStarting();
        }

        // �p�x�ύX�A��������
        PlayerMove();
    }
}
