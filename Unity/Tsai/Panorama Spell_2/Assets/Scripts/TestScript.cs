using UnityEngine;
using UnityEngine.UI;


public class TestScript : MonoBehaviour
{
    public RawImage rawImage;
    public RawImage rawImage_1;
    public RawImage rawImage_2;
    public RawImage rawImage_3;
    public RawImage rawImage_4;
    public RawImage rawImage_5;
    Texture2D tex_1;
    Texture2D tex_2;
    Texture2D tex_3;
    Texture2D tex_4;
    Texture2D tex_5;
    Texture2D tex_6;
    private bool flag = false;
    void Start()
    {

        tex_1 = new Texture2D(4096, 2048);
        tex_2 = new Texture2D(4096, 2048);
        tex_3 = new Texture2D(4096, 2048);
        tex_4 = new Texture2D(4096, 2048);
        tex_5 = new Texture2D(4096, 2048);
        tex_6 = new Texture2D(4096, 2048);
    }

    void Update()
    {
        

        if (GameData.panoramaList.Count > 0 && flag == false)
        {
            flag = true;
            tex_1.LoadImage(GameData.panoramaList[0]);
            rawImage.texture = tex_1;
            tex_2.LoadImage(GameData.panoramaList[1]);
            rawImage_1.texture = tex_2;
            tex_3.LoadImage(GameData.panoramaList[2]);
            rawImage_2.texture = tex_3;
            tex_4.LoadImage(GameData.panoramaList[3]);
            rawImage_3.texture = tex_4;
            tex_5.LoadImage(GameData.panoramaList[4]);
            rawImage_4.texture = tex_5;
            tex_6.LoadImage(GameData.panoramaList[5]);
            rawImage_5.texture = tex_6;

        }
       
    }
}
