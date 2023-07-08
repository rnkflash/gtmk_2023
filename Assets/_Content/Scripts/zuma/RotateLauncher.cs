using UnityEngine;

namespace _Content.Scripts.zuma
{
	public class RotateLauncher : MonoBehaviour
	{
		public float ballSpeed = 10;
		[HideInInspector] public GameObject instanceBall;

		private Vector3 lookPos;

		[SerializeField] private Transform originPoint;
		[SerializeField] private LayerMask hitLayer;

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

		// Rotate the launcher along the mouse position
		private void RotatePlayerAlongMousePosition ()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100.0f))
				lookPos = hit.point;

			Vector3 lookDir = lookPos - transform.position;
			lookDir.y = 0;

			transform.LookAt (transform.position + lookDir, Vector3.up);
		}

		// Set balls postions and forward w.r.t to the launcher
		private void SetBallPostion()
		{
			instanceBall.transform.forward = transform.forward;
			instanceBall.transform.position = transform.position + transform.forward * transform.localScale.z;
		}

		private void ShootBall()
		{
			var hitPoint = RayCastBalls();
			instanceBall.transform.position = hitPoint;
			instanceBall.GetComponent<Rigidbody>().AddForce(instanceBall.transform.forward * ballSpeed);
			
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
	}
}
