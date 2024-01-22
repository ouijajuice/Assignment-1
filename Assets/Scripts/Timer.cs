using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    private bool timerRunning = false;
    private float timer = 0f;

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            timerText.text = "Time: " + timer.ToString("F2");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("StartTrigger"))
        {
            timerRunning = true;
        }
        else if (other.CompareTag("EndTrigger"))
        {
            timerRunning = false;
        }
    }

}
