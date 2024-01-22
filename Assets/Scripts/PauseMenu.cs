using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused == true)
        {
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
        }
    }
}

