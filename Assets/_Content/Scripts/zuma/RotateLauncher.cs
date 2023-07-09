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

		public ParticleSystem shootVfx;

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
			var color = Color.blue;
			if (note == "W")
				color = Color.green;
			if (note == "E")
				color = Color.red;
			if (note == "R")
				color = Color.yellow;
			
			shootVfx.Emit(1);
			
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
