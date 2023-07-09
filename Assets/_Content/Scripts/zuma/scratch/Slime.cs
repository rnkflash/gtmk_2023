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
        [HideInInspector] public Curve curve;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private float jumpPowerMin = 0.1f;
        [SerializeField] private float jumpPowerMax = 0.2f;
        [SerializeField] private float jumpDuration = 0.5f;

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
            this.transform.DOJump(destination, Random.Range(jumpPowerMin, jumpPowerMax), 1, jumpDuration);
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
            this.transform.DOJump(this.transform.position, Random.Range(jumpPowerMin, jumpPowerMax), 1, jumpDuration);
        }

        public void UpdateSortingOrder()
        {
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 10.0f);
        }

        public void SetSortingOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }

        public void Die()
        {
            curve?.DestroySlime(this);
        }
    }
}