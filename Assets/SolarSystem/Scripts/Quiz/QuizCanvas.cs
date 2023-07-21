using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SolarSystem.Quiz {
	public class QuizCanvas : MonoBehaviour {

		[SerializeField] private RectTransform canvas;
		[SerializeField] private RectTransform question;
		[SerializeField] private Text uiText;

		[Header("Question in animation")] 
		[SerializeField] private float questionInDuration;
		[SerializeField] private AnimationCurve questionInAnimationCurve;
		
		[Header("Question out animation")] 
		[SerializeField] private float questionOutDuration;
		[SerializeField] private AnimationCurve questionOutAnimationCurve;

		public void ShowQuestion(string text) {
			ResetQuestion();
			SetQuestionText(text);
			StartCoroutine(AnimateQuestionIn());
		}

		public void HideQuestion() {
			ResetQuestion();
			StartCoroutine(AnimateQuestionOut());
		}

		public void HideNoWait() {
			canvas.localScale = Vector3.zero;
		}

		private void SetQuestionText(string text) {
			uiText.text = text;
		}

		private IEnumerator AnimateQuestionIn() {

			var start = new Vector3(0, 50, 0);
			var end = Vector3.zero;

			var duration = questionInDuration;
			var time = 0.0f;

			while (time < duration) {

				var lerpIndex = questionInAnimationCurve.Evaluate(time / duration);
				var newPosition = Vector3.Lerp(start, end, lerpIndex);

				canvas.position = newPosition;
				
				yield return null;
				time += Time.deltaTime;
			}

		}

		private IEnumerator AnimateQuestionOut() {
			var start = canvas.localScale;
			var end = Vector3.zero;

			var duration = questionOutDuration;
			var time = 0.0f;

			while (time < duration) {

				var lerpIndex = questionOutAnimationCurve.Evaluate(time / duration);
				var newScale = Vector3.Lerp(start, end, lerpIndex);

				canvas.localScale = newScale;
				
				yield return null;
				time += Time.deltaTime;
			}
		}

		private void ResetQuestion() {
			canvas.localScale = Vector3.one;
			canvas.position = Vector3.zero;
		}
	}
}