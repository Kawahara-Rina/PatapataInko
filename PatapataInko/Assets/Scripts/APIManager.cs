/*
    APIManager.cs 
    
    通信処理を行うクラス。
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.Linq;
using System.IO;



public class APIManager : MonoBehaviour
{
    // 通信を行うスプレッドシートのURL
    private const string URL_GAS = "https://script.google.com/macros/s/AKfycbzeSgRnyPwmnzSwyqiFF1NNO7VLexXnL0HOqsyk-VJvBT_97RejczUavkdijQ-9jHvZ/exec";
    // 追加ランキング取得数
    private const int MORE_GET = 5;
    // 通信成功
    private const int COM_SUCCESS = 200;

    // 処理に必要なオブジェクト等取得
    [SerializeField] private string accessKey;
    [SerializeField] private GameObject loadingObj;
    [SerializeField] private GameObject compleatedTextEn;
    [SerializeField] private GameObject compleatedTextJa;
    [SerializeField] private InputField nameInputField;

    // 他スクリプトから取得用
    private RankingList rList;  // ランキングのリスト
    private GameManager gm;     // ゲームマネージャ―

    // 現在取得したランキングの数
    private int getCnt;
    // ランキング最大取得数
    private int maxGet=10;
    // 順位のカウント
    private int rankCnt;

    // 前回取得したスコア、重複用のカウント
    private int befoScore;
    private int befoCnt = 0;

    // ランキングを一度取得したかどうかのフラグ
    private bool isGetRanking=false;

    public TextAsset textAsset;

    private Records jsonRecords;


    // 初期化処理
    private void Init()
    {
        //PlayerPrefs.DeleteKey("Rank");

        // ランキングリストの取得
        rList = GameObject.Find("GameManager").GetComponent<RankingList>();
        // ゲームマネージャ―の取得
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // カウント、フラグの初期化
        getCnt = 0;
        maxGet = 10;
        rankCnt=1;
        befoScore=0;
        befoCnt = 0;
        isGetRanking = false;
}
    /*
        // Get通信処理
        private IEnumerator GetData()
        {

            //データ受信開始
            var request = UnityWebRequest.Get("https://script.google.com/macros/s/" + accessKey + "/exec");
            // サーバーとの通信を開始
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // 通信成功時処理
                if (request.responseCode == COM_SUCCESS)
                {
                    // Json形式のレコードを取得
                    var records = JsonUtility.FromJson<Records>(request.downloadHandler.text).records;


                    // スクロール領域の子オブジェクト削除
                    foreach (Transform child in rList.imageScroll.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }

                    // データを順に取得
                    foreach (var record in records)
                    {

                        // 前回のスコアと同じだった場合
                        if (int.Parse(record.score)== befoScore)
                        {
                            // 重複後のランキングカウントを加算
                            befoCnt++;

                            // 順位のカウントを加算
                            rankCnt++;

                            // Jsonオブジェクトに変換
                            rList.SetList(rankCnt-befoCnt, record.name, record.score);     
                        }
                        else
                        {
                            // Jsonオブジェクトに変換
                            rList.SetList(rankCnt+1, record.name, record.score);

                            // 順位のカウントを加算
                            rankCnt++;

                            // 重複のカウントをリセット
                            befoCnt = 0;
                        }

                        // ローディング非表示
                        loadingObj.SetActive(false);

                        // ひとつ前のスコアを格納
                        befoScore = int.Parse(record.score);

                        // レコード取得数を加算
                        getCnt++;

                        // 20件取得したら終了
                        if (getCnt >= maxGet)
                        {
                            break;
                        }

                    }
                }
                else
                {
                    Debug.LogError("データ受信失敗：" + request.responseCode);
                }
            }
            else
            {
                Debug.LogError("データ受信失敗" + request.result);
            }
        }
        */

    /*
    // Post通信処理
    private IEnumerator PostCommunication(string url, string strPost)
    {
        using (var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            // Json形式文字列をByteに変換
            var postData = Encoding.UTF8.GetBytes(strPost);

            // Post通信設定
            req.uploadHandler = new UploadHandlerRaw(postData);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            // サーバーとの通信を開始
            yield return req.SendWebRequest();

            switch (req.result)
            {
                // 通信に成功したとき
                case UnityWebRequest.Result.Success:
                   
                    // 通信結果をリターン
                    yield return req.downloadHandler.text;
                    break;

                // 通信に失敗
                default:
                    Debug.Log("エラー");
                    break;
            }
        }
    }
    */

    // Get通信処理 TGS版
    /*private void GetData()
    {

        string str_File = Resources.Load<TextAsset>("JsonRankingData").ToString();
        Records json_File = JsonUtility.FromJson<Records>(str_File);

        // スコアを降順に並べ替える
        json_File.records = json_File.records.OrderByDescending(record => int.Parse(record.score)).ToArray();


        // スクロール領域の子オブジェクト削除
        foreach (Transform child in rList.imageScroll.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        rankCnt = 0; // 順位カウントのリセット
        getCnt = 0;  // レコード取得数のリセット


        foreach (Record record in json_File.records)
        {
            

            // 前回のスコアと同じだった場合
            if (int.Parse(record.score) == befoScore)
            {
                // 重複後のランキングカウントを加算
                befoCnt++;

                // 順位のカウントを加算
                rankCnt++;

                // Jsonオブジェクトに変換
                rList.SetList(rankCnt - befoCnt, record.name, record.score);
            }
            else
            {
                // Jsonオブジェクトに変換
                rList.SetList(rankCnt + 1, record.name, record.score);

                // 順位のカウントを加算
                rankCnt++;

                // 重複のカウントをリセット
                befoCnt = 0;
            }

            // ローディング非表示
            loadingObj.SetActive(false);

            // ひとつ前のスコアを格納
            befoScore = int.Parse(record.score);

            // レコード取得数を加算
            getCnt++;

            // 20件取得したら終了
            if (getCnt >= maxGet)
            {
                break;
            }
        }

        //rankCnt = 0;


    }*/

    // Jsonファイルにランキング情報を追加する処理 TGS版
    /*private void AddScoreToJsonFile(JsonPostData newRecord)
    {
        // JsonRankingDataファイルの読み込み
        string str_File = Resources.Load<TextAsset>("JsonRankingData").ToString();
        Records json_File = JsonUtility.FromJson<Records>(str_File);

        // 新しいレコードを追加
        var newRecordsList = json_File.records.ToList();
        Record newRecordEntry = new Record
        {
            name = newRecord.name,
            score = newRecord.score.ToString()
        };
        newRecordsList.Add(newRecordEntry);

        // 新しいデータでjson_Fileを更新
        json_File.records = newRecordsList.ToArray();

        // Jsonファイルに書き込み（上書き保存）
        string newJsonData = JsonUtility.ToJson(json_File);
        File.WriteAllText(Application.dataPath + "/Resources/JsonRankingData.json", newJsonData);



}
*/


    // Get通信処理 TGS版
    private void GetData()
    {
        // 
        if (PlayerPrefs.HasKey("Rank"))
        {
            var jsonRankData = PlayerPrefs.GetString("Rank");
            jsonRecords = JsonUtility.FromJson<Records>(jsonRankData);
        }
        else
        {
            jsonRecords = new Records();
            //jsonRecords.records = new Record[0];

            // 新しいレコードを追加
            var records = new Record[1];
            Record newRecordEntry = new Record
            {
                name = "a",
                score = "1"
            };
            records[0]=newRecordEntry;
            jsonRecords.records = records;

            PlayerPrefs.SetString("Rank", JsonUtility.ToJson(jsonRecords));
            print(JsonUtility.ToJson(jsonRecords));
            //jsonRecords = JsonUtility.FromJson<Records>(jsonRecords.ToString());
        }

        
        // スコアを降順に並べ替える
        jsonRecords.records = jsonRecords.records.OrderByDescending(record => int.Parse(record.score)).ToArray();


        // スクロール領域の子オブジェクト削除
        foreach (Transform child in rList.imageScroll.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        rankCnt = 0; // 順位カウントのリセット
        getCnt = 0;  // レコード取得数のリセット

        
        foreach (Record record in jsonRecords.records)
        {


            // 前回のスコアと同じだった場合
            if (int.Parse(record.score) == befoScore)
            {
                // 重複後のランキングカウントを加算
                befoCnt++;

                // 順位のカウントを加算
                rankCnt++;

                // Jsonオブジェクトに変換
                rList.SetList(rankCnt - befoCnt, record.name, record.score);
            }
            else
            {
                // Jsonオブジェクトに変換
                rList.SetList(rankCnt + 1, record.name, record.score.ToString());

                // 順位のカウントを加算
                rankCnt++;

                // 重複のカウントをリセット
                befoCnt = 0;
            }

            // ローディング非表示
            loadingObj.SetActive(false);

            // ひとつ前のスコアを格納
            befoScore = int.Parse(record.score);

            // レコード取得数を加算
            getCnt++;

            // 20件取得したら終了
            if (getCnt >= maxGet)
            {
                break;
            }
        }

        //rankCnt = 0;

        
    }


    // Jsonファイルにランキング情報を追加する処理 TGS版
    private void AddScoreToJsonFile(JsonPostData newRecord)
    {
        var jsonRankData = PlayerPrefs.GetString("Rank");
        jsonRecords = JsonUtility.FromJson<Records>(jsonRankData);

        // 新しいレコードを追加
        var newRecordsList = jsonRecords.records.ToList();
        Record newRecordEntry = new Record
        {
            name = newRecord.name,
            score = newRecord.score.ToString()
        };
        newRecordsList.Add(newRecordEntry);

        jsonRecords.records = newRecordsList.ToArray();     // 
        PlayerPrefs.SetString("Rank", JsonUtility.ToJson(jsonRecords));

    }



    // ランキング登録処理
    //private IEnumerator PostScore()
    private void PostScore()
    {
        // 登録データ作成
        var post = new JsonPostData();

        // プレイヤー名の登録
        // 未入力の場合
        if (nameInputField.text =="")
        {
            post.name = "No Name";
        }
        // 名前が入力されていれば、その名前で登録
        else
        {
            post.name = nameInputField.text;
        }

        // スコアの登録
        post.score = gm.score;

        // 登録するデータをJson形式に変換
        var jsonPost = JsonUtility.ToJson(post);

        // 通信処理を実行 TGS版でコメントアウト中
        //var coroutine = PostCommunication(URL_GAS, jsonPost);
        //yield return StartCoroutine(coroutine);

        // 通信終了
        // ローディングを非表示
        loadingObj.SetActive(false);

        // 登録完了のテキストを表示
        if (GameManager.isJapanese)
        {
            compleatedTextJa.SetActive(true);
        }
        else
        {
            compleatedTextEn.SetActive(true);
        }

        //TGS版
        AddScoreToJsonFile(post);
    }

    // ランキング登録ボタン押下時処理
    // Post通信を開始
    public void PushPostButton()
    {
        // ローディングを表示
        loadingObj.SetActive(true);

        // Post通信処理を実行
        PostScore();// TGS版
        //var coroutine = PostScore();
        //StartCoroutine(coroutine);
        
    }


    // ランキングボタン押下処理
    public void PushRankingButton()
    {
        // まだランキングを取得していなければ取得 TGS版
        //if (!isGetRanking)
        {
            // ランキング取得のフラグをオン
            isGetRanking = true;

            // ローディングを表示 TGS版
            //loadingObj.SetActive(true);

            // Get通信処理を実行
            //StartCoroutine(GetData());
            GetData();

        }
    }

    // ランキングを更に取得したい場合の処理
    public void PushMoreButton()
    {
        // ローディングを表示 TGS版
        //loadingObj.SetActive(true);
        // 最大取得数を変更
        maxGet += MORE_GET;
        // Get通信を開始
        //StartCoroutine(GetData());
        GetData();

        // ランキング順位のカウントなどを初期化
        getCnt = 0;
        rankCnt = 0;
        // 最大取得数を変更
        maxGet += MORE_GET;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        Init();
    }

}
