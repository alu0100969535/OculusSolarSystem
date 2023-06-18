using UnityEngine;

namespace SolarSystem {
	
	[CreateAssetMenu(fileName = "GameParameters", menuName = "ScriptableObjects/GameParameters", order = 1)]
	public class GameParameters : ScriptableObject {
		
		public float YearDurationInSeconds => yearDurationInSeconds;
		public float DayDurationInSeconds => yearDurationInSeconds / DAYS_IN_YEAR;
		public float DaysInYear => DAYS_IN_YEAR;
		
		[SerializeField] private float yearDurationInSeconds = 600;
		
		private const float DAYS_IN_YEAR = 365.25f;
		
	}
}