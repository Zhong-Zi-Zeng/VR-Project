using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Cast ray from index finger to interact with Alphabat Generators
/// </summary>
public class RayCaster : MonoBehaviour
{
    [Tooltip("The layermask to check for collisions.")]
    [SerializeField]
    LayerMask layerAlphabetGenerator;
    [Tooltip("The max distance the ray should check for collisions.")]
    [SerializeField]
    float maxDistance = 50f;
    [Tooltip("Whether use the line renderer to show the ray.")]
    [SerializeField]
    bool useVisibleRay = true;
    [Tooltip("The time in seconds that the ray must be hitting the AlphabetGenerator before interaction begins.")]
    [SerializeField]
    float interactionDelay = 1f;
    #region LineRenderer
    LineRenderer lineRenderer;
    Color hitStart = new Color(0.3254717f, 1f, 0.7896893f);
    Color end = new Color(0.2311321f, 0.6582434f, 1f);
    Color nohitStart = new Color(1f, 0.4323243f, 0.3254902f);
    #endregion
    bool isHitting = false;
    float hitTime = 0f;
    public TMP_Text text1; // ¤Þ¥ÎTMP Text

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        int layerIndex = LayerMask.NameToLayer("Alphabet Generator");
        if (-1 == layerIndex)
        {
            Debug.LogError("LayerMask \"Alphabet Generator\" is missing.");
            gameObject.SetActive(false);
        }
        else
        {
            layerAlphabetGenerator = 1 << layerIndex;
        }

        gameObject.SetActive(false);
    }

    
    void Update()
    {
      
        float w = 4096;
        float h = 2048;
        float x = 0f;
        float y = 0f;
        if (useVisibleRay)
        {
            lineRenderer.SetPosition(0, transform.position);
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, maxDistance, layerAlphabetGenerator))
        {
            Vector3 hitPoint = hitInfo.point;
            float phi = Mathf.Atan2(hitPoint.y, Mathf.Sqrt(Mathf.Pow(hitPoint.x, 2) + Mathf.Pow(hitPoint.z, 2)));
            float theta = Mathf.Atan2(hitPoint.x, hitPoint.z);

            x = ((theta + Mathf.PI) * w) / (2 * Mathf.PI);
            y = h - ((2 * phi + Mathf.PI) * h) / (2 * Mathf.PI);
            text1.text = $"Hand Reflection Hit Position: U: {x}, V: {y}";

            if (useVisibleRay)
            {
                lineRenderer.enabled = true;
                lineRenderer.startColor = hitStart;
                lineRenderer.SetPosition(1, hitInfo.point);
            }
        }
        else
        {
            if (useVisibleRay)
            {
                lineRenderer.enabled = true;
                lineRenderer.startColor = nohitStart;
                Vector3 rayEndPoint = transform.position + transform.TransformDirection(Vector3.forward) * maxDistance;
                lineRenderer.SetPosition(1, rayEndPoint);
            }
        }
    }

    
}
