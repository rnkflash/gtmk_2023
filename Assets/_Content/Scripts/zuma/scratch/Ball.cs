using System;
using UnityEngine;

namespace _Content.Scripts.zuma.scratch
{
    public class Ball: MonoBehaviour
    {
        private int index;
        private State state;
        
        private enum State
        {
            Moving, Idle, Shooting
        }

        private void Start()
        {
            state = State.Moving;
        }

        private void Update()
        {
            switch (state)
            {
                case State.Idle or State.Shooting:
                    return;
                case State.Moving:
                    Move();
                    break;
            }
        }

        private void Move()
        {
            
        }
    }
}