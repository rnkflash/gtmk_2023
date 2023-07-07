using DG.Tweening;
using UnityEngine;

namespace _Content.Scripts.scenes
{
	public class BootController : MonoBehaviour
	{
		private void Start()
		{
			Application.targetFrameRate = 60;
			DOTween.Init();
			SceneController.Instance.Load("intro");
		}
	}
}