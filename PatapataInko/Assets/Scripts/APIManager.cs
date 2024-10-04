/*
    APIManager.cs 
    
    �ʐM�������s���N���X�B
*/
#define InternetCommunication
//#define LocalCommunication

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class APIManager : MonoBehaviour
{
    // �ʐM���s���X�v���b�h�V�[�g��URL
    private const string URL_GAS = "https://script.google.com/macros/s/AKfycbzeSgRnyPwmnzSwyqiFF1NNO7VLexXnL0HOqsyk-VJvBT_97RejczUavkdijQ-9jHvZ/exec";
    // �ǉ������L���O�擾��
    private const int MORE_GET = 5;
    // �ʐM����
    private const int COM_SUCCESS = 200;

    // �����ɕK�v�ȃI�u�W�F�N�g���擾
    [SerializeField] private string accessKey;
    [SerializeField] private GameObject loadingObj;
    [SerializeField] private GameObject compleatedTextEn;
    [SerializeField] private GameObject compleatedTextJa;
    [SerializeField] private InputField nameInputField;

    // ���X�N���v�g����擾�p
    private RankingList rList;  // �����L���O�̃��X�g
    private GameManager gm;     // �Q�[���}�l�[�W���\

    // ���ݎ擾���������L���O�̐�
    private int getCnt;
    // �����L���O�ő�擾��
    private int maxGet=10;
    // ���ʂ̃J�E���g
    private int rankCnt;

    // �O��擾�����X�R�A�A�d���p�̃J�E���g
    private int befoScore;
    private int befoCnt = 0;

    // �����L���O����x�擾�������ǂ����̃t���O
    private bool isGetRanking=false;

    public TextAsset textAsset;

    private Records jsonRecords;


    // ����������
    private void Init()
    {
        //PlayerPrefs.DeleteKey("Rank");

        // �����L���O���X�g�̎擾
        rList = GameObject.Find("GameManager").GetComponent<RankingList>();
        // �Q�[���}�l�[�W���\�̎擾
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // �J�E���g�A�t���O�̏�����
        getCnt = 0;
        maxGet = 10;
        rankCnt = 1;
        befoScore = 0;
        befoCnt = 0;
        isGetRanking = false;
    }

// �C���^�[�l�b�g���g�p���ĒʐM���s���ꍇ�̏���
#if InternetCommunication
    // Get�ʐM����
    private IEnumerator GetData()
    {

        //�f�[�^��M�J�n
        var request = UnityWebRequest.Get("https://script.google.com/macros/s/" + accessKey + "/exec");
        // �T�[�o�[�Ƃ̒ʐM���J�n
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // �ʐM����������
            if (request.responseCode == COM_SUCCESS)
            {
                // Json�`���̃��R�[�h���擾
                var records = JsonUtility.FromJson<Records>(request.downloadHandler.text).records;


                // �X�N���[���̈�̎q�I�u�W�F�N�g�폜
                foreach (Transform child in rList.imageScroll.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }

                // �f�[�^�����Ɏ擾
                foreach (var record in records)
                {

                    // �O��̃X�R�A�Ɠ����������ꍇ
                    if (int.Parse(record.score) == befoScore)
                    {
                        // �d����̃����L���O�J�E���g�����Z
                        befoCnt++;

                        // ���ʂ̃J�E���g�����Z
                        rankCnt++;

                        // Json�I�u�W�F�N�g�ɕϊ�
                        rList.SetList(rankCnt - befoCnt, record.name, record.score);
                    }
                    else
                    {
                        // Json�I�u�W�F�N�g�ɕϊ�
                        rList.SetList(rankCnt + 1, record.name, record.score);

                        // ���ʂ̃J�E���g�����Z
                        rankCnt++;

                        // �d���̃J�E���g�����Z�b�g
                        befoCnt = 0;
                    }

                    // ���[�f�B���O��\��
                    loadingObj.SetActive(false);

                    // �ЂƂO�̃X�R�A���i�[
                    befoScore = int.Parse(record.score);

                    // ���R�[�h�擾�������Z
                    getCnt++;

                    // 20���擾������I��
                    if (getCnt >= maxGet)
                    {
                        break;
                    }

                }
            }
            else
            {
                Debug.LogError("�f�[�^��M���s�F" + request.responseCode);
            }
        }
        else
        {
            Debug.LogError("�f�[�^��M���s" + request.result);
        }
    }

    
    // Post�ʐM����
    private IEnumerator PostCommunication(string url, string strPost)
    {
        using (var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            // Json�`���������Byte�ɕϊ�
            var postData = Encoding.UTF8.GetBytes(strPost);

            // Post�ʐM�ݒ�
            req.uploadHandler = new UploadHandlerRaw(postData);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            // �T�[�o�[�Ƃ̒ʐM���J�n
            yield return req.SendWebRequest();

            switch (req.result)
            {
                // �ʐM�ɐ��������Ƃ�
                case UnityWebRequest.Result.Success:
                   
                    // �ʐM���ʂ����^�[��
                    yield return req.downloadHandler.text;
                    break;

                // �ʐM�Ɏ��s
                default:
                    Debug.Log("�G���[");
                    break;
            }
        }
    }


    // ���[�J���ʐM��
