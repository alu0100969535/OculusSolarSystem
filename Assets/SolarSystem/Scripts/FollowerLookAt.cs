using System;
using UnityEngine;

namespace SolarSystem {
	public class FollowerLookAt : MonoBehaviour {
		[SerializeField] private Transform pivot;
		[SerializeField] private Transform lookAt;

		private void Start() {
			UpdateTransform();
		}

		private void Update() {
			UpdateTransform();
		}

		void UpdateTransform() {
			transform.position = pivot.transform.position;
			transform.LookAt(lookAt);
		}
	}
}