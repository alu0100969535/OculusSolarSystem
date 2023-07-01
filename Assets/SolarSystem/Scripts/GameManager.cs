﻿using System;
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

		private void Awake() {
			planets = new[] { venus, earth, mars };

			InitializeAllStars(1.0f);
			SetSpeedAllStars(gameParameters.YearDurationInSeconds, gameParameters.DayDurationInSeconds);
			
			timeHelper.Initialize(gameParameters.DayDurationInSeconds);
			cameraRig.Initialize(new CameraRigInitializationData {
				sun = sun,
				earth = earth
			});
		}

		public void Start() {
			if (!Application.IsPlaying(gameObject)) {
				return;
			}

			InitializeAllStars(0.25f);
			StartMovementAllStars();
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
			var dayDurationInSeconds = value / gameParameters.DaysInYear;
			SetSpeedAllStars(value, dayDurationInSeconds);
			timeHelper.SetSpeed(dayDurationInSeconds);
		}
		
		public void SetGizmos(bool value) {
			foreach (var planet in planets) {
				planet.GizmosEnabled = value;
			}

			moon.GizmosEnabled = value;
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
			foreach (var planet in planets) {
				planet.PauseMovement();
			}
			
			moon.PauseMovement();
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
			foreach (var planet in planets) {
				planet.SetParameters(yearDurationInSeconds, dayDurationInSeconds);
			}
			
			moon.SetParameters(yearDurationInSeconds, dayDurationInSeconds);
		}

		private void StartMovementAllStars() {
			foreach (var planet in planets) {
				planet.StartMovement();
			}
			
			moon.StartMovement();
		}




	}
}