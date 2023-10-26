using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateId
{
    public static int InitialState = 0;
    public static int Beginstate = 1;
    public static int WaitPythonState = 2;
    public static int InGameState = 3;
}

public class GameData
{
    public static string image_name;
    public static string id_map;
}

public class Manager
{
    public static Manager self = null;

    public static Manager Instatance()
    {
        if (self == null)
        {
            self = new Manager();
        }
        return self;
    }

    private Manager() { }
    public static void ChangeState(int stateId)
    {
        SceneManager.LoadScene(stateId);
    }
}
