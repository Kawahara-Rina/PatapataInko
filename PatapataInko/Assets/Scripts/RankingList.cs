/*
    RankingList.cs 
    
    ランキング情報をセットし、表示を行うクラス。。
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingList : MonoBehaviour
{
    // ランキングのテンプレート
    [SerializeField] private GameObject prefabRankingTemplate;

    // プレファブを表示するスクロール領域
    public GameObject imageScroll;

    // ランキングを取得後、ランキングテンプレ―トプレファブを生成する処理
    public void SetList(int rank,string nameText,string scoreText)
    {
        // プレファブを生成
        var temp = Instantiate(prefabRankingTemplate, imageScroll.transform);

        // 順位、名前、スコアを各テキストに反映
        temp.transform.Find("rankText").GetComponent<Text>().text = rank.ToString();
        temp.transform.Find("nameText").GetComponent<Text>().text = nameText;
        temp.transform.Find("scoreText").GetComponent<Text>().text = scoreText;
    }

}
