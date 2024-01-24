using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void replay(string sceneName)
    {
        Debug.Log("replay");
        SceneManager.LoadScene(sceneName);

    }

    public void MainMenu(string sceneName)
    {
        Debug.Log("start");
        SceneManager.LoadScene(sceneName);

    }

    public void Exit()
    {
        Debug.Log("exit");
        Application.Quit();

    }
}