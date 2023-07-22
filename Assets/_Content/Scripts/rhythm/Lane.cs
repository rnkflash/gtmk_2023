using System;
using System.Collections.Generic;
using _Content.Scripts.zuma.scratch;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace _Content.Scripts.rhythm
{
    public class Lane : MonoBehaviour
    {
        public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
        public KeyCode input;
        public List<double> timeStamps = new List<double>();

        int inputIndex = 0;

        private double nextEvent = -1.0d;
        private float timer;
        private bool startLane = false;
        private bool pumpingEvents = false;

        public UnityEvent fireEvent;
        public UnityEvent hitEvent;
        public UnityEvent missEvent;
        
        public ParticleSystem vfxGood;
        public ParticleSystem vfxBad;

        public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
        {
            foreach (var note in array)
            {
                if (note.NoteName == noteRestriction)
                {
                    var metricTimeSpan =
                        TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                    timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                   (double)metricTimeSpan.Milliseconds / 1000f);
                }
            }
            nextEvent = GetEvent(nextEvent);
        }

        public void StartLane()
        {
            startLane = true;
        }

        private double GetEvent(double gt)
        {
            double result = -1.0d;
            foreach (var timestamp in timeStamps)
            {
                if (gt < timestamp)
                {
                    result = timestamp;
                    break;
                }
            }
            return result;
        }

        void Update()
        {
            if (startLane)
            {
                timer = 0;
                startLane = false;
                pumpingEvents = true;
            }
            
            if (!pumpingEvents || nextEvent < 0)
                return;

            timer += Time.deltaTime;

            if (timer >= nextEvent)
            {
                nextEvent = GetEvent(nextEvent);
                fireEvent?.Invoke();
            }
            

            if (inputIndex < timeStamps.Count)
            {
                double timeStamp = timeStamps[inputIndex];
                double marginOfError = SongManager.Instance.marginOfError;
                double audioTime = SongManager.GetAudioSourceTime() -
                                   (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

                if (Input.GetKeyDown(input))
                {
                    if (Math.Abs(audioTime - timeStamp) < marginOfError)
                    {
                        Hit();
                        if (vfxGood != null)
                        {
                            vfxGood.Emit(1);
                        }
                        //print($"Hit on {inputIndex} note");
                        inputIndex++;
                    }
                    else
                    {
                        //print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                    }
                }

                if (timeStamp + marginOfError <= audioTime)
                {
                    Miss();
                    //print($"Missed {inputIndex} note, {timeStamp} | {marginOfError} | {audioTime}");
                    inputIndex++;
                    if (vfxBad != null)
                    {
                        vfxBad.Emit(1);
                    }
                }
            }
        }

        private void Hit()
        {
            hitEvent?.Invoke();        
        }

        private void Miss()
        {
            missEvent?.Invoke();
        }
    }
}