using System;
using UnityEngine;

namespace SolarSystem {
	[ExecuteAlways]
	public class GameManager : MonoBehaviour {

		[SerializeField] private GameParameters gameParameters;

		[SerializeField] private GameObject sun;
		[SerializeField] private Planet[] planets;
		[SerializeField] private Planet moon;

		[SerializeField] private CurrentTimeHelper timeHelper;

		[SerializeField] private GameObject cameraRig;
		
		[SerializeField] private Transform sunPivot;
		[SerializeField] private Transform earthPivot;

		private CameraFadeToBlack cameraFadeToBlack;
		private Planet earth;
		private float executionTime;
		private float currentYearDuration;
		
		private void Awake() {

			cameraFadeToBlack = cameraRig.GetComponent<CameraFadeToBlack>();
			earth = planets[0]; // TODO: get earth
			
			foreach (var planet in planets) {
				planet.Initialize(new Planet.InitializationParameters {
					sun = sun,
					yearDurationInSeconds = gameParameters.YearDurationInSeconds,
					dayDurationInSeconds = gameParameters.DayDurationInSeconds
				});
			}
			
			moon.Initialize(new Planet.InitializationParameters {
				sun = earth.gameObject,
				yearDurationInSeconds = gameParameters.YearDurationInSeconds,
				dayDurationInSeconds = gameParameters.DayDurationInSeconds
			});
			
			timeHelper.Initialize(gameParameters.DayDurationInSeconds);
		}

		public void Start() {
			if (!Application.IsPlaying(gameObject)) {
				return;
			}
			
			foreach (var planet in planets) {
				planet.StartMovement();
			}
			
			moon.StartMovement();

			if (cameraRig.activeInHierarchy) {
				cameraRig.transform.SetParent(sunPivot, false);
				sun.GetComponent<Renderer>().enabled = false;
			}
		}

		private void Update() {
			timeHelper.SetCurrentExecutionTime(executionTime);
			executionTime += Time.deltaTime;
		}

		public void SetSunCamera(bool value) {
			if (!value) {
				return;
			}
			
			cameraFadeToBlack.Transition(() => {
				cameraRig.transform.SetParent(sunPivot, false);
				sun.GetComponent<Renderer>().enabled = false;
			});
		}

		public void SetEarthCamera(bool value) {
			if (!value) {
				return;
			}
			
			cameraFadeToBlack.Transition(() => {
				cameraRig.transform.SetParent(earthPivot, false);
				sun.GetComponent<Renderer>().enabled = true;
			});
		}

		public void SetSpeed(Single value) {
			foreach (var planet in planets) {
				planet.Initialize(new Planet.InitializationParameters {
					sun = sun,
					yearDurationInSeconds = value,
					dayDurationInSeconds = value / gameParameters.DaysInYear
				});
			}

			ConvertExecutionTime(value);
		}

		private void ConvertExecutionTime(float newYearDuration) {
			var ratio = currentYearDuration / newYearDuration;

			executionTime *= ratio;

			currentYearDuration = newYearDuration;
		}

		public void SetGizmos(bool value) {
			foreach (var planet in planets) {
				planet.GizmosEnabled = value;
			}
		}

		public void SetSeason(Planet.Season season) {
			foreach (var planet in planets) {
				planet.PauseMovement();
				planet.MoveToSeason(season);
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
	}
}