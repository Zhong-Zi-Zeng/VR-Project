using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �D�n�@����ӹC�����_�I�A���F�i�JBeginstate�Ҩϥ�
/// </summary>
public class Initial : MonoBehaviour
{   
    void Start()
    {
        GameManager.AddAllStates();
        GameManager.ChangeState(StateId.BeginState);
    }
}
