using System;
using UnityEngine;

namespace SolarSystem {
	public class CameraRig : MonoBehaviour {

		[SerializeField] private CameraFadeToBlack fadeToBlack;
		[SerializeField] private Transform canvas;

		[Header("Camera Pivots")] 
		[SerializeField] private CameraFollower cameraFollower;
		[SerializeField] private Transform sunPivot;

		private GameObject sun;
		private Planet earth;
		private CameraPivot currentCameraPivot;

		public void Initialize(CameraRigInitializationData initializationData) {
			sun = initializationData.sun;
			earth = initializationData.earth;
		}

		public void Start() {
			if (gameObject.activeInHierarchy) {
				transform.SetParent(sunPivot, false);
				sun.GetComponent<Renderer>().enabled = false;
				canvas.SetParent(transform);
				currentCameraPivot = CameraPivot.Sun;
			}
		}

		public void SetCamera(CameraPivot pivot) {

			if (currentCameraPivot == pivot) {
				return;
			}
			
			switch (pivot) {
				case CameraPivot.Sun:
					SetCameraPivot(sunPivot);
					break;
				case CameraPivot.Earth:
					SetCameraPivot(earth.transform);
					break;
				case CameraPivot.Moon:
					break;
				case CameraPivot.Venus:
					break;
				case CameraPivot.Mars:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(pivot), pivot, null);
			}
		}
		
		private void SetCameraPivot(Transform pivot) {

			var isSunPivot = pivot == sunPivot;
			
			fadeToBlack.Transition(() => {

				if (isSunPivot) {
					SetSunPivot();
				}
				else {
					SetPlanetPivot(pivot);
				}
			});
		}
		
		private void SetSunPivot(){
			transform.SetParent(sunPivot, false);
			sun.GetComponent<Renderer>().enabled = false;
			cameraFollower.gameObject.SetActive(false);
		}

		private void SetPlanetPivot(Transform pivot) {
			cameraFollower.gameObject.SetActive(true);
			cameraFollower.Initialize(new CameraFollower.CameraFollowerInitializationData {
				sun = sun.transform,
				planet = pivot,
				angle = 35.0f,
				distance = 4.0f,
			});
			
			transform.SetParent(cameraFollower.transform, false);
			sun.GetComponent<Renderer>().enabled = true;
		}
		
	}


	public enum CameraPivot {
		Sun,
		Earth,
		Moon,
		Venus,
		Mars
	}

	public struct CameraRigInitializationData {
		public GameObject sun;
		public Planet earth;
	}
}