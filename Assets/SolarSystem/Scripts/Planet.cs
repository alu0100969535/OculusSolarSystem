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

    [SerializeField] private LineRenderer lineRenderer;

    public bool GizmosEnabled {
        get => gizmosEnabled;
        set {

            if (value) {
                lineRenderer.loop = false;
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
            }
                
            lineRenderer.enabled = value;
            gizmosEnabled = value;
        }
    }

    private GameObject sun;

    private Vector3 ownRotationAxis;
    private float ownRotationAngle;

    private float orbitAngle;

    private bool isRunning;

    private Vector3[] debugOrbitPoints;
    private bool gizmosEnabled = false;

    public void Initialize(InitializationParameters initializationParameters) {
        this.sun = initializationParameters.sun;
        ComputeValues(initializationParameters);
        SetInitialTransform();
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

    private void Update() {
        if (!isRunning) {
            return;
        }
        
        Orbit();
        Rotate();

        if (GizmosEnabled) {
            DrawLineRendererGizmos();
        }
    }

    private void SetInitialTransform() {
        transform.position = (transform.position - sun.transform.position).normalized * distanceFromSun + sun.transform.position;
        
        var rotation = Vector3.zero;
        rotation.x = inclination;
        transform.eulerAngles = rotation;
    }

    private void ComputeValues(InitializationParameters initializationParameters) {
        orbitAngle =  1 / period * 360 / initializationParameters.yearDurationInSeconds;
        
        var angle = inclination * Math.PI/180;
        
        ownRotationAxis = new Vector3(0.0f, (float) Math.Cos(angle), (float) Math.Sin(angle));
        ownRotationAngle = 1 / ownRotationPeriod * 360 / initializationParameters.dayDurationInSeconds;

        debugOrbitPoints = new Vector3[360];
        for (var i = 0; i < 360; i++) {
            var angle2 = i * Math.PI/180;
            debugOrbitPoints[i] = new Vector3(distanceFromSun * (float) Math.Cos(angle2), 0, distanceFromSun * (float) Math.Sin(angle2));
        }
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

        if (debugOrbitPoints == null) {
            return;
        }

        Gizmos.color = Color.white;

        for (var i = 0; i < debugOrbitPoints.Length; i++) {
            var nextPointIndex = i == debugOrbitPoints.Length - 1 ? 0 : i + 1; 
            
            var point = debugOrbitPoints[i];
            var point2 = debugOrbitPoints[nextPointIndex];
            Gizmos.DrawLine(point, point2);
        }
    }

    private void DrawLineRendererGizmos() {
        var point1 = transform.position + ownRotationAxis;
        var point2 = transform.position - ownRotationAxis;


        lineRenderer.gameObject.SetActive(true);
        lineRenderer.SetPositions(new []{point1, point2});
    }

    public struct InitializationParameters {
        public GameObject sun;
        public float yearDurationInSeconds;
        public float dayDurationInSeconds;
    }
}
