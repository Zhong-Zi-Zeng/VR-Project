using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EyeInteractable : MonoBehaviour
{
    public bool IsHovered { get; set; }
    [SerializeField]
    private UnityEvent<GameObject> OnObjectHover;
    [SerializeField]
    private Material OnHoverActiveMaterial;
    [SerializeField]
    private Material OnHoverInactiveMaterial;
    private MeshRenderer meshrenderer;
    // Start is called before the first frame update
    void Start() => meshrenderer = GetComponent<MeshRenderer>();
    
    // Update is called once per frame
    void Update()
    {
        if (IsHovered)
        {
            meshrenderer.material = OnHoverActiveMaterial;
            OnObjectHover?.Invoke(gameObject);
        }
        else
        {
            meshrenderer.material = OnHoverInactiveMaterial;
        }
    }
}
