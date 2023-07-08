using System;
using System.Collections.Generic;
using _Content.Scripts.zuma.scratch;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

namespace _Content.Scripts.rhythm
{
    public class Lane : MonoBehaviour
    {
        public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
        public KeyCode input;
        public GameObject notePrefab;
        List<Note> notes = new List<Note>();
        public List<double> timeStamps = new List<double>();

        int spawnIndex = 0;
        int inputIndex = 0;

        private double nextEvent = -1.0d;
        private float timer;
        private bool startLane = false;

        public Curve curve; 

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
            if (!startLane || nextEvent < 0)
                return;

            timer += Time.deltaTime;

            if (timer >= nextEvent)
            {
                nextEvent = GetEvent(nextEvent);
                curve.MoveSlimes();
            }
            

            /*
            if (spawnIndex < timeStamps.Count)
            {
                if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
                {
                    var note = Instantiate(notePrefab, transform);
                    notes.Add(note.GetComponent<Note>());
                    note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                    spawnIndex++;
                }
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
                        print($"Hit on {inputIndex} note");
                        Destroy(notes[inputIndex].gameObject);
                        inputIndex++;
                    }
                    else
                    {
                        print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                    }
                }

                if (timeStamp + marginOfError <= audioTime)
                {
                    Miss();
                    print($"Missed {inputIndex} note");
                    inputIndex++;
                }
            }
            */
        }

        private void Hit()
        {
        }

        private void Miss()
        {
        }
    }
}