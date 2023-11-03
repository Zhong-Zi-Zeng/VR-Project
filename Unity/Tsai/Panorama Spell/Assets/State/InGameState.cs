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

        // �����JScene
        SceneManager.LoadScene(StateId.InGameState);

    }


    public override void OnStateExit()
    {

        
    }
}
