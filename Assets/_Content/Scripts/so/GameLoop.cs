using UnityEngine;

namespace _Content.Scripts.so
{
    [CreateAssetMenu(fileName = "GameLoop", menuName = "GameLoop/GameLoop", order = 1)]
    public class GameLoop : ScriptableObject
    {
        public Level[] Levels;
    }
}