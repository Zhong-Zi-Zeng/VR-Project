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

        // �����JScene
        GameManager.ChangeScene(SceneName.WaitingPythonScene);

        // �N�ϥΪ̰T���ǰe��Python�ݡA�õ��ݦ^��


        // �Y������T���������InGameState

    }


    public override void OnStateExit()
    {

        
    }
}
