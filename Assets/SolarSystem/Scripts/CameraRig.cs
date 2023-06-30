using System;
using UnityEngine;

namespace SolarSystem {
	public class CameraRig : MonoBehaviour {

		[SerializeField] private CameraFadeToBlack fadeToBlack;

		[Header("Camera Pivots")] 
		[SerializeField] private CameraFollower cameraFollower;
		[SerializeField] private Transform sunPivot;

		private GameObject sun;
		private Planet earth;
		

		public void Initialize(CameraRigInitializationData initializationData) {
			sun = initializationData.sun;
			earth = initializationData.earth;
		}

		public void Start() {
			if (gameObject.activeInHierarchy) {
				transform.SetParent(sunPivot, false);
				sun.GetComponent<Renderer>().enabled = false;
			}
		}

		public void SetCamera(CameraPivot pivot) {

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
				
				transform.SetParent(pivot.transform, false);
				sun.GetComponent<Renderer>().enabled = !isSunPivot;
				cameraFollower.gameObject.SetActive(!isSunPivot);
				if (!isSunPivot) {
					cameraFollower.Initialize(new CameraFollower.CameraFollowerInitializationData {
						sun = sun.transform,
						planet = pivot,
						angle = 35.0f,
						distance = 4.0f,
					});
				}
			});
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