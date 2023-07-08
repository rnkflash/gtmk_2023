using UnityEngine;

namespace _Content.Scripts.zuma
{
	public class BallCollider : MonoBehaviour
	{
		private bool shooterBall = false;

		void Start()
		{
			
		}

		public void MakeShooterBall(bool orly)
		{
			if (orly)
			{
				this.gameObject.tag = "NewBall";
				this.gameObject.layer = LayerMask.NameToLayer("Default");
			}
			else
			{
				this.gameObject.tag = "ActiveBalls";
				this.gameObject.layer = LayerMask.NameToLayer("ActiveBalls");
			}
			
			shooterBall = orly;
		}

		void OnCollisionEnter(Collision other)
		{
			Debug.Log(other.gameObject.tag + " " + shooterBall);
			if (other.gameObject.tag == "ActiveBalls" && shooterBall)
			{
				MakeShooterBall(false);

				this.GetComponent<Rigidbody>().velocity = Vector2.zero;
				this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

				// Get a vector from the center of the collided ball to the contact point
				ContactPoint contact = other.contacts[0];
				Vector3 CollisionDir = contact.point - other.transform.position;

				int currentIdx = other.transform.GetSiblingIndex();

				float angle  = Vector3.Angle(CollisionDir, other.transform.forward);
				/*if ( angle > 90)
					moveBallsScript.AddNewBallAt(this.gameObject, currentIdx + 1, currentIdx);
				else
					moveBallsScript.AddNewBallAt(this.gameObject, currentIdx, currentIdx);
				*/
				this.gameObject.GetComponent<BallCollider>().enabled = false;
			}
		}
	}
}
