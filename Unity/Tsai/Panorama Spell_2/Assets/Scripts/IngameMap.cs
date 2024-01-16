using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameMap : MonoBehaviour
{
    private unityConnect trans_api;

    public RawImage img_1;
    public RawImage img_2;


    Texture2D tex_1;
    Texture2D tex_2;

    private bool flag = false;
    // Start is called before the first frame update
    void Start()
    {
        // 傳輸使用的接口
        trans_api = new unityConnect();
        tex_1 = new Texture2D(4096, 2048);
        tex_2 = new Texture2D(4096, 2048);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.idMap != null)
        {
            tex_1.LoadImage(GameData.idMap);
            img_1.texture = tex_1;

            tex_2.LoadImage(GameData.indexMap);
            img_2.texture = tex_2;
        }
        Debug.Log(GameData.panoramaWithMaskList.Count);
    }
}
