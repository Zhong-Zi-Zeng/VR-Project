using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �D�n�@����ӹC�����_�I�A���F�i�JBeginstate�Ҩϥ�
/// </summary>
public class Initial : MonoBehaviour
{
    //public static unityConnect trans_api = new();
    
   
    void Start()
    {
        Debug.Log("21312312313");

        unityConnect.build();
        unityConnect.SendData("Search");

        GameManager.AddAllStates();
        GameManager.ChangeState(StateId.BeginState);
    }
}
