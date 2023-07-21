using System.Collections.Generic;
using UnityEngine;

namespace SolarSystem.Scripts.Quiz {
	[CreateAssetMenu(fileName = "QuizSetup", menuName = "QuizSetup", order = 0)]
	public class QuizSetup : ScriptableObject {

		private Question[] questions;

		public List<Question> GetQuestions() {
			return new List<Question>(questions);
		}

	}
}