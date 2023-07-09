using System;
using _Content.Scripts.zuma.scratch;
using UnityEngine;

namespace _Content.Scripts.zuma
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform originPoint;
        [SerializeField] private float maxDistance;
        [SerializeField] private LayerMask hitLayer;

        private void Awake()
        {
            Deactivate();
        }

        private void Activate()
        {
            lineRenderer.enabled = true;
        }
        
        private void Deactivate()
        {
            lineRenderer.enabled = false;
            lineRenderer.SetPosition(0, originPoint.position);
            lineRenderer.SetPosition(1, originPoint.position);
        }

        private void FixedUpdate()
        {
            if (!lineRenderer.enabled) return;
            
            var hit = Raycast();
            lineRenderer.SetPosition(0, originPoint.position);
            lineRenderer.SetPosition(1, new Vector3(hit.x, hit.y, 0));

            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0)
            {
                laserTimer = 0;
                Deactivate();
            }
            
        }

        private Vector2 Raycast(float radius = 0.0f)
        {
            var start = new Vector2(originPoint.position.x, originPoint.position.y);
            var end = new Vector2(originPoint.forward.x, originPoint.forward.y);
            RaycastHit2D hit2D;
            if (radius <= 0.0f)
                hit2D = Physics2D.Raycast(start, end, 100f, hitLayer);
            else
                hit2D = Physics2D.CircleCast(start, 0.42f, end, 100f, hitLayer);
            return hit2D.collider ? hit2D.point : start + end * 100.0f;
        }
        
        private Slime RaycastSlime(float radius = 0.0f)
        {
            var start = new Vector2(originPoint.position.x, originPoint.position.y);
            var end = new Vector2(originPoint.forward.x, originPoint.forward.y);
            RaycastHit2D hit2D;
            if (radius <= 0.0f)
                hit2D = Physics2D.Raycast(start, end, 100f, hitLayer);
            else
                hit2D = Physics2D.CircleCast(start, 0.42f, end, 100f, hitLayer);
            if (hit2D.collider != null)
            {
                return hit2D.collider.gameObject.GetComponentInParent<Slime>();
            }

            return null;
        }
        
        private float laserTimer = 0.0f;

        public void Fire(Color color)
        {
            var slime = RaycastSlime(0.42f);
            if (slime != null && slime.isAlive)
            {
                slime.HitByLaser();
            }
            
            Activate();
            laserTimer = 0.1f;
        }
    }
}
