using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToggleTest : MonoBehaviour
{
    // Start is called before the first frame update

    
    private void Start()
    {
        
    }

    public void LoadScene()
    {
        
            // �[���ؼг���
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        
    }
}
