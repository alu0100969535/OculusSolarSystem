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
		[SerializeField] private Transform originalCameraPivot;

		private GameObject sun;
		private Planet earth;
		private CameraPivot currentCameraPivot = CameraPivot.Unassigned;

		public Transform CenterEyeTransform => centerEyeTransform;

		public void Initialize(CameraRigInitializationData initializationData) {
			sun = initializationData.sun;
			earth = initializationData.earth;
			
			canvas.SetParent(transform);
		}


		public void SetCamera(CameraPivot pivot, Action midAnimationCallbackBeforeChange = null, Action midAnimationCallbackAfterChange = null,  Action endAnimationCallback = null) {

			if (currentCameraPivot == pivot) {
				return;
			}
			
			switch (pivot) {
				case CameraPivot.Sun:
					SetCameraPivot(sunPivot, midAnimationCallbackBeforeChange, midAnimationCallbackAfterChange, endAnimationCallback);
					break;
				case CameraPivot.Earth:
					SetCameraPivot(earth.transform, midAnimationCallbackBeforeChange, midAnimationCallbackAfterChange, endAnimationCallback);
					break;
				case CameraPivot.Moon:
					break;
				case CameraPivot.Venus:
					break;
				case CameraPivot.Mars:
					break;
				case CameraPivot.Unassigned:
					SetCameraPivot(originalCameraPivot, midAnimationCallbackBeforeChange, midAnimationCallbackAfterChange, endAnimationCallback);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(pivot), pivot, null);
			}

			currentCameraPivot = pivot;
		}
		
		private void SetCameraPivot(Transform pivot, Action actionBefore = null, Action actionAfter = null, Action endAction = null) {

			var isPlanetPivot = pivot != sunPivot && pivot != originalCameraPivot;
			
			fadeToBlack.Transition(() => {
				actionBefore?.Invoke();
				if (!isPlanetPivot) {
					SetDirectPivot(pivot);
				}
				else {
					SetPlanetPivot(pivot);
				}
				actionAfter?.Invoke();
			}, endAction);
		}
		
		private void SetDirectPivot(Transform pivot){
			transform.SetParent(pivot, false);
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
			//transform.LookAt(pivot);
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
		public Planet earth;
	}
}