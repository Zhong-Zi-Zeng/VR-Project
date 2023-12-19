using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingTextAnimator : MonoBehaviour
{
    public TextMeshProUGUI waitingText; // �ѦҨ�A��Text�ե�
    private string[] waitingStates = new string[] { "Waiting", "Waiting.", "Waiting..", "Waiting...", "Waiting....", "Waiting....." };
    private int currentState = 0;

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        while (true)
        {
            waitingText.text = waitingStates[currentState];
            currentState = (currentState + 1) % waitingStates.Length;
            yield return new WaitForSeconds(0.5f); // ���ݮɶ��A�i�H�վ�
        }
    }
}
