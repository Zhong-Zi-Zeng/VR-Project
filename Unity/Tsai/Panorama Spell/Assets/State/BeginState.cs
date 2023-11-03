using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class BeginState : StateBase
{   
    public override void OnStateEnter()
    {
        Debug.Log("Now in the BeginState:");

        // 先載入Scene
        GameManager.ChangeScene(SceneName.BeginScene);


    }

    public override void OnStateExit()
    {   

    }

    /// <summary>
    /// 當使用者確認完照片後，會呼叫這個函式
    /// 主要是將使用者所選擇的照片名稱和基礎設置傳送給Python端後等待Python端回傳訊息
    /// </summary>
    public static void UserConfirm()
    {
        // 確認資訊有沒有問題


        // 切換到WaitPythonState
        GameManager.ChangeState(StateId.WaitingPythonState);
    }
}

