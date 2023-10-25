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
    public TMP_Text colorInfoText2; // 引用TMP Text
    private float rayDistanse = 1.0f;
    private Transform newTransform;
    void Start()
    {
        GameObject newObject = new GameObject("NewObject");
        newTransform = newObject.transform; 
        newTransform.position = Vector3.zero;
    }
    void Update()
    {
        float w = 4096;
        float h = 2048;
        // 在主體 Cube 的位置向前射出 Ray
        RaycastHit hit;
        //(0,0,0)反射
        if (Physics.Raycast(newTransform.position + (transform.TransformDirection(Vector3.forward) * 1000000), -transform.TransformDirection(Vector3.forward), out hit, 1000000, layersToInclude))
        {
            Vector3 hitPoint = hit.point;

            float phi = Mathf.Atan2(hitPoint.y, Mathf.Sqrt(Mathf.Pow(hitPoint.x, 2) + Mathf.Pow(hitPoint.z, 2)));
            float theta = Mathf.Atan2(hitPoint.x, hitPoint.z);

            float x = ((theta + Mathf.PI) * w) / (2 * Mathf.PI);
            float y = h - ((2 * phi + Mathf.PI) * h) / (2 * Mathf.PI);

            colorInfoText.text = $"Hand Reflection Hit Position: ({hit.point}),U: {x}, V: {y}";
        }
        //(0,0,0)直射
        Vector3 ratCastDirection = transform.TransformDirection(Vector3.forward) * rayDistanse;
        if (Physics.Raycast(newTransform.position, ratCastDirection, out hit, Mathf.Infinity, layersToInclude))
        {
            Vector3 hitPoint = hit.point;

            float phi = Mathf.Atan2(hitPoint.y, Mathf.Sqrt(Mathf.Pow(hitPoint.x, 2) + Mathf.Pow(hitPoint.z, 2)));
            float theta = Mathf.Atan2(hitPoint.x, hitPoint.z);

            float x = ((theta + Mathf.PI) * w) / (2 * Mathf.PI);
            float y = h - ((2 * phi + Mathf.PI) * h) / (2 * Mathf.PI);

            colorInfoText2.text = $"Hand Direct Hit Position: ({hit.point}),U: {x}, V: {y}";
        }

    }
}
