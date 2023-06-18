using UnityEngine;
using UnityEngine.UI;

namespace SolarSystem {
	
	public class VisualDebugLog : MonoBehaviour {

		[SerializeField] private OVRHand leftHand;
		[SerializeField] private OVRHand rightHand;
		
		[SerializeField] private Text outputPinching;
		[SerializeField] private Text outputPoseValid;
		[SerializeField] private Text outputPosePosition;

		void Update() {
			RegisterHandIsPinching();
		}

		void RegisterHandIsPinching() {
			var isPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
			outputPinching.text = $"Right hand is pinching: {isPinching}";

			var isPoseValid = rightHand.IsPointerPoseValid;
			outputPoseValid.text = $"Right hand is poseValid: {isPoseValid}";
			
			var strength = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
			outputPosePosition.text = $"Right hand pinching strength: {strength}";
		}
		
	}
}