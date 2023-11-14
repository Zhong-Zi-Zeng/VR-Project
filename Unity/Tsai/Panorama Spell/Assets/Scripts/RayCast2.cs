using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast2 : MonoBehaviour
{
    // Start is called before the first frame update
    private LineRenderer rendo;
    void Start()
    {
        rendo = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.RawButton.RHandTrigger))
        {
            DoPewPew(transform.position, transform.forward, 5f);
            rendo.enabled = true;
        }
        else
        {
            rendo.enabled = false;
        }
    }
    void DoPewPew(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction);
        Vector3 endPos = targetPos + (direction * length);

        if(Physics.Raycast(ray,out RaycastHit rayhit, length))
        {
            endPos = rayhit.point;

        }
        rendo.SetPosition(0, targetPos);
        rendo.SetPosition(1, endPos);
    }
}
