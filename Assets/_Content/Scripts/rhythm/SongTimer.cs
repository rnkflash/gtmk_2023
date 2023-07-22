using System;
using TMPro;
using UnityEngine;

namespace _Content.Scripts.rhythm
{
    public class SongTimer : MonoBehaviour
    {
        public TMP_Text timerText
            ;
        private float timeLeft;
        private AudioSource song;
        private float updateCooldown = 0.1f;
        private bool isStarted;

        private void Start()
        {
            timerText.text = "00:00";
        }

        public void SetAudio(AudioSource song)
        {
            this.song = song;
            RefreshTimer();
        }

        public void StartTimer()
        {
            isStarted = true;
        }

        private void RefreshTimer()
        {
            timeLeft = song.clip.length - song.time;
            var timeLeftSeconds = Math.Floor(timeLeft);
            timerText.text = $"{(timeLeftSeconds / 60):00}:{(timeLeftSeconds % 60):00}";
        }

        public void Update()
        {
            if (!isStarted) return;
            
            updateCooldown -= Time.deltaTime;
            if (updateCooldown <= 0)
            {
                updateCooldown = 0.1f;
                RefreshTimer();
            }
        }
    }
}
