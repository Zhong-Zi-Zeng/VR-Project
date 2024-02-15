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

        // �����JScene
        SceneManager.LoadScene(SceneName.LearningScene);

    }


    public override void OnStateExit()
    {

        
    }
}
