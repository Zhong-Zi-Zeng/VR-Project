using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 主要作為整個遊戲的起點，為了進入Beginstate所使用
/// </summary>
public class Initial : MonoBehaviour
{
    public static unityConnect trans_api = new();

    void Start()
    {
        trans_api.SendData("Search");
        GameManager.AddAllStates();
        GameManager.ChangeState(StateId.BeginState);
    }
}
