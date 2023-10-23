using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreate : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    public void CloneMe()
    {
        var newObject = Instantiate(gameObject);
        newObject.transform.parent = gameObject.transform.parent;
        newObject.transform.position = gameObject.transform.position + offset;
        gameObject.SetActive(false);
    }
}
