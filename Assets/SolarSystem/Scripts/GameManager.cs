using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SolarSystem {
	public class GameManager : MonoBehaviour {

		[SerializeField] private GameParameters gameParameters;

		[SerializeField] private GameObject sun;
		[SerializeField] private Planet[] planets;

		private void Awake() {
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
	}
}