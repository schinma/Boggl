using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timerLength;

    public float time = 0;

    private void Update()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
        }
    }

    public void RestartTimer()
    {
        time = timerLength;
    }
}
