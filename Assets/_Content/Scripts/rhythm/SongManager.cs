using System.Collections;
using System.IO;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

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


        public string fileLocation;
        public float noteTime;
        public float noteSpawnY;
        public float noteTapY;

        public float noteDespawnY
        {
            get { return noteTapY - (noteSpawnY - noteTapY); }
        }

        public static MidiFile midiFile;

        void Start()
        {
            Instance = this;
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

            Invoke(nameof(StartSong), songDelayInSeconds);
        }

        public void StartSong()
        {
            audioSource.Play();
            foreach (var lane in lanes) lane.StartLane();
        }

        public static double GetAudioSourceTime()
        {
            return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
        }
    }
}