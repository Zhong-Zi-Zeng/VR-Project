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
    public static List<byte[]> panoramaWithMaskList = new(); // 帶有mask的panorama
    public static List<byte[]> panoramaList = new(); // 一般的panorama，存放python有的panorama照片
    public static List<string> panoramaNameList = new();
    public static byte[] idMap = new byte[0]; //用來儲存每個pixel對應到哪一個物件
    public static Texture2D idMapTexture;
    public static byte[] indexMap = new byte[0]; // 用來儲存每個pixel對應到哪一張mask
    public static Texture2D indexMapTexture;
    public static int progress; // 進度條使用
    public static string text;
    public static string nowpanorama;
}

public class GameManager
{
    private static FSM fsm = new();
    /// <summary>
    /// 把所有狀態加入到fsm中
    /// </summary>
    public static void AddAllStates()
    {        
        fsm.AddState(new BeginState());
        fsm.AddState(new WaitingPythonState());
        fsm.AddState(new InGameState());
        fsm.AddState(new LearningState());
    }

    /// <summary>
    /// 作為每個狀態的接口，可透過這個函式去切換狀態
    /// </summary>
    public static void ChangeState(int stateId)
    {
        fsm.SetState(stateId);
    }

    /// <summary>
    /// 作為每個狀態的接口，可透過這個函式去切換Scene
    /// </summary>
    public static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}