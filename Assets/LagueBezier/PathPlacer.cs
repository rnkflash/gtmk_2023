using System.Linq;
using UnityEngine;

namespace _Content.Scripts.zuma.scratch.bezier
{
	public class PathPlacer : MonoBehaviour {

		public float spacing = .1f;
		public float resolution = 1;

		[HideInInspector] public Vector3[] path;
	
		public Vector3[] CalculatePath () {
			Vector2[] points = FindObjectOfType<PathCreator>().path.CalculateEvenlySpacedPoints(spacing, resolution);
			path = points.Select((p) => new Vector3(p.x, p.y, 0.0f)).ToArray();
			return path;
		}

		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			foreach (Vector2 p in path)
				Gizmos.DrawSphere(p, spacing * .5f);
		}
	
	
	}
}
