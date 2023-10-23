using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    // Update is called once per frame
    float w = 4096;
    float h = 20486;
    public TMP_Text colorInfoText; // 引用TMP Text
    public TMP_Text colorInfoText2; // 引用TMP Text
    public TMP_Text colorInfoText3; // 引用TMP Text
    void Update()
    {
        // 在主體 Cube 的位置向前射出 Ray
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.forward * 1000f, -transform.forward, out hit, 1000f))
        {
            Vector3 hitPoint = hit.point;
            
            float  phi= Mathf.Atan2(hitPoint.y , Mathf.Sqrt(Mathf.Pow(hitPoint.x,2)+ Mathf.Pow(hitPoint.z, 2)));
            float theta = Mathf.Atan2(hitPoint.x, hitPoint.z);

            float x = ((theta+Mathf.PI)*2048)/(2* Mathf.PI);
            float y = ((2*phi + Mathf.PI) * 1024)/(2 * Mathf.PI); 

            colorInfoText.text = $"Hit Position: ({hit.point}),U: {x}, V: {y}";
        }
        if (Physics.Raycast(transform.position , transform.forward * 1000f, out hit, 1000f))
        {
            colorInfoText2.text= $"Hit Collider Name: {hit.collider.name}";
            colorInfoText3.text = $"transform.position: {transform.position}";
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * 8000f, Color.red);
       
    }
}
