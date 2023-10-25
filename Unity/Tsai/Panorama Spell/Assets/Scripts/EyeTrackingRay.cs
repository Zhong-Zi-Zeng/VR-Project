using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    public List<Texture2D> cubemaps = new List<Texture2D>();
    private int currentCubemapIndex = 0;
    float w = 4096;
    float h = 2048;
    private Transform newTransform;
    // Start is called before the first frame update
    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        SetupRay();

        GameObject newObject = new GameObject("NewObject");
        newTransform = newObject.transform;
        newTransform.position = Vector3.zero;
        //更換Skybox
        //for (int i = 1; i <= 6; i++)
        //{
        //    Texture2D texture = Resources.Load<Texture2D>("../Materials/Image/" + i.ToString() + ".PNG");
        //    if (texture != null)
        //    {
        //        cubemaps.Add(texture);
        //    }
        //    else
        //    {
        //        Debug.LogError("Failed to load" + i);
        //    }
        //}

        //if (cubemaps.Count > 0)
        //{
        //    Material skyboxMaterial = new Material(RenderSettings.skybox);
        //    skyboxMaterial.SetTexture("_Tex", cubemaps[currentCubemapIndex]);
        //    RenderSettings.skybox = skyboxMaterial;
        //}
        //else
        //{
        //    Debug.LogError("Cubemap列表為空。");
        //}
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
        //正常直射
        if (Physics.Raycast(transform.position, ratCastDirection,out hit, Mathf.Infinity, layersToInclude))
        {
            UnSelect();
            lineRender.startColor = raycolor2;
            lineRender.endColor = raycolor2;
            var eyeInteractable = hit.transform.GetComponent<EyeInteractable>();
            eyeInteractables.Add(eyeInteractable);
            eyeInteractable.IsHovered = true;

            // 新增的部分
            string objectName = hit.collider.gameObject.name;
            colorInfoText.text = "Object Name: " + objectName;
            //更換Skybox
            //Material skyboxMaterial = RenderSettings.skybox;
            //skyboxMaterial.SetTexture("_Tex", cubemaps[0]);
            //RenderSettings.skybox = skyboxMaterial;

        }
        else
        {
            lineRender.startColor = raycolor;
            lineRender.endColor = raycolor;
            UnSelect(true);
            colorInfoText.text = "";
        }
        //(0,0,0)直射
        if (Physics.Raycast(newTransform.position, ratCastDirection, out hit, Mathf.Infinity, layersToInclude))
        {
            Vector3 hitPoint = hit.point;

            float phi = Mathf.Atan2(hitPoint.y, Mathf.Sqrt(Mathf.Pow(hitPoint.x, 2) + Mathf.Pow(hitPoint.z, 2)));
            float theta = Mathf.Atan2(hitPoint.x, hitPoint.z);

            float x = ((theta + Mathf.PI) * w) / (2 * Mathf.PI);
            float y = h - ((2 * phi + Mathf.PI) * h) / (2 * Mathf.PI);

            colorInfoText2.text = $"Eye Direct Hit Position: ({hit.point}),U: {x}, V: {y}";

        }
        //(0,0,0)反射
        if (Physics.Raycast(newTransform.position + (transform.TransformDirection(Vector3.forward) * 1000000), -transform.TransformDirection(Vector3.forward), out hit, 1000000, layersToInclude))
        {
           
            Vector3 hitPoint = hit.point;

            float phi = Mathf.Atan2(hitPoint.y, Mathf.Sqrt(Mathf.Pow(hitPoint.x, 2) + Mathf.Pow(hitPoint.z, 2)));
            float theta = Mathf.Atan2(hitPoint.x, hitPoint.z);

            float x = ((theta + Mathf.PI) * w) / (2 * Mathf.PI);
            float y = h - ((2 * phi + Mathf.PI) * h) / (2 * Mathf.PI);

            colorInfoText3.text = $"Eye Reflection Hit Position: ({hit.point}),U: {x}, V: {y}";
        }
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
}
