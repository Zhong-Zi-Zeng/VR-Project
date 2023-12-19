using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameState : StateBase
{
    public override void OnStateEnter()
    {
        Debug.Log("Now in the InGameState:");

        // ¥ý¸ü¤JScene
        SceneManager.LoadScene(SceneName.InGameScene);

    }


    public override void OnStateExit()
    {

        
    }
}
