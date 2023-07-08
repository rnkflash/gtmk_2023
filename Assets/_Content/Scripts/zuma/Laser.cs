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
            lineRenderer.enabled = true;
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

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            if (!lineRenderer.enabled) return;

            Ray ray = new Ray(originPoint.position, originPoint.forward);
            bool cast = Physics.Raycast(ray, out RaycastHit hit, maxDistance);
            Vector3 hitPosition = cast ? hit.point : originPoint.position + originPoint.forward * maxDistance;
            lineRenderer.SetPosition(0, originPoint.position);
            lineRenderer.SetPosition(1, hitPosition);
        }
    }
}
