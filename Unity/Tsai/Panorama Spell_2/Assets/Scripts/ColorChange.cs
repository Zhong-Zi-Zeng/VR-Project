using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ColorChange : MonoBehaviour
{
    public Image image;
    public TMP_Text colorInfoText; // ¤Þ¥ÎTMP Text
    public void ChangeBackgroundColor()
    {
        Color randomColor = Random.ColorHSV();
        image.color = randomColor;
        colorInfoText.text = $"RGB: ({randomColor.r * 255}, {randomColor.g * 255}, {randomColor.b * 255})";
    }
    void Update()
    {
        
    }

}
