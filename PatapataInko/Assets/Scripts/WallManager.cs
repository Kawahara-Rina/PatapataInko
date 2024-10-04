/*
    WallManager.cs 
    
    障害物の管理をするクラス。
*/
using UnityEngine;

public class WallManager : MonoBehaviour
{
    // 移動速度とデフォルトポジション
    [SerializeField] private float speed;
    [SerializeField] private float defPos;

    // 自身のポジション
    private Vector3 position;

    // カメラの左端座標
    private Vector3 screenLeftBottom;

    // 何かが触れたかどうかのフラグ
    private bool isTouch = false;

    // GameManager取得用
    GameManager gm;

    // 何かに触れた時の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 触れた判定を取り、1度だけ実行
        if (!isTouch)
        {
            // スコアを加算
            gm.score++;
            isTouch = true;
        }
    }

    // 何かが外れたときの処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        // フラグを初期化
        isTouch = false;
    }

    // 土管を動かす処理
    private void ClayPipeMove()
    {
        // ゲームオーバー、ポーズ以外の場合に実行
        if (!GameManager.isGameOver && !gm.isPause)
        {
            // ゲームスタートしていれば減算処理
            if (gm.isStart)
            {
                // x座標をspeedの速さぶん減算
                position.x -= (speed * SettingsManager.speedValue) * Time.deltaTime;
            }
        }

        // x座標がリセットポジションに到達した場合
        if (position.x < screenLeftBottom.x - 1f)
        {
            // x座標にデフォルトポジションを代入
            position.x = defPos;

            // y座標をランダムの値に変更
            position.y = Random.Range(5.5f, 15.5f);

        }

        // ポジションを変更
        transform.position = new Vector3(position.x, position.y, position.z);
    }

    // 初期化処理
    private void Init()
    {
        // オブジェクトの初期位置を取得
        position = transform.position;

        // カメラの左端座標取得
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);

        // GameManagerスクリプト取得
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // 土管を動かす処理
        ClayPipeMove();
    }
}
