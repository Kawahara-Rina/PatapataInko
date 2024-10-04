/*
    PlayerManager.cs 
    
    プレイヤーの動きを制御するクラス。
*/

using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // 定数
    // プレイヤーの最大・最小角度、加える角度
    private const int MAX_ANGLE = 45;
    private const int MIN_ANGLE = -90;
    private const float ADD_ANGLE = 150f;
    private const float ADD_UP_ANGLE = 100f;

    [SerializeField]private float jump;   // ジャンプ力

    [SerializeField]private GameObject damageImage;  // ゲームオーバー時表示するイメージ

    // ポジション、ローテーション、Rigidbodyなどパラメーター取得用
    private Vector3 startPos; // スタート時のポジション
    private Vector3 pos;      // ポジション
    private Rigidbody2D rb;   // Rigidbody取得用
    private float lotZ;       // z軸の角度

    private bool isStart;     // ゲーム開始時のフラグ
    public bool isDie;        // 障害物に当たったかどうかのフラグ

    // AudioSource、AudioClip取得用
    private AudioSource audioSource;
    public AudioClip flySE;

    // GameManager取得
    private GameManager gm;

    // 何かに当たった場合の処理
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // 障害物に当たった場合
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            // ゲームオーバー時の処理を実行
            GameOver();
        }

    }

    // ゲームオーバー時の処理
    private void GameOver()
    {
        // 一度だけ実行
        if (!isDie)
        {
            isDie = true;

            // ゲームオーバー時のイメージを表示
            damageImage.SetActive(true);

            // 効果音を鳴らす
            gm.PlayHitSE();

            // 下方向に力を加える
            rb.AddForce(Vector2.down * jump, ForceMode2D.Impulse);
        }
    }

    // ポジションを取得する処理
    private void GetPos()
    {
        // 現在のポジションを取得
        pos = transform.position;
    }

    // flySEを鳴らす処理
    private void PlayFlySE()
    {
        // ポーズ中でなければ効果音を鳴らす
        if (!gm.isPause)
        {
            audioSource.PlayOneShot(flySE);
        }
    }

    // 初期化処理
    private void init()
    {
        // 各コンポーネント取得
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // 各値初期化
        lotZ = 0;
        isStart = false;

        // 初期座標を代入
        startPos = transform.position;

    }

    // スタート前の処理
    private void BeforeStarting()
    {
        // 回転させないため角度は0度
        lotZ = 0;

        // ゲームスタートまでy座標はスタートポジション固定
        transform.position = new Vector3(pos.x, startPos.y, pos.z);

        // タップで上昇 一度だけ実行
        if (Input.GetMouseButtonDown(0))
        {
            // ゲームスタート
            isStart = true;
        }
    }

    // 角度、落下処理
    private void PlayerMove()
    {
        // 一時停止された場合
        if (gm.isPause)
        {
            //Rigidbodyを停止
            rb.velocity = Vector3.zero;

            //重力を停止させる
            rb.isKinematic = true;

        }
        // 一時停止していない場合
        else
        {
            // 重力を再開、発生
            rb.isKinematic = false;

            // 下降中
            if (rb.velocity.y < 0)
            {
                // 現在の角度が最小の角度でない場合角度を減らす
                if (lotZ > MIN_ANGLE)
                {
                    lotZ -= ADD_ANGLE * Time.deltaTime;
                }
            }
            // 上昇中
            else
            {
                // 現在の角度が最大の角度でない場合角度を増やす
                if (lotZ < MAX_ANGLE)
                {
                    // 上を向く速度を少し早めるため+ADD_UP_ANGLE
                    lotZ += (ADD_ANGLE + ADD_UP_ANGLE) * Time.deltaTime;
                }
            }
        }

        // ゲームオーバーしていない場合
        if (!isDie)
        {
            // 角度を変更
            transform.eulerAngles = new Vector3(0, 0, lotZ);

            // タップで上昇
            if (Input.GetMouseButtonDown(0))
            {
                // ポーズ中でなければ効果音を鳴らす
                PlayFlySE();

                // 落下速度を一度リセットする(加速し続けないため)
                rb.velocity = Vector2.zero;

                // 上方向に力を加える
                rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        init();
    }

    // Update is called once per frame
    void Update()
    {
        // ポジションを取得
        GetPos();

        // ゲームスタートまでの処理
        if (!isStart)
        {
            BeforeStarting();
        }

        // 角度変更、落下処理
        PlayerMove();
    }
}
