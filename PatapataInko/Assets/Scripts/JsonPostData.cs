/*
    JsonPostParam.cs 
    
    Post通信時にセットする値のクラス。
*/

using System;
using UnityEngine.SocialPlatforms.Impl;

[Serializable]
public class JsonPostData
{
    public string name;
    public int score;
}
