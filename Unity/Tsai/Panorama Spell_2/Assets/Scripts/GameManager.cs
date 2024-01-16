using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateId
{
    public static int BeginState = 0;
    public static int WaitingPythonState = 1;
    public static int InGameState = 2;
    public static int LearningState = 3;
}


public class SceneName
{
    public static string InitialScene = "Scenes/InitialScene";
    public static string BeginScene = "Scenes/BeginScene";
    public static string WaitingPythonScene = "Scenes/WaitingPythonScene";
    public static string InGameScene = "Scenes/InGameScene";
    public static string LearningScene = "Scenes/LearningScene";
}

public class GameData
{
    public static List<byte[]> panoramaWithMaskList = new(); // �a��mask��panorama
    public static List<byte[]> panoramaList = new(); // �@�몺panorama�A�s��python����panorama�Ӥ�
    public static List<string> panoramaNameList = new();
    public static byte[] idMap = new byte[0]; //�Ψ��x�s�C��pixel��������@�Ӫ���
    public static Texture2D idMapTexture;
    public static byte[] indexMap = new byte[0]; // �Ψ��x�s�C��pixel��������@�imask
    public static Texture2D indexMapTexture;
    public static int progress; // �i�ױ��ϥ�
    public static string text;
    public static string nowpanorama;
}

public class GameManager
{
    private static FSM fsm = new();
    /// <summary>
    /// ��Ҧ����A�[�J��fsm��
    /// </summary>
    public static void AddAllStates()
    {        
        fsm.AddState(new BeginState());
        fsm.AddState(new WaitingPythonState());
        fsm.AddState(new InGameState());
        fsm.AddState(new LearningState());
    }

    /// <summary>
    /// �@���C�Ӫ��A�����f�A�i�z�L�o�Ө禡�h�������A
    /// </summary>
    public static void ChangeState(int stateId)
    {
        fsm.SetState(stateId);
    }

    /// <summary>
    /// �@���C�Ӫ��A�����f�A�i�z�L�o�Ө禡�h����Scene
    /// </summary>
    public static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}