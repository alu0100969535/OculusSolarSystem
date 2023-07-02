using Unity.XR.CoreUtils;
using UnityEngine;

namespace SolarSystem {
	public class EarthPoints : MonoBehaviour {
		
		[SerializeField] private GameObject[] points;
		[SerializeField] private Camera camera;
		[SerializeField] private float cameraDistance;

		private void Start() {
			Disable();
		}

		public GameObject GetPoint() {

			var randomIndex = Random.Range(0, points.Length);

			var point = points[randomIndex];
			point.SetActive(true);

			SetCameraInFrontOfPoint(point);

			return point;
		}

		public void Disable() {
			foreach (var point in points) {
				point.SetActive(false);
			}
			
			camera.gameObject.SetActive(false);
		}

		private void SetCameraInFrontOfPoint(GameObject point) {
			camera.gameObject.SetActive(true);
			
			var centerP = transform.position;
			var pointP = point.transform.position;

			var direction = (pointP - centerP).normalized;

			var newCameraPos = centerP + direction * cameraDistance;
			camera.transform.position = newCameraPos;
			
			camera.transform.LookAt(transform.position);
			
		}

	}
}