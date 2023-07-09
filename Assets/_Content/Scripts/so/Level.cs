using UnityEngine;

namespace _Content.Scripts.so
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level/Level", order = 1)]
    public class Level : ScriptableObject
    {
        public string midi;
        public AudioClip clip;
        public float loseInSeconds;
        public string sceneName;
    }
}