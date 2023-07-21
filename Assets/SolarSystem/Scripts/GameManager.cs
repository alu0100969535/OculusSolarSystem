using System;
using UnityEngine;

namespace SolarSystem {
	[ExecuteAlways]
	public class GameManager : MonoBehaviour {

		[SerializeField] private GameParameters gameParameters;

		[Header("Stars")]
		[SerializeField] private GameObject sun;
		
		[SerializeField] private Planet venus;
		[SerializeField] private Planet earth;
		[SerializeField] private Planet mars;
		
		[SerializeField] private Planet moon;

		[Header("References")]
		[SerializeField] private CameraRig cameraRig;
		[SerializeField] private CurrentTimeHelper timeHelper;
		[SerializeField] private GameObject quizCameraVisor;
		[SerializeField] private ApplicationLifecycle applicationLifecycle;
		[SerializeField] private GameObject canvas;
 
		private Planet[] planets;
		private Planet[] bodies;

		private void Awake() {
			planets = new[] { venus, earth, mars };
			bodies = new [] { venus, earth, mars, moon};
		}
		
		public void Initialize() {
			InitializeAllStars(0.25f);
			
			timeHelper.Initialize(gameParameters.DayDurationInSeconds);
			cameraRig.Initialize(new CameraRigInitializationData {
				sun = sun,
				earth = earth
			});
		}

		public void InitializeSimulation() {
			Enable();
			SetSpeedAllStars(gameParameters.YearDurationInSeconds, gameParameters.DayDurationInSeconds);
			StartMovementAllStars();
		}

		public void Disable() {
			foreach (var body in bodies) {
				body.gameObject.SetActive(false);
			}
			
			sun.SetActive(false);
			timeHelper.gameObject.SetActive(false);
		}
		
		public void Enable() {
			foreach (var body in bodies) {
				body.gameObject.SetActive(true);
			}
			
			sun.SetActive(true);
			timeHelper.gameObject.SetActive(true);
		}

		public void StartQuiz() {
			
			// TODO: Make quiz 
			
			earth.ShowRandomPoint();
			
			quizCameraVisor.SetActive(true);
			
			var newPosition = cameraRig.CenterEyeTransform.position;
			newPosition += cameraRig.CenterEyeTransform.forward * 0.5f;
			
			quizCameraVisor.transform.position = newPosition;
			quizCameraVisor.transform.LookAt(cameraRig.CenterEyeTransform);
		}

		#region EventHandlers

		public void ExitMode() {
			cameraRig.SetCamera(CameraPivot.Unassigned, () => {
				canvas.SetActive(false);
			}, () => {
				applicationLifecycle.ShowMainMenu();
			});
		}

		public void SetSunCamera(bool value) {
			if (!value) {
				return;
			}
			
			cameraRig.SetCamera(CameraPivot.Sun);
		}

		public void SetEarthCamera(bool value) {
			if (!value) {
				return;
			}

			cameraRig.SetCamera(CameraPivot.Earth);
		}

		public void SetSpeed(Single value) {
			var dayDurationInSeconds = value / gameParameters.DaysInYear;
			SetSpeedAllStars(value, dayDurationInSeconds);
			timeHelper.SetSpeed(dayDurationInSeconds);
		}
		
		public void SetGizmos(bool value) {
			foreach (var body in bodies) {
				body.GizmosEnabled = value;
			}
		}
		
		public void SetWinterSeason() {
			SetSeason(Planet.Season.Winter);
		}

		public void SetSpringSeason() {
			SetSeason(Planet.Season.Spring);
		}

		public void SetSummerSeason() {
			SetSeason(Planet.Season.Summer);
		}

		public void SetAutumnSeason() {
			SetSeason(Planet.Season.Autumn);
		}

		public void Pause() {
			foreach (var body in bodies) {
				body.PauseMovement();
			}
			
			timeHelper.Pause();
		}

		#endregion

		private void SetSeason(Planet.Season season) {
			/*foreach (var planet in planets) {
				planet.PauseMovement();
				planet.MoveToSeason(season);
			}
			*/
			earth.PauseMovement();
			earth.MoveToSeason(season);
			timeHelper.SetSeason(season);
			earth.ResumeMovement();
			/*foreach (var planet in planets) {
				planet.ResumeMovement();
			}*/
		}

		private void InitializeAllStars(float scale) {
			foreach (var planet in planets) {
				planet.Initialize(new Planet.InitializationParameters {
					sun = sun,
					planetScale = scale
				});
			}
			
			moon.Initialize(new Planet.InitializationParameters {
				sun = earth.gameObject,
				planetScale = scale
			});
		}

		private void SetSpeedAllStars(float yearDurationInSeconds, float dayDurationInSeconds) {
			foreach (var body in bodies) {
				body.SetParameters(yearDurationInSeconds, dayDurationInSeconds);
			}
		}

		private void StartMovementAllStars() {
			foreach (var body in bodies) {
				body.StartMovement();
			}
		}
		
	}
}