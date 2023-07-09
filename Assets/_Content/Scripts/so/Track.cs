using UnityEngine;

namespace _Content.Scripts.so
{
    [CreateAssetMenu(fileName = "Track", menuName = "Tracks/Track", order = 1)]
    public class Track : ScriptableObject
    {
        public string id;
        public AudioClip clip;
    }
}
