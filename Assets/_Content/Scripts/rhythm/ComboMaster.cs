using System;
using _Content.Scripts.zuma;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Content.Scripts.rhythm
{
    public class ComboMaster : MonoBehaviour
    {
        [SerializeField] private ComboLevel[] levels;
        [SerializeField] private BeatMachine beatMachine;
        [SerializeField] private TMP_Text comboLevelText;
        [SerializeField] private RotateLauncher frog;

        [HideInInspector] public int level;
        private int currentCombo;

        public UnityEvent onComboLevelUp;
        public UnityEvent onComboBreak;

        [Serializable]
        struct ComboLevel
        {
            public int combo;
            public int bpm;
        }

        private void Start()
        {
            comboLevelText.text = "Combo Level " + level;
        }

        public void Hit()
        {
            currentCombo++;
            
            if (level < levels.Length - 1)
            {
                var nextLevel = levels[level + 1].combo;
                var baseCombo = 0;
                for (int i = 0; i <= level; i++)
                {
                    baseCombo += levels[i].combo;
                }

                if (currentCombo >= baseCombo + nextLevel)
                {
                    level++;
                    ComboLevelUp();
                }
            }
        }

        public void Miss()
        {
            currentCombo = 0;
            level = 0;
            ComboBreak();
        }

        private void ComboLevelUp()
        {
            onComboLevelUp?.Invoke();
            beatMachine.SetBPM(levels[level].bpm);
            
            comboLevelText.text = "Combo Level " + level;
            
            frog.SetIdleAnimation(level);
        }

        private void ComboBreak()
        {
            onComboBreak?.Invoke();
            beatMachine.SetBPM(levels[level].bpm);

            comboLevelText.text = "Combo Level " + level;
            
            frog.SetIdleAnimation(level);
        }
    }
}
