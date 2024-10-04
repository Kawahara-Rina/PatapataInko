/*
    JsonGetParam.cs 
    
    Get通信時に取得する値のクラス。
*/

using System;

[Serializable]
public class Records
{
    public Record[] records;
}

[Serializable]
public class Record
{
    public string name;
    public string score;

}