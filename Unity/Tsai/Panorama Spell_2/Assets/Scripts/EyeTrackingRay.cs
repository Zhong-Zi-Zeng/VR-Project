using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class EyeTrackingRay : MonoBehaviour
{
    [SerializeField]
    private float rayDistanse = 1.0f;
    [SerializeField]
    private float raywidth = 0.01f;
    [SerializeField]
    private LayerMask layersToInclude;
    [SerializeField]
    private Color raycolor = Color.yellow;
    [SerializeField]
    private Color raycolor2 = Color.red;
    private LineRenderer lineRender;
    private List<EyeInteractable> eyeInteractables = new List<EyeInteractable>();
    public TMP_Text colorInfoText; // 引用TMP Text
    public TMP_Text colorInfoText2; // 引用TMP Text
    public TMP_Text colorInfoText3; // 引用TMP Text
    public TMP_Text colorInfoText4; // 引用TMP Text
    public List<Texture2D> cubemaps = new List<Texture2D>();
    
    private Transform newTransform;
    public Material targetMaterial;
    Texture2D tex_1;
    Texture2D tex_2;
    Texture2D tex_3;
    private Dictionary<int, string> labelDictionary = new Dictionary<int, string>();

    private unityConnect trans_api;
    public RawImage img_1;
    public RawImage img_2;
    public RawImage img_3;
    private bool flag = false;
    private string flag2 ;
    // Start is called before the first frame update
    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        SetupRay();
        //trans_api = new unityConnect();
        tex_1 = new Texture2D(4096, 2048);
        tex_2 = new Texture2D(4096, 2048);
        tex_3 = new Texture2D(4096, 2048);
        GameObject newObject = new GameObject("NewObject");
        newTransform = newObject.transform;
        newTransform.position = Vector3.zero;
        ApplyCubemapAsSkybox_old("panoramalist/" + Path.GetFileNameWithoutExtension(GameData.nowpanorama));//原圖

        labelDictionary.Add(0, "person");
        labelDictionary.Add(1, "bicycle");
        labelDictionary.Add(2, "car");
        labelDictionary.Add(3, "motorcycle");
        labelDictionary.Add(4, "airplane");
        labelDictionary.Add(5, "bus");
        labelDictionary.Add(6, "train");
        labelDictionary.Add(7, "truck");
        labelDictionary.Add(8, "boat");
        labelDictionary.Add(9, "traffic light");
        labelDictionary.Add(10, "fire hydrant");
        labelDictionary.Add(11, "stop sign");
        labelDictionary.Add(12, "parking meter");
        labelDictionary.Add(13, "bench");
        labelDictionary.Add(14, "bird");
        labelDictionary.Add(15, "cat");
        labelDictionary.Add(16, "dog");
        labelDictionary.Add(17, "horse");
        labelDictionary.Add(18, "sheep");
        labelDictionary.Add(19, "cow");
        labelDictionary.Add(20, "elephant");
        labelDictionary.Add(21, "bear");
        labelDictionary.Add(22, "zebra");
        labelDictionary.Add(23, "giraffe");
        labelDictionary.Add(24, "backpack");
        labelDictionary.Add(25, "umbrella");
        labelDictionary.Add(26, "handbag");
        labelDictionary.Add(27, "tie");
        labelDictionary.Add(28, "suitcase");
        labelDictionary.Add(29, "frisbee");
        labelDictionary.Add(30, "skis");
        labelDictionary.Add(31, "snowboard");
        labelDictionary.Add(32, "sports ball");
        labelDictionary.Add(33, "kite");
        labelDictionary.Add(34, "baseball bat");
        labelDictionary.Add(35, "baseball glove");
        labelDictionary.Add(36, "skateboard");
        labelDictionary.Add(37, "surfboard");
        labelDictionary.Add(38, "tennis racket");
        labelDictionary.Add(39, "bottle");
        labelDictionary.Add(40, "wine glass");
        labelDictionary.Add(41, "cup");
        labelDictionary.Add(42, "fork");
        labelDictionary.Add(43, "knife");
        labelDictionary.Add(44, "spoon");
        labelDictionary.Add(45, "bowl");
        labelDictionary.Add(46, "banana");
        labelDictionary.Add(47, "apple");
        labelDictionary.Add(48, "sandwich");
        labelDictionary.Add(49, "orange");
        labelDictionary.Add(50, "broccoli");
        labelDictionary.Add(51, "carrot");
        labelDictionary.Add(52, "hot dog");
        labelDictionary.Add(53, "pizza");
        labelDictionary.Add(54, "donut");
        labelDictionary.Add(55, "cake");
        labelDictionary.Add(56, "chair");
        labelDictionary.Add(57, "couch");
        labelDictionary.Add(58, "potted plant");
        labelDictionary.Add(59, "bed");
        labelDictionary.Add(60, "dining table");
        labelDictionary.Add(61, "toilet");
        labelDictionary.Add(62, "tv");
        labelDictionary.Add(63, "laptop");
        labelDictionary.Add(64, "mouse");
        labelDictionary.Add(65, "remote");
        labelDictionary.Add(66, "keyboard");
        labelDictionary.Add(67, "cell phone");
        labelDictionary.Add(68, "microwave");
        labelDictionary.Add(69, "oven");
        labelDictionary.Add(70, "toaster");
        labelDictionary.Add(71, "sink");
        labelDictionary.Add(72, "refrigerator");
        labelDictionary.Add(73, "book");
        labelDictionary.Add(74, "clock");
        labelDictionary.Add(75, "vase");
        labelDictionary.Add(76, "scissors");
        labelDictionary.Add(77, "teddy bear");
        labelDictionary.Add(78, "hair dryer");
        labelDictionary.Add(79, "toothbrush");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void SetupRay()
    {
        lineRender.useWorldSpace = false;
        lineRender.positionCount = 2;
        lineRender.startWidth = raywidth;
        lineRender.endWidth = raywidth;
        lineRender.startColor = raycolor;
        lineRender.endColor = raycolor;
        lineRender.SetPosition(0, transform.position);
        lineRender.SetPosition(1, new Vector3(transform.position.x, transform.position.y, transform.position.z + rayDistanse));
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 ratCastDirection = transform.TransformDirection(Vector3.forward) * rayDistanse;
        float x = 0f;
        float y = 0f;
        //正常直射
        if (Physics.Raycast(transform.position, ratCastDirection, out hit, Mathf.Infinity, layersToInclude))
        {
            UnSelect();
            lineRender.startColor = raycolor2;
            lineRender.endColor = raycolor2;
            var eyeInteractable = hit.transform.GetComponent<EyeInteractable>();
            eyeInteractables.Add(eyeInteractable);
            eyeInteractable.IsHovered = true;



        }
        else
        {
            lineRender.startColor = raycolor;
            lineRender.endColor = raycolor;
            UnSelect(true);

        }

        if (Physics.Raycast(newTransform.position + ratCastDirection, -transform.forward, out hit, rayDistanse))
        {

            Vector3 hitPoint = hit.point;
            float w = 4096;
            float h = 2048;
            float phi = Mathf.Atan2(hitPoint.y, Mathf.Sqrt(Mathf.Pow(hitPoint.x, 2) + Mathf.Pow(hitPoint.z, 2)));
            float theta = Mathf.Atan2(hitPoint.x, hitPoint.z);

            x = ((theta + Mathf.PI) * w) / (2 * Mathf.PI);
            y = h - ((2 * phi + Mathf.PI) * h) / (2 * Mathf.PI);

            colorInfoText.text = $"Eye Reflection Hit Position: U: {x}, V: {y}";


        }
        //idmap
        //Color id_pixelColor = Color.black; ;
        //Color index_pixelColor = Color.black; ;
        if (GameData.idMap != null && flag == false)
        {
            flag = true;
            tex_1.LoadImage(GameData.idMap);
            img_1.texture = tex_1;

            tex_2.LoadImage(GameData.indexMap);
            img_2.texture = tex_2;
        }
        Color id_pixelColor = tex_1.GetPixel(Mathf.FloorToInt(x), 2048 - Mathf.FloorToInt(y));
        Color index_pixelColor = tex_2.GetPixel(Mathf.FloorToInt(x), 2048 - Mathf.FloorToInt(y));
        colorInfoText2.text = "GameData.panoramaList.Count:"+GameData.panoramaWithMaskList.Count.ToString();
        //這樣寫會害這個方法只run一次，eyetracking會失效 
        if (Mathf.FloorToInt(id_pixelColor.r * 255) - 1 > 0)
        {
            string label = labelDictionary[Mathf.FloorToInt(id_pixelColor.r * 255) - 1];
            colorInfoText3.text = "label:"+label;
            if (label != flag2)
            {
                flag2 = label;
                if (Mathf.FloorToInt(index_pixelColor.r * 255) - 1 > 0)
                {
                    byte[] cubemapBytes = GameData.panoramaWithMaskList[Mathf.FloorToInt(index_pixelColor.r * 255) - 1];
                    tex_3.LoadImage(cubemapBytes);
                    img_3.texture = tex_3;
                    colorInfoText4.text = "index_pixelColor" + (Mathf.FloorToInt(index_pixelColor.r * 255) - 1).ToString();
                }
            }
        }

        //if (Mathf.FloorToInt(index_pixelColor.r * 255) - 1 > 0)
        //{
        //    string cubemapPath = "panoramawithmask/" + (Mathf.FloorToInt(index_pixelColor.r * 255) - 1).ToString();
        //    ApplyCubemapAsSkybox_old(cubemapPath);
        //    colorInfoText4.text = "index_pixelColor" + (Mathf.FloorToInt(index_pixelColor.r * 255) - 1).ToString();
        //}

        //int index = Mathf.FloorToInt(index_pixelColor.r * 255) - 1;
        //if (Mathf.FloorToInt(index_pixelColor.r * 255) - 1 > 0 )
        //{
        //    //byte[] cubemapBytes = GameData.panoramaWithMaskList[(Mathf.FloorToInt(index_pixelColor.r * 255) - 2)];
        //    //Cubemap cubemap = CreateCubemapFromBytes(cubemapBytes);
        //    //ApplyCubemapAsSkybox(cubemap);
        //    tex_3.LoadImage(GameData.panoramaWithMaskList[(Mathf.FloorToInt(index_pixelColor.r * 255) - 2)]);
        //    img_3.texture = tex_3;
        //    //Texture2D cubemap = CreateCubemapFromBytes_2(cubemapBytes);
        //    //ApplyCubemapAsSkybox_2(cubemap);
        //}
        //else
        //{
        //    colorInfoText3.text = "gg";
        //}
    }

    void UnSelect(bool clear = false)
    {
        foreach(var interactable in eyeInteractables)
        {
            interactable.IsHovered = false;
        }
        if (clear)
        {
            eyeInteractables.Clear(); 
        }
    }
    void ApplyCubemapAsSkybox_old(string cubemapPath)
    {
        Cubemap cubemapTexture = Resources.Load<Cubemap>(cubemapPath);
        if (cubemapTexture != null)
        {
            targetMaterial.SetTexture("_Tex", cubemapTexture);
            Debug.Log("skybox已更新" + cubemapPath);
        }
        else
        {
            Debug.LogError("404");
        }
    }

    void ApplyCubemapAsSkybox(Cubemap cubemap)
    {
        if (cubemap != null)
        {
            targetMaterial.SetTexture("_Tex", cubemap);
            colorInfoText3.text = "skybox已更新"; 
        }
        else
        {
            colorInfoText3.text = "404";
        }
    }

    Cubemap CreateCubemapFromBytes(byte[] cubemapBytes)
    {

        int size = 2048; 
        Cubemap cubemap = new Cubemap(size, TextureFormat.RGB24, false);

        Texture2D tex = new Texture2D(4096, 2048, TextureFormat.RGB24, false);
        tex.LoadImage(cubemapBytes); 

        for (int face = 0; face < 6; face++)
        {
            Color[] facePixels = tex.GetPixels(face * size, 0, size, size);
            CubemapFace cubemapFace = (CubemapFace)face;
            cubemap.SetPixels(facePixels, cubemapFace);
        }
        cubemap.Apply();
        return cubemap;
    }

    Texture2D CreateCubemapFromBytes_2(byte[] cubemapBytes)
    {

        Texture2D tex = new Texture2D(4096, 2048, TextureFormat.RGB24, false);
        tex.LoadImage(cubemapBytes);


        tex.Apply();
        return tex;
    }

    void ApplyCubemapAsSkybox_2(Texture2D cubemap)
    {
        if (cubemap != null)
        {
            img_2.texture = cubemap;
            colorInfoText3.text = "skybox已更新";
        }
        else
        {
            colorInfoText3.text = "404";
        }
    }
}
