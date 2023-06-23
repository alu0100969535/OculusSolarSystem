using System;
using System.Collections;
using UnityEngine;

namespace SolarSystem {
	public class CameraFadeToBlack : MonoBehaviour {

		[SerializeField] private OVRScreenFade screenFade;
		[SerializeField] private float duration;

		public void Transition(Action midTransitionCallback) {
			StartCoroutine(MakeTransition(midTransitionCallback));
		}

		private IEnumerator MakeTransition(Action callback) {
			
			yield return Fade(0f, 1f, duration / 2);

			callback();

			yield return Fade(1f, 0f, duration / 2);

		}

		private IEnumerator Fade(float start, float end, float duration) {

			var value = start;

			var time = 0.0f;
			
			while (time < duration) {

				value = Mathf.Lerp(start, end, time / duration);
				screenFade.SetUIFade(value);
				
				yield return null;
				time += Time.deltaTime;
			}
		}
		
	}
}