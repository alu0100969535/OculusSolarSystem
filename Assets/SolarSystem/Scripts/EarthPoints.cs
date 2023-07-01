using UnityEngine;

namespace SolarSystem {
	public class EarthPoints : MonoBehaviour {

		[SerializeField] private GameObject[] points;

		private void Start() {
			Disable();
		}

		public GameObject GetPoint() {

			var randomIndex = Random.Range(0, points.Length);

			var point = points[randomIndex];
			point.SetActive(true);

			return point;
		}

		public void Disable() {
			foreach (var point in points) {
				point.SetActive(false);
			}
		}

	}
}