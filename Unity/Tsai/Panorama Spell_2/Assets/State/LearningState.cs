using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LearningState : StateBase
{
    public override void OnStateEnter()
    {
        Debug.Log("Now in the LearningState:");

        // ¥ý¸ü¤JScene
        SceneManager.LoadScene(StateId.LearningState);

    }


    public override void OnStateExit()
    {

        
    }
}
