using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanoramahManager : MonoBehaviour
{
    // Start is called before the first frame update
    //public OVRPassthroughLayer passthrough;
    //public OVRInput.Button button;
    //public OVRInput.Controller controller;
    //public List<Gradient> colorMapGradient;
    //public GameObject passthroughStyleCanvas;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (OVRInput.GetDown(button, controller))
        //{
        //    passthrough.hidden = !passthrough.hidden;
        //}
        //if (OVRInput.GetDown(OVRInput.Button.Four))
        //{
        //    passthroughStyleCanvas.SetActive(!passthroughStyleCanvas.activeSelf);
        //}
    }
    //public void SetOpacity(float value)
    //{
    //    passthrough.textureOpacity = value;
    //    ScreenCapture.CaptureScreenshot("screenshot.jpg");
    //}
    //public void SerColorMapGradient(int index)
    //{
    //    passthrough.colorMapEditorGradient = colorMapGradient[index];
    //}
    //public void SetBrightness(float value)
    //{
    //    passthrough.colorMapEditorBrightness = value;
    //}
    //public void SetContrast(float value)
    //{
    //    passthrough.colorMapEditorContrast = value;
    //    ScreenCapture.CaptureScreenshot("screenshot1.jpg");
    //}
    //public void SetPosterize(float value)
    //{
    //    passthrough.colorMapEditorPosterize = value;
    //    ScreenCapture.CaptureScreenshot("screenshot2.jpg");
    //}

    //public void SetEdgeRendering(bool value)
    //{
    //    passthrough.edgeRenderingEnabled = value;
    //}
    //public void SetEdgeRed(float value)
    //{
    //    Color newclolor = new Color(value, passthrough.edgeColor.g, passthrough.edgeColor.b);
    //    passthrough.edgeColor = newclolor;
    //}
    //public void SetEdgeGreen(float value)
    //{
    //    Color newclolor = new Color(passthrough.edgeColor.r, value, passthrough.edgeColor.b);
    //    passthrough.edgeColor = newclolor;
    //}
    //public void SetEdgeBlue(float value)
    //{
    //    Color newclolor = new Color(passthrough.edgeColor.r, passthrough.edgeColor.g, value);
    //    passthrough.edgeColor = newclolor;
    //}
    public void LoadScene()
    {
        // 加載目標場景
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
