using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PositionChange : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text colorInfoText; // 引用TMP Text
   
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {

            // 顯示 Ray 擊中點的座標
            colorInfoText.text = "Ray Hit Point Position: " + hit.collider.name;
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * 100f, Color.red);
    }
}
