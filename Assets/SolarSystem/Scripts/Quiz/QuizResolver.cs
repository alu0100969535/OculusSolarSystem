using UnityEngine;

namespace SolarSystem {
	public class QuizResolver : MonoBehaviour {

		[SerializeField] private Transform invariableTransform;
		[SerializeField] private Transform runtimeAxisAngleTransform;
		
		public bool Resolve(QuizResolverData data) {
			var current = invariableTransform.eulerAngles + runtimeAxisAngleTransform.eulerAngles;
			var reference = data.rotationAxisAngle;

			return IsSimilar(current.y, reference, 0.5f);
		}

		private bool IsSimilar(float a, float b, float slack) {
			return a - b < slack && a - b > -slack;
		}
		
		public struct QuizResolverData {
			public int rotationAxisAngle;
		}
		
	}


}