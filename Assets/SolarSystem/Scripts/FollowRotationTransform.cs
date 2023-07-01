using System;
using UnityEngine;

namespace SolarSystem {
	public class FollowRotationTransform : MonoBehaviour {

		[SerializeField] private Transform target;

		private void Start() {
			transform.eulerAngles = target.eulerAngles;
		}

		private void LateUpdate() {
			transform.eulerAngles = target.eulerAngles;
		}
	}
}