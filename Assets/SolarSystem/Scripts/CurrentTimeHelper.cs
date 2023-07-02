using System;
using UnityEngine;
using UnityEngine.UI;

namespace SolarSystem {
	public class CurrentTimeHelper : MonoBehaviour {

		[SerializeField] private Text text;
		[SerializeField] private UDateTime winterTime;
		[SerializeField] private UDateTime springTime;
		[SerializeField] private UDateTime summerTime;
		[SerializeField] private UDateTime autumnTime;
		
		private float dayInSeconds;
		
		private DateTime startingDate;
		private float executionTime;
		private bool isPaused;


		private void Awake() {
			startingDate = winterTime.dateTime;
		}
		
		private void Update() {
			if (isPaused) {
				return;
			}
			
			UpdateExecutionTime();
			var date = GetCurrentDate();
			RenderDate(date);
		}

		public void Pause() {
			isPaused = true;
		}

		public void Resume() {
			isPaused = false;
		}

		public void Initialize(float dayInSeconds) {
			this.dayInSeconds = dayInSeconds;
		}

		public void SetSpeed(float dayInSeconds) {
			ConvertExecutionTime(dayInSeconds);
			this.dayInSeconds = dayInSeconds;
		}

		public void SetSeason(CelestialBody.Season season) {
			executionTime = season switch {
				CelestialBody.Season.Winter => 0,
				CelestialBody.Season.Spring => GetExecutionTimeForTargetDate(springTime.dateTime),
				CelestialBody.Season.Summer => GetExecutionTimeForTargetDate(summerTime.dateTime),
				CelestialBody.Season.Autumn => GetExecutionTimeForTargetDate(autumnTime.dateTime),
				_ => throw new ArgumentOutOfRangeException(nameof(season), season, null)
			};
		}

		private float GetExecutionTimeForTargetDate(DateTime targetTime) {
			var diff = (targetTime - startingDate).TotalDays * dayInSeconds;
			return (float) diff;
		}

		private DateTime GetCurrentDate() {
			var elapsedTimeInSeconds = (int) (executionTime / dayInSeconds * 24 * 60 * 60);
			return startingDate + new TimeSpan(0, 0, elapsedTimeInSeconds);
		}

		private void RenderDate(DateTime date) {
			text.text = date.ToString("d MMM yyyy");
		}
		
		private void ConvertExecutionTime(float newDayDuration) {
			var ratio = dayInSeconds / newDayDuration;

			executionTime *= ratio;
		}

		private void UpdateExecutionTime() {
			executionTime += Time.deltaTime;
		}
	}
}