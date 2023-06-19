using UnityEngine;

namespace SolarSystem {
	public class TopFollower : MonoBehaviour {
		
		[SerializeField] private Transform planet;
		[SerializeField] private float distance;

		private void Start() {
			UpdatePosition();
			transform.LookAt(planet);
		}
    
		// Update is called once per frame
		private void LateUpdate() {
			UpdatePosition();
		}

		private void UpdatePosition() {
			var pos = planet.transform.position;
			pos.y = distance;

			transform.position = pos;
		}
		
	}
}