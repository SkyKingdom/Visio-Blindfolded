using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timeStamp;
    private float interval;
    private float pauseDifference;

    public bool isPaused { get; private set; }
    public bool isActive { get; private set; }


    /// <summary>
    /// Returns the time left on the timer.
    /// </summary>
    /// <returns></returns>
    public float TimeLeft()
    {
        return TimerDone() ? 0 : (1 - TimerProgress()) * interval;
    }

    /// <summary>
    /// returns how long the timer has been running for.
    /// </summary>
    /// <returns></returns>
    public float TimerProgress()
    {
        return (isPaused) ? (interval - pauseDifference / interval) : TimerDone() == true ? 1 : Mathf.Abs((timeStamp - Time.time) / interval);
    }

    /// <summary>
    /// Returns if the timer is finished
    /// </summary>
    /// <returns></returns>
    public bool TimerDone()
    {
        return (isPaused) ? pauseDifference == 0.0f : Time.time >= timeStamp + interval ? true : false;
    }

    /// <summary>
    /// Sets a timer with the given input.
    /// </summary>
    /// <param name="_interval"></param>
    public void SetTimer(float _interval = 2)
    {
        timeStamp = Time.time;
        interval = _interval;
        isActive = true;
    }

    /// <summary>
    /// Restarts the timer with the interval that it previously had.
    /// </summary>
    public void RestartTimer()
    {
        SetTimer(interval);
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void StopTimer()
    {
        isActive = false;
        timeStamp = interval;
    }

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    /// <param name="pause"></param>
    public void PauseTimer(bool pause)
    {
        if (pause)
        {
            pauseDifference = TimeLeft();
            isPaused = pause;
            return;
        }
        isPaused = pause;
        timeStamp = Time.time - (interval - pauseDifference);
    }
}