#elif LocalCommunication

    // Get�ʐM���� TGS��
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

            // �V�������R�[�h��ǉ�
            var records = new Record[1];
            Record newRecordEntry = new Record
            {
                name = "a",
                score = "1"
            };
            records[0] = newRecordEntry;
            jsonRecords.records = records;

            PlayerPrefs.SetString("Rank", JsonUtility.ToJson(jsonRecords));
            print(JsonUtility.ToJson(jsonRecords));
            //jsonRecords = JsonUtility.FromJson<Records>(jsonRecords.ToString());
        }


        // �X�R�A���~���ɕ��בւ���
        jsonRecords.records = jsonRecords.records.OrderByDescending(record => int.Parse(record.score)).ToArray();


        // �X�N���[���̈�̎q�I�u�W�F�N�g�폜
        foreach (Transform child in rList.imageScroll.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        rankCnt = 0; // ���ʃJ�E���g�̃��Z�b�g
        getCnt = 0;  // ���R�[�h�擾���̃��Z�b�g


        foreach (Record record in jsonRecords.records)
        {


            // �O��̃X�R�A�Ɠ����������ꍇ
            if (int.Parse(record.score) == befoScore)
            {
                // �d����̃����L���O�J�E���g�����Z
                befoCnt++;

                // ���ʂ̃J�E���g�����Z
                rankCnt++;

                // Json�I�u�W�F�N�g�ɕϊ�
                rList.SetList(rankCnt - befoCnt, record.name, record.score);
            }
            else
            {
                // Json�I�u�W�F�N�g�ɕϊ�
                rList.SetList(rankCnt + 1, record.name, record.score.ToString());

                // ���ʂ̃J�E���g�����Z
                rankCnt++;

                // �d���̃J�E���g�����Z�b�g
                befoCnt = 0;
            }

            // ���[�f�B���O��\��
            loadingObj.SetActive(false);

            // �ЂƂO�̃X�R�A���i�[
            befoScore = int.Parse(record.score);

            // ���R�[�h�擾�������Z
            getCnt++;

            // 20���擾������I��
            if (getCnt >= maxGet)
            {
                break;
            }
        }

        //rankCnt = 0;


    }

    // Json�t�@�C���Ƀ����L���O����ǉ����鏈�� TGS��
    private void AddScoreToJsonFile(JsonPostData newRecord)
    {
        var jsonRankData = PlayerPrefs.GetString("Rank");
        jsonRecords = JsonUtility.FromJson<Records>(jsonRankData);

        // �V�������R�[�h��ǉ�
        var newRecordsList = jsonRecords.records.ToList();
        Record newRecordEntry = new Record
        {
            name = newRecord.name,
            score = newRecord.score.ToString()
        };
        newRecordsList.Add(newRecordEntry);

        jsonRecords.records = newRecordsList.ToArray();     // 
        PlayerPrefs.SetString("Rank", JsonUtility.ToJson(jsonRecords));
        // ���[�f�B���O��\��
        loadingObj.SetActive(false);
    }

#endif

#if InternetCommunication
    // �����L���O�o�^����
    private IEnumerator PostScore()

#elif LocalCommunication
    private void PostScore()

#endif
    {
        // �o�^�f�[�^�쐬
        var post = new JsonPostData();

        // �v���C���[���̓o�^
        // �����͂̏ꍇ
        if (nameInputField.text =="")
        {
            post.name = "No Name";
        }
        // ���O�����͂���Ă���΁A���̖��O�œo�^
        else
        {
            post.name = nameInputField.text;
        }

        // �X�R�A�̓o�^
        post.score = gm.score;

        // �o�^����f�[�^��Json�`���ɕϊ�
        var jsonPost = JsonUtility.ToJson(post);

#if InternetCommunication
        // �ʐM���������s
        var coroutine = PostCommunication(URL_GAS, jsonPost);
        yield return StartCoroutine(coroutine);

        // �ʐM�I��
        // ���[�f�B���O���\��
        loadingObj.SetActive(false);

        // �o�^�����̃e�L�X�g��\��
        if (GameManager.isJapanese)
        {
            compleatedTextJa.SetActive(true);
        }
        else
        {
            compleatedTextEn.SetActive(true);
        }

#elif LocalCommunication
        // �o�^�����̃e�L�X�g��\��
        if (GameManager.isJapanese)
        {
            compleatedTextJa.SetActive(true);
        }
        else
        {
            compleatedTextEn.SetActive(true);
        }

        //TGS��
        AddScoreToJsonFile(post);
#endif

    }

    // �����L���O�o�^�{�^������������
    // Post�ʐM���J�n
    public void PushPostButton()
    {
        // ���[�f�B���O��\��
        loadingObj.SetActive(true);

#if InternetCommunication
        // Post�ʐM���������s
        var coroutine = PostScore();
        StartCoroutine(coroutine);

#elif LocalCommunication
        PostScore();

#endif

    }


    // �����L���O�{�^����������
    public void PushRankingButton()
    {
#if InternetCommunication
        // �܂������L���O���擾���Ă��Ȃ���Ύ擾
        if (!isGetRanking)
#elif LocalCommunication
#endif
        {
            // �����L���O�擾�̃t���O���I��
            isGetRanking = true;

#if InternetCommunication
            // ���[�f�B���O��\��
            loadingObj.SetActive(true);

            // Get�ʐM���������s
            StartCoroutine(GetData());

#elif LocalCommunication
            GetData();
#endif

        }
    }

    // �����L���O���X�Ɏ擾�������ꍇ�̏���
    public void PushMoreButton()
    {
        // �ő�擾����ύX
        maxGet += MORE_GET;

#if InternetCommunication
        // ���[�f�B���O��\��
        loadingObj.SetActive(true);
        
        // Get�ʐM���J�n
        StartCoroutine(GetData());

#elif LocalCommunication
        GetData();
#endif

        // �����L���O���ʂ̃J�E���g�Ȃǂ�������
        getCnt = 0;
        rankCnt = 0;
        // �ő�擾����ύX
        maxGet += MORE_GET;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ����������
        Init();
    }

}
