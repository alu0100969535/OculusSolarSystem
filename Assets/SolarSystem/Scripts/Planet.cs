using System;
using System.Collections;
using System.Collections.Generic;
using SolarSystem;
using UnityEngine;

public class Planet : MonoBehaviour {
    
    [Header("Orbit")]
    [SerializeField] private float distanceFromSun;
    [SerializeField] [Tooltip("In years relative to Earth")] private float period;
    
    [Header("Rotation")]
    [SerializeField] [Tooltip("In degrees relative to Sun's Equator")] private float inclination;
    [SerializeField] private float ownRotationPeriod;

    private GameObject sun;

    private Vector3 ownRotationAxis;
    private float ownRotationAngle;

    private float orbitAngle;

    private bool isRunning;
    
    public void Initialize(InitializationParameters initializationParameters) {
        this.sun = initializationParameters.sun;
        ComputeValues(initializationParameters);
    }

    public void PauseMovement() {
        isRunning = false;
    }

    public void ResumeMovement() {
        isRunning = true;
    }

    public void StartMovement() {
        isRunning = true;
    }

    private void Start() {
        SetInitialDistance();
    }
    
    private void Update() {
        if (!isRunning) {
            return;
        }
        
        Orbit();
        Rotate();
    }

    private void SetInitialDistance() {
        transform.position = (transform.position - sun.transform.position).normalized * distanceFromSun + sun.transform.position;
    }

    private void ComputeValues(InitializationParameters initializationParameters) {
        orbitAngle =  1 / period * 360 / initializationParameters.yearDurationInSeconds;
        
        ownRotationAxis = new Vector3(0.0f, (float) Math.Sin(inclination), (float) Math.Cos(inclination));
        ownRotationAngle = 1 / ownRotationPeriod * 360 / initializationParameters.dayDurationInSeconds;
    }

    private void Orbit() {
        transform.RotateAround(sun.transform.position, Vector3.up, orbitAngle * Time.deltaTime);
    }

    private void Rotate() {
        transform.Rotate(ownRotationAxis, ownRotationAngle * Time.deltaTime, Space.World);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        
        Gizmos.DrawLine(
            transform.position + ownRotationAxis,
            transform.position - ownRotationAxis
        );
    }

    public struct InitializationParameters {
        public GameObject sun;
        public float yearDurationInSeconds;
        public float dayDurationInSeconds;
    }
}
