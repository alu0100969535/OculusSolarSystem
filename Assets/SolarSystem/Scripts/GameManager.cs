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

		private Planet[] planets;
		private float executionTime;
		private float currentYearDuration;
		
		private void Awake() {
			planets = new[] { venus, earth, mars };

			InitializeAllStars(gameParameters.YearDurationInSeconds, gameParameters.DayDurationInSeconds);
			
			timeHelper.Initialize(gameParameters.DayDurationInSeconds);
		}

		public void Start() {
			if (!Application.IsPlaying(gameObject)) {
				return;
			}

			StartMovementAllStars();
		}

		private void Update() {
			timeHelper.SetCurrentExecutionTime(executionTime);
			executionTime += Time.deltaTime;
		}

		#region EventHandlers

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
			InitializeAllStars(value, value / gameParameters.DaysInYear);
			ConvertExecutionTime(value);
		}
		
		public void SetGizmos(bool value) {
			foreach (var planet in planets) {
				planet.GizmosEnabled = value;
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

		#endregion

		private void SetSeason(Planet.Season season) {
			foreach (var planet in planets) {
				planet.PauseMovement();
				planet.MoveToSeason(season);
			}
		}
		
		private void ConvertExecutionTime(float newYearDuration) {
			var ratio = currentYearDuration / newYearDuration;

			executionTime *= ratio;

			currentYearDuration = newYearDuration;
			
			timeHelper.Initialize(currentYearDuration / gameParameters.DaysInYear);
			timeHelper.SetCurrentExecutionTime(executionTime);
		}

		private void InitializeAllStars(float yearDuration, float dayDuration) {
			foreach (var planet in planets) {
				planet.Initialize(new Planet.InitializationParameters {
					sun = sun,
					yearDurationInSeconds = yearDuration,
					dayDurationInSeconds = dayDuration
				});
			}
			
			moon.Initialize(new Planet.InitializationParameters {
				sun = earth.gameObject,
				yearDurationInSeconds = yearDuration,
				dayDurationInSeconds = dayDuration
			});
		}

		private void StartMovementAllStars() {
			foreach (var planet in planets) {
				planet.StartMovement();
			}
			
			moon.StartMovement();

		}




	}
}