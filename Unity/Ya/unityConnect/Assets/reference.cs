// Server
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamRender : MonoBehaviour
{
    public RawImage rawImage; // Rendering on UI
    private WebCamTexture webCamTexture;
    private bool camAvailable = false;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }

        webCamTexture = new WebCamTexture(devices[0].name, 640, 360, 60);

        if (webCamTexture == null)
        {
            Debug.Log("Unable to find camera");
            return;
        }

        webCamTexture.requestedFPS = 60f;

        webCamTexture.Play();
        rawImage.texture = webCamTexture;

        camAvailable = true;
    }

    private void Update()
    {
        OpenCameraUseInUpdate();
    }

    /// <summary>
    ///  Open camera coroutine
    /// </summary>
    public void ToOpenCamCoroutine()
    {
        StartCoroutine("OpenCamera");
    }

    public IEnumerator OpenCamera()
    {
        int maxl = Screen.height;

        if (Screen.height > Screen.width)
        {
            maxl = Screen.height;
        }

        // Access webcam
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            if (webCamTexture != null)
            {
                webCamTexture.Stop();
            }


            // Activate rawImage OBJ 
            if (rawImage != null)
            {
                rawImage.gameObject.SetActive(true);
            }

            // Busy waiting for checking if first webcam accessing connect to device succesfully.
            int i = 0;
            while (WebCamTexture.devices.Length <= 0 && 1 < 300)
            {
                yield return new WaitForEndOfFrame();
                i++;
            }

            // List usable webcam 
            WebCamDevice[] devices = WebCamTexture.devices;

            if (WebCamTexture.devices.Length <= 0)
            {
                Debug.LogError("No webcam. Please check.");
            }
            else
            {
                string devicename = devices[0].name;

                webCamTexture = new WebCamTexture(devicename, maxl, maxl == Screen.height ? Screen.width : Screen.height, 30)
                {
                    wrapMode = TextureWrapMode.Repeat
                };

                // Render to UI
                if (rawImage != null)
                {
                    rawImage.texture = webCamTexture;
                }

                webCamTexture.Play();
            }

        }
        else
        {
            Debug.LogError("Webcam access denial.");
        }
    }

    public void OpenCameraUseInUpdate()
    {
        if (!camAvailable)
            return;

        float scaleY = webCamTexture.videoVerticallyMirrored ? -1f : 1f;
        // rawImage.rectTransform.localScale = new Vector3 (1f, scaleY, 1f);    //«DÃè¹³
        rawImage.rectTransform.localScale = new Vector3(-1f, scaleY, 1f);    //Ãè¹³

        int orient = -webCamTexture.videoRotationAngle;
        rawImage.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    private void OnApplicationPause(bool pause)
    {
        // Pausing camera when application pause.
        if (webCamTexture != null)
        {
            if (pause)
            {
                webCamTexture.Pause();
            }
            else
            {
                webCamTexture.Play();
            }
        }

    }

    private void OnDestroy()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
        }
    }
}
