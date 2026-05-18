using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float timeLeft = 60f;
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer();
        }
        else
        {
            timeLeft = 0;
            Debug.Log("GAME OVER");
        }
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
    
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}