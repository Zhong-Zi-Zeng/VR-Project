using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class WaitingPython : MonoBehaviour
{
    public Material targetMaterial;
    //public TMP_Text Text_1;
    // Start is called before the first frame update
    void Start()
    {
        string cubemapPath = "panoramalist/"+ Path.GetFileNameWithoutExtension(GameData.nowpanorama);
        ApplyCubemapAsSkybox(cubemapPath);
        //Text_1.text = GameData.nowpanorama;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.text == "over")
        {
            GameManager.ChangeState(StateId.InGameState);
            GameData.text = "";
        }
    }

    void ApplyCubemapAsSkybox(string cubemapPath)
    {
        Cubemap cubemapTexture = Resources.Load<Cubemap>(cubemapPath);
        if (cubemapTexture != null)
        {
            targetMaterial.SetTexture("_Tex", cubemapTexture);
            Debug.Log("skybox¤w§ó·s" + cubemapPath);
        }
        else
        {
            Debug.LogError("404");
        }
    }
}
