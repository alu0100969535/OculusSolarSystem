using System;
using UnityEngine;
using UnityEngine.UI;

namespace SolarSystem {
	public class CurrentTimeHelper : MonoBehaviour {

		[SerializeField] private Text text;
		[SerializeField] private int startingDateYear;
		[SerializeField] private int startingDateMonth;
		[SerializeField] private int startingDateDay;
		
		private float dayInSeconds;

		private float currentTime;
		private DateTime startingDate;


		public void Initialize(float dayInSeconds) {
			this.dayInSeconds = dayInSeconds;

			startingDate = new DateTime(startingDateYear, startingDateMonth, startingDateDay);
		}

		public void SetCurrentExecutionTime(float time) {
			currentTime = time;
		}

		private void Update() {
			var date = GetCurrentDate();
			RenderDate(date);
		}

		private DateTime GetCurrentDate() {
			var elapsedTimeInSeconds = (int) (currentTime / dayInSeconds * 24 * 60 * 60);
			return startingDate + new TimeSpan(0, 0, elapsedTimeInSeconds);
		}

		private void RenderDate(DateTime date) {
			text.text = date.ToString("d MMM yyyy");
			Debug.Log(text.text);
		}
	}
}