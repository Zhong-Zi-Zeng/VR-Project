using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UserConfirm : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle toggle0;
    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;
    public Toggle toggle4;
    public Toggle toggle5;
    void Start()
    {
        // ���C�� toggle �K�[��ť��
        toggle0.onValueChanged.AddListener(delegate { ToggleChanged(toggle0, GameData.panoramaNameList[0]); });
        toggle1.onValueChanged.AddListener(delegate { ToggleChanged(toggle1, GameData.panoramaNameList[1]); });
        toggle2.onValueChanged.AddListener(delegate { ToggleChanged(toggle2, GameData.panoramaNameList[2]); });
        toggle3.onValueChanged.AddListener(delegate { ToggleChanged(toggle3, GameData.panoramaNameList[3]); });
        toggle4.onValueChanged.AddListener(delegate { ToggleChanged(toggle4, GameData.panoramaNameList[4]); });
        toggle5.onValueChanged.AddListener(delegate { ToggleChanged(toggle5, GameData.panoramaNameList[5]); });
    }
    void ToggleChanged(Toggle changedToggle, string filename)
    {
        if (changedToggle.isOn)
        {
            // �� toggle �Q�ҰʮɡA�I�s trans_api �� SendData ��k
            Initial.trans_api.SendData("Generate", filename);
            GameData.nowpanorama = Path.GetFileNameWithoutExtension(filename);
        }
        BeginState.UserConfirm();
    }
}
