using System.Collections;
using System.IO;
using _Content.Scripts.scenes;
using _Content.Scripts.so;
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

        public float loseTimer = 60 * 3.0f;

        public string fileLocation;
        public float noteTime;
        public float noteSpawnY;
        public float noteTapY;

        public LaneVisualizator[] laneVisualizators;

        public BeatMachine beatMachine;

        public UnityEvent loseEvent;

        public SongTimer songTimer;

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
            
            songTimer.SetAudio(audioSource);

            if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
            {
                StartCoroutine(ReadFromWebsite());
            }
            else
            {
                ReadFromFile();
            }
        }
        
        private IEnumerator ReadFromWebsite()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    byte[] results = www.downloadHandler.data;
                    using (var stream = new MemoryStream(results))
                    {
                        midiFile = MidiFile.Read(stream);
                        GetDataFromMidi();
                    }
                }
            }
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
            
            songTimer.StartTimer();
            
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