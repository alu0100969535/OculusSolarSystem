using System;
using UnityEngine;

namespace SolarSystem {
	public class FollowPlanet : MonoBehaviour {

		[SerializeField] GameObject target;
		
		private Vector3 originalTargetPosition;
		private Vector3 originalPosition;

		private void Start() {
			originalTargetPosition = target.transform.position;
			originalPosition = transform.position;
		}

		private void LateUpdate() {
			var newPosition = target.transform.position - originalTargetPosition ;
			transform.position = originalPosition + newPosition;
		}
	}
}