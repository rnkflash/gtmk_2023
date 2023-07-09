using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Content.Scripts.zuma.scratch
{
    public class Slime : MonoBehaviour
    {
        [HideInInspector] public int tile;
        [HideInInspector] public bool moved = false;
        [HideInInspector] public int type = 0;
        [SerializeField] private Sprite[] sprites;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void SetType(int _type)
        {
            type = _type;
            GetComponentInChildren<SpriteRenderer>().sprite = sprites[type];
        }
        
        public void Move(Vector3 destination)
        {
            moved = true;
            //this.transform.DOMove(destination, 0.5f);
            this.transform.DOJump(destination, Random.Range(0.1f, 0.2f), 1, 0.5f);
        }

        public void FinishMovements()
        {
            this.transform.DOComplete();
            moved = false;
            UpdateSortingOrder();
        }

        public void Jump()
        {
            moved = true;
            this.transform.DOJump(this.transform.position, Random.Range(0.1f, 0.2f), 1, 0.5f);
        }

        public void UpdateSortingOrder()
        {
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 10.0f);
        }

        public void SetSortingOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }
    }
}