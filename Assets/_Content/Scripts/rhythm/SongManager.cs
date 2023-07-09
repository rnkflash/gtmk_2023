using System.Collections;
using _Content.Scripts.scenes;
using _Content.Scripts.so;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace _Content.Scripts.rhythm
{
    public class SongManager : MonoBehaviour
    {
        public static SongManager Instance;
        public AudioSource audioSource;
        public Lane[] lanes;
        public float songDelayInSeconds;
        public double marginOfError; // in seconds

        public int inputDelayInMilliseconds;

        public float loseTimer = 60 * 3.0f;

        public string fileLocation;
        public float noteTime;
        public float noteSpawnY;
        public float noteTapY;

        public LaneVisualizator[] laneVisualizators;

        public BeatMachine beatMachine;

        public UnityEvent loseEvent;

        public float noteDespawnY
        {
            get { return noteTapY - (noteSpawnY - noteTapY); }
        }

        public static MidiFile midiFile;

        void Start()
        {
            Instance = this;

            var level = Player.Instance.getCurrentLevel();
            if (level != null)
            {
                fileLocation = level.midi;
                audioSource.clip = level.clip;
                loseTimer = level.loseInSeconds;
            }

            ReadFromFile();
        }

        private void ReadFromFile()
        {
            midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
            GetDataFromMidi();
        }

        public void GetDataFromMidi()
        {
            var notes = midiFile.GetNotes();
            var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
            notes.CopyTo(array, 0);

            foreach (var lane in lanes) lane.SetTimeStamps(array);
            foreach (var lane in laneVisualizators) lane.ImportLane();

            Invoke(nameof(StartSong), songDelayInSeconds);
        }

        public void StartSong()
        {
            audioSource.Play();
            
            foreach (var lane in lanes) lane.StartLane();
            foreach (var lane in laneVisualizators) lane.StartSong();
            beatMachine.StartMF();

            loseCoroutine = LoseAfter(loseTimer); 
            StartCoroutine(loseCoroutine);
        }

        private IEnumerator loseCoroutine = null;

        public void StopLoseTimer()
        {
            if (loseCoroutine != null)
            {
                StopCoroutine(loseCoroutine);
                loseCoroutine = null;
            }
        }

        IEnumerator LoseAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            loseEvent?.Invoke();
        }

        public static double GetAudioSourceTime()
        {
            return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
        }
    }
}