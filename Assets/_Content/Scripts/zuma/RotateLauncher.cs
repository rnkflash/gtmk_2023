﻿using UnityEngine;

namespace _Content.Scripts.zuma
{
	public class RotateLauncher : MonoBehaviour
	{
		public float ballSpeed = 10;

		private Vector3 lookPos;

		[SerializeField] private Transform originPoint;
		[SerializeField] private LayerMask hitLayer;

		public Laser laserGun; 

		private void Start()
		{
			CreateBall();
		}

		private void Update ()
		{
			RotatePlayerAlongMousePosition();
			SetBallPostion();
			if (Input.GetKeyDown(KeyCode.Mouse0))
				ShootBall();
		}

		private void RotatePlayerAlongMousePosition ()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100.0f))
				lookPos = hit.point;

			Vector3 lookDir = lookPos - transform.position;
			lookDir.z = 0;

			transform.LookAt (transform.position + lookDir, Vector3.back);
		}

		private void SetBallPostion()
		{
			//instanceBall.transform.forward = transform.forward;
			//instanceBall.transform.position = transform.position + transform.forward * transform.localScale.z;
		}

		private void ShootBall()
		{
			var hitPoint = RayCastBalls();
			//instanceBall.transform.position = hitPoint;
			//instanceBall.GetComponent<Rigidbody>().AddForce(instanceBall.transform.forward * ballSpeed);
			
			//raycast from origin to distance
			//if no hit ball disappears
			//if hit ball position changes to hit - radius
			//hitted ball initiates collision logic
			
			CreateBall();
		}

		private Vector3 RayCastBalls()
		{
			Ray ray = new Ray(originPoint.position, originPoint.forward);
			bool cast = Physics.Raycast(ray, out RaycastHit hit, 100.0f, hitLayer);
			if (cast)
			{
				
			}
			return cast ? hit.point : originPoint.position + originPoint.forward * 100.0f;
		}

		private void CreateBall()
		{
			//instanceBall = Instantiate(moveBalls.GetRandomBall(), transform.position, Quaternion.identity);
			//instanceBall.SetActive(true);
			//instanceBall.GetComponent<BallCollider>().MakeShooterBall(true);
		}

		public void FireLaser(string note)
		{
			var color = Color.blue;
			if (note == "W")
				color = Color.green;
			if (note == "E")
				color = Color.red;
			if (note == "R")
				color = Color.yellow;
			
			laserGun.Fire(color);
		}

		public void NotePlayedQ()
		{
			FireLaser("Q");
		}
		
		public void NotePlayedW()
		{
			FireLaser("W");
		}
		
		public void NotePlayedE()
		{
			FireLaser("E");
		}
		
		public void NotePlayedR()
		{
			FireLaser("R");
		}
	}
}
