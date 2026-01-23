using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class GameTimerScript : MonoBehaviour
{

    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public TMP_Text timerText;



    void Start()
    {
        timerIsRunning = true;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(timeRemaining);
            }
        }


        void DisplayTime(float timeDisplayed)
        {
            timeDisplayed += 1;

            float minutes = Mathf.FloorToInt(timeDisplayed / 60);
            float seconds = Mathf.FloorToInt(timeDisplayed % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }







    }

}
