using UnityEngine;

namespace SolarSystem {
	public class RotationAxisGizmo : MonoBehaviour {

		//[SerializeField] private BoxCollider collider;

		public void Initialize(float planetSize) {
			var scale = transform.localScale;
			scale.y = planetSize * 1.1f;

			transform.localScale = scale;
		}
	}
}