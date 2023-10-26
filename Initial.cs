using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Initial : MonoBehaviour
{   
    private Manager manager;
    

    void Start()
    {
        this.manager = Manager.Instatance();
        Manager.ChangeState(StateId.Beginstate);
    }

}
