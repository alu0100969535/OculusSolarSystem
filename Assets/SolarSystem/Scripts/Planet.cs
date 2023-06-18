using System;
using System.Collections;
using System.Collections.Generic;
using SolarSystem;
using UnityEngine;

public class Planet : MonoBehaviour {

    [SerializeField] private GameObject sun;
    
    [Header("Orbit")]
    [SerializeField] private float distanceFromSun;
    [SerializeField] [Tooltip("In years relative to Earth")] private float period;
    
    [Header("Rotation")]
    [SerializeField] [Tooltip("In degrees relative to Sun's Equator")] private float inclination;
    [SerializeField] private float ownRotationPeriod;


    private Vector3 ownRotationAxis;
    private float ownRotationAngle;

    private float orbitAngle;

    private void Awake() {
        ComputeValues();
    }

    private void Start() {
        SetInitialDistance();
    }
    
    private void Update() {
        if(sun != null) {
            Orbit();
        }

        Rotate();
    }

    private void SetInitialDistance() {
        transform.position = (transform.position - sun.transform.position).normalized * distanceFromSun + sun.transform.position;
    }

    private void ComputeValues() {
        orbitAngle =  1 / period * 360 / Constants.YEAR_DURATION_IN_SECONDS;
        
        ownRotationAxis = new Vector3(0.0f, (float) Math.Sin(inclination), (float) Math.Cos(inclination));
        ownRotationAngle = 1 / ownRotationPeriod * 360 / Constants.DAY_DURATION_IN_SECONDS;
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
}
