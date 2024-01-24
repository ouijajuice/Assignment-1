using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenTrigger : MonoBehaviour
{
    public string winScreen;
    private void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene(winScreen);
    }
}
