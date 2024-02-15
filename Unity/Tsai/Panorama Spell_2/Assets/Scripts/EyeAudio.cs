using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Meta.WitAi.TTS.Utilities;

public class EyeAudio : MonoBehaviour
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
    public TMP_Text BingoText;
    public TMP_Text Text1; // 引用TMP Text
    public TMP_Text Text2; // 引用TMP Text
    public TMP_Text Text3; // 引用TMP Text
    public TMP_Text Text4; // 引用TMP Text

    private Transform newTransform;
    public Material targetMaterial;
    private Dictionary<int, string> labelDictionary = new Dictionary<int, string>();
    public AudioSource audioSource; // 指定音頻源
   
    private string flag2;
    private string eyelable;
    private string audiolable;
    private bool flag = false;
    Texture2D tex_1;
    Texture2D tex_2;
    private int currentSoundIndex = -1;
    private Color id_pixelColor;
    private List<int> uniqueValuesList = new List<int>();
    private int randomIndex;
    private Vector3 hitPoint;
    
    //拼單字
    TTSSpeaker ttsspeaker;
    bool showVoiceLog = false;
    private float gazeTimer = 0f;
    private string currentGazeObject = "";
    public GameObject Effect;
    public AudioClip applauseClip;
    // Start is called before the first frame update
    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        SetupRay();

        GameObject newObject = new GameObject("NewObject");
        newTransform = newObject.transform;
        newTransform.position = Vector3.zero;
        ApplyCubemapAsSkybox("panoramawithmask/0");//原圖

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
        StartCoroutine(PlayRandomSoundRepeatedly());
        uniqueValuesList = new List<int> { 0, 39, 40, 56, 58, 68, 69, 71, 72, 75 };


    }
    IEnumerator PlayRandomSoundRepeatedly()
    {

        while (true) // 無限循環
        {
            if (uniqueValuesList.Count > 0)
            {
                int randomListIndex = Random.Range(0, uniqueValuesList.Count);
                randomIndex = uniqueValuesList[randomListIndex];

            }
            else
            {
                randomIndex = 0;

            }
            PlayRandomSound("sounds/" + randomIndex); // 播放音效
            audioSource.loop = true;
            BingoText.gameObject.SetActive(false);
            Text2.gameObject.SetActive(true);
            Text3.gameObject.SetActive(true);
            Text4.gameObject.SetActive(true);

            bool found = false; // 用於標記是否找到對應物件
            float startTime = Time.time; // 紀錄開始時間

            for (int i = 10; i > 0 && !found; i--) // 檢查是否提前找到物件
            {
                if (eyelable == audiolable)
                {
                    BingoText.text = "Bingo"; // 更新BingoText
                    BingoText.gameObject.SetActive(true);
                    Text2.gameObject.SetActive(false);
                    Text3.gameObject.SetActive(false);
                    Text4.gameObject.SetActive(false);
                    audioSource.clip = applauseClip;
                    audioSource.Play();

                    Instantiate(Effect, hitPoint, Quaternion.identity);
                    found = true; // 標記找到對應物件
                    yield return new WaitForSeconds(3); // 慶祝3秒
                    eyelable = "";
                }
                else
                {
                    Text4.text = "Remaining: " + i.ToString();
                }
                yield return new WaitForSeconds(1);
            }

            // 計算已過時間，確保總等待時間為13秒（包括尋找、慶祝或失败階段）
            float elapsed = Time.time - startTime;
            float waitTime = Mathf.Max(0, 15 - elapsed); // 總等待時間減去已經過的時間

            if (!found)
            {
                BingoText.text = "Failed"; // 如果未找到，更新BingoText為Failed
                BingoText.gameObject.SetActive(true);
                Text2.gameObject.SetActive(false);
                Text3.gameObject.SetActive(false);
                Text4.gameObject.SetActive(false);

                eyelable = "";
            }
            yield return new WaitForSeconds(waitTime); // 等待剩餘時間，确保下一轮有完整的十秒
            ResetState();
        }
    }
    void ResetState()
    {
        audioSource.loop = false;
        audioSource.Stop();

    }
    void PlayRandomSound(string audioPath)
    {
        //Text.text = randomIndex.ToString();
        Text3.text = "PlayAudio:"+ (labelDictionary.ContainsKey(randomIndex) ? labelDictionary[randomIndex] : "Unknown");
        audiolable = labelDictionary.ContainsKey(randomIndex) ? labelDictionary[randomIndex] : "Unknown";
        audioSource.clip = Resources.Load<AudioClip>(audioPath);
        audioSource.Play();
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
        Vector3 rayCastDirection = transform.TransformDirection(Vector3.forward) * rayDistanse;
        
        float x = 0f;
        float y = 0f;
        //正常直射
        if (Physics.Raycast(transform.position, rayCastDirection, out hit, Mathf.Infinity, layersToInclude))
        {
            lineRender.startColor = raycolor2;
            lineRender.endColor = raycolor2;

        }
        else
        {
            lineRender.startColor = raycolor;
            lineRender.endColor = raycolor;

        }

        if (Physics.Raycast(newTransform.position + rayCastDirection, -transform.forward, out hit, rayDistanse))
        {

            hitPoint = hit.point;
            float w = 4096;
            float h = 2048;
            float phi = Mathf.Atan2(hitPoint.y, Mathf.Sqrt(Mathf.Pow(hitPoint.x, 2) + Mathf.Pow(hitPoint.z, 2)));
            float theta = Mathf.Atan2(hitPoint.x, hitPoint.z);

            x = ((theta + Mathf.PI) * w) / (2 * Mathf.PI);
            y = h - ((2 * phi + Mathf.PI) * h) / (2 * Mathf.PI);

            //colorInfoText.text = $"Eye Reflection Hit Position: U: {x}, V: {y}";
        }
        //idmap
        tex_1 = Resources.Load<Texture2D>("id_map");

        id_pixelColor = tex_1.GetPixel(Mathf.FloorToInt(x), 2048 - Mathf.FloorToInt(y));

        tex_2 = Resources.Load<Texture2D>("index_map");
        Color index_pixelColor = tex_2.GetPixel(Mathf.FloorToInt(x), 2048 - Mathf.FloorToInt(y));
   

        if (Mathf.FloorToInt(id_pixelColor.r * 255) - 1 > 0)
        {
            string label = labelDictionary[Mathf.FloorToInt(id_pixelColor.r * 255) - 1];
            Text2.text = "Watching:"+label;
            if (label == currentGazeObject)
            {
                gazeTimer += Time.fixedDeltaTime;
                if (gazeTimer >= 2f) 
                {
                    eyelable = label; 
                    gazeTimer = 0; 
                }
            }
            else
            {
                currentGazeObject = label;
                gazeTimer = 0; 
            }
            //if (label != flag2)
            //{
            //    flag2 = label;
                if (Mathf.FloorToInt(index_pixelColor.r * 255) - 1 > 0)
                {
                    string cubemapPath = "panoramawithmask/" + (Mathf.FloorToInt(index_pixelColor.r * 255) - 1).ToString();
                    ApplyCubemapAsSkybox(cubemapPath);
 
                }
            //}
        }
        else
        {
            Text2.text = "Watching: none";
            ApplyCubemapAsSkybox("panoramawithmask/0");
        }

        
    }

    void ApplyCubemapAsSkybox(string cubemapPath)
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

    
    //void SaySpelling()
    //{
    //    try
    //    {
    //        if (ttsspeaker.IsLoading || ttsspeaker.IsSpeaking)
    //        {
    //            ttsspeaker.Stop();
    //        }
    //        ttsspeaker.Speak(audiolable);
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError(e);
    //        //UpdateVoiceLog(e.ToString());
    //    }
    //}
    //public void UpdateVoiceLog(string newLog)
    //{
    //    if (showVoiceLog)
    //    {
    //        Text1.text = newLog;
    //    }
    //}

    //string ListToString(List<int> list)
    //{
    //    return string.Join(", ", list);
    //}
}