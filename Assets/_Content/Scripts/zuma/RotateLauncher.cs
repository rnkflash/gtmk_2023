using _Content.Scripts.zuma.scratch;
using UnityEngine;

namespace _Content.Scripts.zuma
{
	public class RotateLauncher : MonoBehaviour
	{
		private Vector3 lookPos;

		[SerializeField] private Transform originPoint;
		[SerializeField] private LayerMask hitLayer;

		public Laser laserGun;
		public Curve curve;

		public ParticleSystem shootVfxBlue1;
		public ParticleSystem shootVfxBlue2;
		public ParticleSystem shootVfxBlue3;
		
		public ParticleSystem shootVfxRed1;
		public ParticleSystem shootVfxRed2;
		public ParticleSystem shootVfxRed3;
		
		public ParticleSystem shootVfxYellow1;
		public ParticleSystem shootVfxYellow2;
		public ParticleSystem shootVfxYellow3;
		
		public ParticleSystem shootVfxGreen1;
		public ParticleSystem shootVfxGreen2;
		public ParticleSystem shootVfxGreen3;

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

		private void ShootBall()
		{
			var hitPoint = RayCastBalls();
			Debug.Log(hitPoint);
		}

		private Vector2 RayCastBalls()
		{
			var start = new Vector2(originPoint.position.x, originPoint.position.y);
			var end = new Vector2(originPoint.forward.x, originPoint.forward.y);
			var hit2D = Physics2D.Raycast(start, end, 100f, hitLayer);
			return hit2D.collider ? hit2D.point : start + end * 100.0f;
		}

		public void FireLaser(string note)
		{
			var color = Color.white;
			
			if (note == "Q")
			{
				color = Color.blue;
				shootVfxBlue1.Emit(1);
				shootVfxBlue2.Emit(1);
				shootVfxBlue3.Emit(1);
			}
			
			if (note == "W")
			{
				color = Color.green;
				shootVfxGreen1.Emit(1);
				shootVfxGreen2.Emit(1);
				shootVfxGreen3.Emit(1);
			}

			if (note == "E")
			{
				color = Color.red;
				shootVfxRed1.Emit(1);
				shootVfxRed2.Emit(1);
				shootVfxRed3.Emit(1);
			}

			if (note == "R")
			{
				color = Color.yellow;
				shootVfxYellow1.Emit(1);
				shootVfxYellow2.Emit(1);
				shootVfxYellow3.Emit(1);
			}
				
			
			
			
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

		public void Aim()
		{
			if (curve == null)
				return;
			
			

			Slime slime = curve.GetRandomSlime(Random.Range(0,4));
			if (slime != null)
				transform.LookAt (slime.transform.position, Vector3.back);
		}
		
	}
}
