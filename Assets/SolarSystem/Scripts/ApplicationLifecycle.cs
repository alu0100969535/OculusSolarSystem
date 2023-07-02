using System.Collections;
using UnityEngine;

namespace SolarSystem {
	public class ApplicationLifecycle : MonoBehaviour {

		[SerializeField] private GameManager gameManager;
		[SerializeField] private CameraRig cameraRig;

		[Header("Menus")] 
		[SerializeField] private GameObject mainMenuCanvas;
		[SerializeField] private AnimationCurve mainMenuAnimationCurve;
		
		private void Start() {
			ShowMainMenu();
		}

		private void ShowMainMenu() {
			gameManager.Disable();
			mainMenuCanvas.SetActive(true);

			if (!OVRManager.isHmdPresent) {
				OVRManager.HMDMounted += HandleHMDMounted;
				return;
			}
			
			StartCoroutine(MoveUIAnimation());
		}

		public void StartSandboxMode() {
			cameraRig.SetCamera(CameraPivot.Sun, () => {
				mainMenuCanvas.SetActive(false);
				gameManager.Initialize();
			});
		}

		private void HandleHMDMounted() {
			OVRManager.HMDMounted -= HandleHMDMounted;
			StartCoroutine(MoveUIAnimation());
		}

		private IEnumerator MoveUIAnimation() {

			var targetYPosition = cameraRig.CenterEyeTransform.position.y;
			var initialPosition = mainMenuCanvas.transform.position;
			var duration = 1f;
			var time = 0f;

			while (time < duration) {

				var lerpIndex = mainMenuAnimationCurve.Evaluate(time / duration);
				var newY = Mathf.Lerp(initialPosition.y, targetYPosition, lerpIndex);

				var newPosition = new Vector3(initialPosition.x, newY, initialPosition.z);
				mainMenuCanvas.transform.position = newPosition;
				
				yield return null;
				time += Time.deltaTime;
			}

		}
		
	}
}