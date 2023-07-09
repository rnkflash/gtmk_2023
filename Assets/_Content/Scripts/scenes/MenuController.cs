using System;
using _Content.Scripts.so;
using Melanchall.DryWetMidi.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Content.Scripts.scenes
{
    public class MenuController : MonoBehaviour
    {
        public GameLoop gameLoop;
        
        public void OnStartClicked()
        {
            Player.Instance.levels = gameLoop.Levels;
            Player.Instance.currentLevel = 0;
            SceneController.Instance.Load(Player.Instance.getCurrentLevel().sceneName);
        }
    }
}
