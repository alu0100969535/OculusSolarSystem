using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlTest : MonoBehaviour {

    [SerializeField] private OVRHand leftHand;
    [SerializeField] private OVRHand rightHand;

    [SerializeField] private GameObject ui;
    [SerializeField] private Transform uiPivot;
    [SerializeField] private float timeThreshold;
    
    private Color originalColor;
    private float timeSinceStartButtonAction;
    private bool hasShownUIOnce;

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
        if (OVRInput.Get(OVRInput.Button.Start) && timeSinceStartButtonAction >= timeThreshold) {

            if (!hasShownUIOnce) {
                ui.transform.position = uiPivot.position;
                hasShownUIOnce = true;
            }
            
            ui.SetActive(!ui.activeInHierarchy);
            timeSinceStartButtonAction = 0.0f;
        }

        timeSinceStartButtonAction += Time.deltaTime;
    }
    
}
