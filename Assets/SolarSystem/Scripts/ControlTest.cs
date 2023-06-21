using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlTest : MonoBehaviour {

    [SerializeField] private OVRHand leftHand;
    [SerializeField] private OVRHand rightHand;

    [SerializeField] private GameObject ui;
    
    private Color originalColor;

    void Start() {
        originalColor = rightHand.gameObject.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update() {
        SetSystemGestureColor();
        CheckStartButton();
        //CheckPointerGesture();
    }

    private void SetSystemGestureColor() {
        if (rightHand.IsSystemGestureInProgress) {
            rightHand.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else {
            rightHand.gameObject.GetComponent<Renderer>().material.color = originalColor;
        }
    }

    private void CheckStartButton() {
        if (OVRInput.Get(OVRInput.Button.Start)) {
            ui.SetActive(!ui.activeInHierarchy);
        }
    }

    private void CheckPointerGesture() {

        if (rightHand.IsPointerPoseValid) {
            var strength = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
            
            var pose = rightHand.PointerPose;
            
            var origin = pose.position;
            var dir = transform.TransformDirection(pose.forward);

            if (rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index)) {
                if (Physics.Raycast(origin, dir, out var hit)) {
                    Debug.DrawRay(origin, dir * hit.distance, Color.yellow);
                    Debug.Log("Did Hit");
                }
                else {
                    Debug.DrawRay(origin, dir * 1000, Color.white);
                    Debug.Log("Did not Hit");
                }
            }
            
        }
    }
}
