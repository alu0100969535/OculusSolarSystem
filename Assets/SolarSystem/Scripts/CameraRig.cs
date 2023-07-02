using System;
using UnityEngine;

namespace SolarSystem {
	public class CameraRig : MonoBehaviour {

		[SerializeField] private CameraFadeToBlack fadeToBlack;
		[SerializeField] private Transform canvas;
		[SerializeField] private Transform centerEyeTransform;

		[Header("Camera Pivots")] 
		[SerializeField] private CameraFollower cameraFollower;
		[SerializeField] private Transform sunPivot;

		private GameObject sun;
		private CelestialBody earth;
		private CameraPivot currentCameraPivot = CameraPivot.Unassigned;

		public Transform CenterEyeTransform => centerEyeTransform;

		public void Initialize(CameraRigInitializationData initializationData) {
			sun = initializationData.sun;
			earth = initializationData.earth;
			
			canvas.SetParent(transform);
		}


		public void SetCamera(CameraPivot pivot, Action midAnimationCallback = null, Action endAnimationCallback = null) {

			if (currentCameraPivot == pivot) {
				return;
			}
			
			switch (pivot) {
				case CameraPivot.Sun:
					SetCameraPivot(sunPivot, midAnimationCallback, endAnimationCallback);
					break;
				case CameraPivot.Earth:
					SetCameraPivot(earth.transform, midAnimationCallback, endAnimationCallback);
					break;
				case CameraPivot.Moon:
					break;
				case CameraPivot.Venus:
					break;
				case CameraPivot.Mars:
					break;
				case CameraPivot.Unassigned:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(pivot), pivot, null);
			}

			currentCameraPivot = pivot;
		}
		
		private void SetCameraPivot(Transform pivot, Action action = null, Action endAction = null) {

			var isSunPivot = pivot == sunPivot;
			
			fadeToBlack.Transition(() => {
				action?.Invoke();
				
				if (isSunPivot) {
					SetSunPivot();
				}
				else {
					SetPlanetPivot(pivot);
				}
			}, endAction);
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
				distance = 1.0f,
			});
			
			transform.SetParent(cameraFollower.CameraPivot, false);
			transform.LookAt(pivot);
			sun.GetComponent<Renderer>().enabled = true;
		}
		
	}


	public enum CameraPivot {
		Sun,
		Earth,
		Moon,
		Venus,
		Mars,
		Unassigned
	}

	public struct CameraRigInitializationData {
		public GameObject sun;
		public CelestialBody earth;
	}
}