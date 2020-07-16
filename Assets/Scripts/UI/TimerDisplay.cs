using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    public Timer timer;
    public Text timerText;
    public AudioManager audioManager;

    private bool audioCountDownPlaying = false;

    private void Update()
    {
        if (timer.time >= 0) {

            int second = Mathf.FloorToInt(timer.time);
            string minutes = Mathf.Floor(second / 60).ToString("00");
            string seconds = (second % 60).ToString("00");
            timerText.text = string.Format("{0}:{1}", minutes, seconds);

            if (!audioCountDownPlaying && (timer.time <= 11f && timer.time >= 10.9f))
            {
                StartCoroutine(PlayTimerSound(1.0f, 10));
            }
        }
    }

    IEnumerator PlayTimerSound(float duration, int times)
    {
        audioCountDownPlaying = true;
        for (int i = 0; i < times; i++)
        {
            audioManager.PlayTimer();
            yield return new WaitForSeconds(duration);
        }
        audioCountDownPlaying = false;
    }
}
