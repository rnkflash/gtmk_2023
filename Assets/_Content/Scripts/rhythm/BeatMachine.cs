using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatMachine : MonoBehaviour
{
    public UnityEvent fireThis;
    public int bpm = 100;
    private bool hasStarted = false;
    private float timePassed = 0.0f;
    private float nextBeat = 0.0f;
    private float beatSpacing;
    private float cachedNextBeat = 0.0f;

    public void StartMF()
    {
        timePassed = 0.0f;
        beatSpacing = 60.0f / bpm;
        nextBeat = beatSpacing; 
        hasStarted = true;
    }

    private void Update()
    {
        if (!hasStarted)
            return;

        timePassed += Time.deltaTime;

        if (timePassed >= nextBeat)
        {
            cachedNextBeat = nextBeat;
            fireThis?.Invoke();
            nextBeat = GetNextOverShoot(cachedNextBeat, timePassed, beatSpacing);
        }
    }

    private float GetNextOverShoot(float initial, float target, float shoot)
    {
        var current = initial;
        while (current < target)
        {
            if (current >= target)
            {
                break;
            }

            current += shoot;
        }

        return current;
    }

    public void SetBPM(int bpm)
    {
        this.bpm = bpm;
        beatSpacing = 60.0f / bpm;
    }
}
