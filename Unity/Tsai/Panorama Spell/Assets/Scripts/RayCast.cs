using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField]
    private LayerMask layersToInclude;
    public TMP_Text colorInfoText; // 引用TMP Text

    void Update()
    {
        // 在主體 Cube 的位置向前射出 Ray
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (transform.TransformDirection(Vector3.forward) * 1000000), -transform.TransformDirection(Vector3.forward), out hit, 1000000, layersToInclude))
        {
            Vector3 hitPoint = hit.point;

            float phi = Mathf.Atan2(hitPoint.y, Mathf.Sqrt(Mathf.Pow(hitPoint.x, 2) + Mathf.Pow(hitPoint.z, 2)));
            float theta = Mathf.Atan2(hitPoint.x, hitPoint.z);

            float x = ((theta + Mathf.PI) * 2048) / (2 * Mathf.PI);
            float y = 1054 - ((2 * phi + Mathf.PI) * 1024) / (2 * Mathf.PI);

            colorInfoText.text = $"Hit Position: ({hit.point}),U: {x}, V: {y}";
        }
         //:)
   
       
    }
}
