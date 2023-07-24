using System;
using _Content.Scripts.zuma.scratch;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Content.Scripts.zuma
{
	public class RotateLauncher : MonoBehaviour
	{
		private Vector3 lookPos;

		[SerializeField] private SkeletonAnimation frogSkeletonAnimation;
		[SerializeField] private AnimationReferenceAsset idleAnimation;
		[SerializeField] private AnimationReferenceAsset attackAnimation;
		[SerializeField] private AnimationReferenceAsset dieAndDisappearAnimation;
		[SerializeField] private AnimationReferenceAsset winLeapAnimation;
		[SerializeField] private AnimationReferenceAsset rightAttackAnimation;
		[SerializeField] private AnimationReferenceAsset leftAttackAnimation;
		
		[SerializeField] private SkeletonAnimation starSkeletonAnimation;
		[SerializeField] private AnimationReferenceAsset starDieAnimation;
		[SerializeField] private GameObject starGlowObject;
		
		[SerializeField] private SkeletonAnimation[] stopTheseSkeletonAnimations;

		[SerializeField] private Transform originPoint;
		[SerializeField] private LayerMask hitLayer;

		[SerializeField] private AudioSource hitSound;
		[SerializeField] private AudioSource missSound;

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

		public UnityEvent onShoot;

		private bool isActive;

		private int currentShootCycle;

		private void Start()
		{
			frogSkeletonAnimation.AnimationState.SetAnimation(0, idleAnimation, true);
			isActive = true;
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

		public void FireLaser(string note)
		{
			if (!isActive) return;

			var color = note switch
			{
				"Q" => Color.red,
				"W" => Color.blue,
				"E" => Color.yellow,
				"R" => Color.green,
				_ => Color.white
			};

			laserGun.Fire(color);
			
			frogSkeletonAnimation.AnimationState.SetAnimation(0, attackAnimation, false);
			frogSkeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0);

			missSound.Play();
			
			onShoot?.Invoke();
		}

		public void PlayeWinAnimationSeries()
		{
			isActive = false;
			frogSkeletonAnimation.AnimationState.SetAnimation(0, winLeapAnimation, false);
			frogSkeletonAnimation.AnimationState.AddAnimation(0, dieAndDisappearAnimation, false, 0);

			foreach (var animationSkeleton in stopTheseSkeletonAnimations)
			{
				animationSkeleton.state.GetCurrent(0).TimeScale = 0;
			}

			starSkeletonAnimation.AnimationState.SetAnimation(0, starDieAnimation, false);
			starGlowObject.SetActive(false);
		}

		public void PlayLoseAnimationSeries()
		{
			isActive = false;
			frogSkeletonAnimation.AnimationState.SetAnimation(0, winLeapAnimation, true);
			
			foreach (var animationSkeleton in stopTheseSkeletonAnimations)
			{
				animationSkeleton.state.GetCurrent(0).TimeScale = 0;
			}
		}

		public void FireBlankLaser(string note)
		{
			if (!isActive) return;
			hitSound.Play();
			FireWorks(note);
		}

		public void Aim()
		{
			if (curve == null || !isActive)
				return;

			Slime slime = curve.GetRandomSlime(Random.Range(0,4));
			if (slime != null)
				transform.LookAt (slime.transform.position, Vector3.back);
		}

		private void FireWorks(string note)
		{
			var color = Color.white;
			
			if (note == "Q")
			{
				color = Color.red;
				if (shootVfxRed1 != null)
				{
					shootVfxRed1.Emit(1);
					if (currentShootCycle == 0)
						shootVfxRed2.Emit(1); //right
					else
						shootVfxRed3.Emit(1); //left
				}
			}
			
			if (note == "W")
			{
				color = Color.blue;
				if (shootVfxBlue1 != null)
				{
					shootVfxBlue1.Emit(1);
					if (currentShootCycle == 0)
						shootVfxBlue2.Emit(1); //right
					else
						shootVfxBlue3.Emit(1); //left
				}
			}

			if (note == "E")
			{
				color = Color.yellow;
				if (shootVfxYellow1 != null)
				{
					shootVfxYellow1.Emit(1);
					if (currentShootCycle == 0)
						shootVfxYellow2.Emit(1); //right
					else
						shootVfxYellow3.Emit(1); //left
				}
			}

			if (note == "R")
			{
				color = Color.green;
				if (shootVfxGreen1 != null)
				{
					shootVfxGreen1.Emit(1);
					if (currentShootCycle == 0)
						shootVfxGreen2.Emit(1); //right
					else
						shootVfxGreen3.Emit(1); //left
				}
			}
			
			if (currentShootCycle == 0)
				frogSkeletonAnimation.AnimationState.SetAnimation(0, rightAttackAnimation, false);
			else
				frogSkeletonAnimation.AnimationState.SetAnimation(0, leftAttackAnimation, false);
			frogSkeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0);

			currentShootCycle++;
			if (currentShootCycle > 1)
				currentShootCycle = 0;
		}
		
	}
}
