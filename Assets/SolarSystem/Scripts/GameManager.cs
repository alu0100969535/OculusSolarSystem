using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SolarSystem {
	public class GameManager : MonoBehaviour {

		[SerializeField] private GameParameters gameParameters;

		[SerializeField] private GameObject sun;
		[SerializeField] private Planet[] planets;

		[SerializeField] private GameObject cameraRig;
		
		[SerializeField] private Transform sunPivot;
		[SerializeField] private Transform earthPivot;

		private CameraFadeToBlack cameraFadeToBlack;
		
		private void Awake() {

			cameraFadeToBlack = cameraRig.GetComponent<CameraFadeToBlack>();
			
			foreach (var planet in planets) {
				planet.Initialize(new Planet.InitializationParameters {
					sun = sun,
					yearDurationInSeconds = gameParameters.YearDurationInSeconds,
					dayDurationInSeconds = gameParameters.DayDurationInSeconds
				});
			}
		}

		public void Start() {
			foreach (var planet in planets) {
				planet.StartMovement();
			}
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
					dayDurationInSeconds = value / 365.25f
				});
			}
		}

		public void SetGizmos(bool value) {
			foreach (var planet in planets) {
				planet.GizmosEnabled = value;
			}
		}
	}
}