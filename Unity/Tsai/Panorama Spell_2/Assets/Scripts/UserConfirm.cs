using System.Collections;
using System.Collections.Generic;
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
        toggle0.onValueChanged.AddListener(delegate { ToggleChanged(toggle0, "panorama.jpg"); });
        toggle1.onValueChanged.AddListener(delegate { ToggleChanged(toggle1, "panorama2.jpg"); });
        toggle2.onValueChanged.AddListener(delegate { ToggleChanged(toggle2, "panorama3.jpg"); });
        toggle3.onValueChanged.AddListener(delegate { ToggleChanged(toggle3, "panorama4.jpg"); });
        toggle4.onValueChanged.AddListener(delegate { ToggleChanged(toggle4, "panorama5.jpg"); });
        toggle5.onValueChanged.AddListener(delegate { ToggleChanged(toggle5, "panorama6.jpg"); });
    }
    void ToggleChanged(Toggle changedToggle, string filename)
    {
        if (changedToggle.isOn)
        {
            // �� toggle �Q�ҰʮɡA�I�s trans_api �� SendData ��k
            Initial.trans_api.SendData("Generate", filename);
        }
        BeginState.UserConfirm();
    }
}
