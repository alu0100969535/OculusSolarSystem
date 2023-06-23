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
    
}
