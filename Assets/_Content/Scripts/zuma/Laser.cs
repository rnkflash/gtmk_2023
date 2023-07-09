using System;
using UnityEngine;

namespace _Content.Scripts.zuma
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform originPoint;
        [SerializeField] private float maxDistance;

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

            Ray ray = new Ray(originPoint.position, originPoint.forward);
            bool cast = Physics.Raycast(ray, out RaycastHit hit, maxDistance);
            Vector3 hitPosition = cast ? hit.point : originPoint.position + originPoint.forward * maxDistance;
            lineRenderer.SetPosition(0, originPoint.position);
            lineRenderer.SetPosition(1, hitPosition);
            
            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0)
            {
                laserTimer = 0;
                Deactivate();
            }
            
        }

        private float laserTimer = 0.0f;

        public void Fire(Color color)
        {
            Activate();
            laserTimer = 0.1f;
        }
    }
}
