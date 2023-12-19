using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingPythonState : StateBase
{
    public override void OnStateEnter()
    {
        Debug.Log("Now in the WaitPythonState:");

        // 先載入Scene
        GameManager.ChangeScene(SceneName.WaitingPythonScene);

        // 將使用者訊息傳送給Python端，並等待回覆


        // 若接收到訊息後切換到InGameState

    }


    public override void OnStateExit()
    {

        
    }
}
