using System.Collections.Generic;
using SolarSystem.Quiz;
using SolarSystem.Scripts.Quiz;
using UnityEngine;

namespace SolarSystem {
	public class QuizSystem : MonoBehaviour {

		[SerializeField] private QuizSetup quizSetup;
		[SerializeField] private QuizCanvas quizCanvas;

		[SerializeField] private QuizResolver quizResolver;

		private Question currentQuestion;

		public void Initialize() {
			quizCanvas.HideNoWait();
		}

		public void ShowQuestion() {
			var question = GetRandomQuestion();
			quizCanvas.ShowQuestion(question.Text);
			currentQuestion = question;
		}

		public void CheckQuestionIsResolved() {
			var pass =
				quizResolver.Resolve(new QuizResolver.QuizResolverData {
					rotationAxisAngle = currentQuestion.rotationAxisAngle
				});

			if (pass) {
				// Continue with next question?
			}
		}

		private Question GetRandomQuestion() {
			var questions = quizSetup.GetQuestions();
			var index = Random.Range(0, questions.Count);

			return questions[index];
		}
		
	}
}