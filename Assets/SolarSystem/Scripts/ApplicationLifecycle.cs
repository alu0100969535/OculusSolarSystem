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

		public void StartSandboxMode() {
			gameManager.Initialize();
			
			cameraRig.SetCamera(CameraPivot.Sun, () => {
				mainMenuCanvas.SetActive(false);
				gameManager.InitializeSimulation();
			});
		}

		public void StartQuizMode() {
			gameManager.Initialize();
			
			cameraRig.SetCamera(CameraPivot.Earth, () => {
				mainMenuCanvas.SetActive(false);
				gameManager.InitializeSimulation();
				gameManager.StartQuiz();
			});
		}

		private void ShowMainMenu() {
			gameManager.Disable();
			mainMenuCanvas.SetActive(true);

			OVRManager.HMDMounted += HandleHMDMounted;
			StartCoroutine(MoveUIAnimation());
		}
		
		private void HandleHMDMounted() {
			StartCoroutine(MoveUIAnimation());
		}

		private IEnumerator MoveUIAnimation() {
			
			var targetPosition = cameraRig.CenterEyeTransform.position + cameraRig.CenterEyeTransform.forward * 1;
			var initialPosition = mainMenuCanvas.transform.position;

			var initialYEulerAngles = mainMenuCanvas.transform.eulerAngles.y;
			var targetYEulerAngles = cameraRig.CenterEyeTransform.eulerAngles.y;
			
			var duration = 1f;
			var time = 0f;

			while (time < duration) {

				var lerpIndex = mainMenuAnimationCurve.Evaluate(time / duration);
				
				var newPosition = Vector3.Lerp(initialPosition, targetPosition, lerpIndex);
				mainMenuCanvas.transform.position = newPosition;

				var newYEulerAngles = Mathf.Lerp(initialYEulerAngles, targetYEulerAngles, lerpIndex);
				var newEulerAngles = mainMenuCanvas.transform.eulerAngles;
				newEulerAngles.y = newYEulerAngles;
				mainMenuCanvas.transform.eulerAngles = newEulerAngles;
				
				yield return null;
				time += Time.deltaTime;
			}
		}
		
	}
}