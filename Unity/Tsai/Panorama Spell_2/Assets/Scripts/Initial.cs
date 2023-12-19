using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// �D�n�@����ӹC�����_�I�A���F�i�JBeginstate�Ҩϥ�
/// </summary>
public class Initial : MonoBehaviour
{
    public static unityConnect trans_api;


    private bool flag = false;
    void Start()
    {
        trans_api = new unityConnect();
        GameManager.AddAllStates();
    }
    void Send()
    {
        trans_api.SendData("Search");
    }
    void Update()
    {

        if (flag == false)
        {
            flag = true;
            Invoke("Send", 10);
        }
        if (GameData.text == "over")
        {
            GameManager.ChangeState(StateId.BeginState);
            GameData.text = "";
        }
        Debug.Log(GameData.panoramaList.Count);
    }
}
