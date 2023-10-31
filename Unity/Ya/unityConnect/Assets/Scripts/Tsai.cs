using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Threading;


public class Tsai : MonoBehaviour
{
    void Start()
    {   


    }

    void Update()
    {
        Debug.Log("Wait user...");
        GameData.text = "panorama";
        Debug.Log("User confirms");
        BeginState.UserConfirm();
    }
}

