/*
    JsonPlayerData.cs 
    
    プレイヤーのデータを端末ごとに保存するためのクラス。
*/

using UnityEngine;

[System.Serializable]
public class JsonPlayerData
{
    public int highScore;    // プレイヤーのハイスコア
    public int[] score=new int[10];
    public string[] name=new string[10];
    public int num;
    
    // 設定関連
    public bool isJapanese;  // 言語が日本語かどうか
    public float speValue;   // スピードの値
    public float volValue;   // ボリュームの値

}
