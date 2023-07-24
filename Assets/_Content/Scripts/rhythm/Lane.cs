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
        private bool isStarted;

        public UnityEvent hitEvent;
        public UnityEvent notHitEvent;
        public UnityEvent missEvent;
        
        public ParticleSystem vfxGood;
        public ParticleSystem vfxBad;
        public ParticleSystem vfxExplosion;

        private float penaltyTime = 0.51f;
        private float penaltyTimer;
        
        private float protectionFromPenaltyTime = 0.51f;
        private float protectionFromPenaltyTimer;

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
            timer = 0;
            isStarted = true;
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

        private void Update()
        {
            if (!isStarted || nextEvent < 0) return;

            timer += Time.deltaTime;
            
            if (penaltyTimer > 0)
                penaltyTimer -= Time.deltaTime;
            
            if (protectionFromPenaltyTimer > 0)
                protectionFromPenaltyTimer -= Time.deltaTime;

            if (timer >= nextEvent)
                nextEvent = GetEvent(nextEvent);

            if (inputIndex >= timeStamps.Count) return;
            
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Input.GetKeyDown(input) && penaltyTimer <= 0)
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    //print($"Hit on {inputIndex} note");
                    inputIndex++;
                    protectionFromPenaltyTimer = protectionFromPenaltyTime;
                }
                else if (protectionFromPenaltyTimer <= 0)
                {
                    //print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                    Miss();
                    penaltyTimer = penaltyTime;
                }
            }

            if (timeStamp + marginOfError <= audioTime)
            {
                TimeOut();
                //print($"Missed {inputIndex} note, {timeStamp} | {marginOfError} | {audioTime}");
                inputIndex++;
            }
        }

        private void Miss()
        {
            notHitEvent?.Invoke();
            vfxBad.Emit(1);
        }

        private void Hit()
        {
            hitEvent?.Invoke();
            vfxGood.Emit(1);
        }

        private void TimeOut()
        {
            missEvent?.Invoke();
            vfxExplosion.Emit(1);
        }
    }
}