using System;
using SolarSystem;
using UnityEditor;
using UnityEngine;

public class CelestialBody : MonoBehaviour {

    [SerializeField] private bool updateSunPositionOnOrbit;
    
    [Header("Orbit")]
    [SerializeField] private float distanceFromSun;
    [SerializeField] [Tooltip("In years relative to Earth")] private float period;
    [SerializeField] private Transform translationPivot;
    
    [Header("Rotation")]
    [SerializeField] [Tooltip("In degrees relative to Sun's Equator")] private float inclination;
    [SerializeField] private float ownRotationPeriod;
    [SerializeField] private Transform rotationPivot;

    [Header("Others")] 
    [SerializeField] private Transform invariableTransform;
    [SerializeField] private EarthPoints earthPoints;

    [Header("Gizmos")]
    [SerializeField] private GameObject rotationAxisGizmo;
    
    private Vector3 initialPosition; // this position is Winter (north hemisphere)    
    private bool transformInitialized;

    public bool GizmosEnabled {
        get => gizmosEnabled;
        set {
            gizmosEnabled = value;
            if (value) {
                EnableRuntimeGizmos();
            }
            else {
                DisableRuntimeGizmos();
            }
        }
    }

    private GameObject sun;
    
    private float ownRotationAngle;

    private float orbitAngle;

    private bool isRunning;

    private Vector3[] debugOrbitPoints;
    private bool gizmosEnabled = false;

    public void Initialize(InitializationParameters initializationParameters) {
        this.sun = initializationParameters.sun;

        var size = initializationParameters.planetScale;
        transform.localScale = new Vector3(size, size, size);
        
        ComputeSunDependantValues();
        SetInitialTransform();
    }

    public void SetParameters(float yearDurationInSeconds, float dayDurationInSeconds) {
        ComputeValues(yearDurationInSeconds, dayDurationInSeconds);
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

    public void MoveToSeason(Season season) {
        translationPivot.position = initialPosition;
        switch (season) {
            case Season.Spring:
                translationPivot.RotateAround(sun.transform.position, Vector3.up, 90);
                break;
            case Season.Summer:
                translationPivot.RotateAround(sun.transform.position, Vector3.up, 180);
                break;
            case Season.Autumn:
                translationPivot.RotateAround(sun.transform.position, Vector3.up, 270);
                break;
        }
        translationPivot.eulerAngles = Vector3.zero;
    }

    public void ShowRandomPoint() {
        var point = earthPoints.GetPoint();
    }

    private void Update() {

        var drawGizmosUnityEditor = false;
        
#if UNITY_EDITOR
        drawGizmosUnityEditor = SceneView.lastActiveSceneView.drawGizmos;
#endif
        
        if (GizmosEnabled || drawGizmosUnityEditor) {
            if (updateSunPositionOnOrbit) {
                ComputeSunDependantValues();
            }
        }
        
        if (!isRunning) {
            return;
        }

        Orbit();
        Rotate();
    }

    private void SetInitialTransform() {
        if (transformInitialized) {
            translationPivot.position = initialPosition;
            return;
        }

        translationPivot.position = (translationPivot.position - sun.transform.position).normalized * distanceFromSun + sun.transform.position;
        
        var rotation = invariableTransform.eulerAngles;
        rotation.x = inclination;
        invariableTransform.eulerAngles = rotation;

        initialPosition = translationPivot.position;
        transformInitialized = true;
    }
    
    private void ComputeValues(float yearDurationInSeconds, float dayDurationInSeconds) {
        orbitAngle =  -1.0f / period * 360.0f / yearDurationInSeconds;
        ownRotationAngle = -1.0f / ownRotationPeriod * 360.0f / dayDurationInSeconds;
    }

    private void ComputeSunDependantValues() {
        debugOrbitPoints = new Vector3[360];
        for (var i = 0; i < 360; i++) {
            var angle2 = i * Math.PI / 180.0f;
            debugOrbitPoints[i] = new Vector3(distanceFromSun * (float) Math.Cos(angle2), 0, distanceFromSun * (float) Math.Sin(angle2)) + sun.transform.position;
        }
    }

    private void Orbit() {
        translationPivot.RotateAround(sun.transform.position, Vector3.up, orbitAngle * Time.deltaTime);
        translationPivot.eulerAngles = Vector3.zero;
        
        if (updateSunPositionOnOrbit) {
            translationPivot.position = (translationPivot.position - sun.transform.position).normalized * distanceFromSun + sun.transform.position;
        }
    }

    private void Rotate() {
        rotationPivot.Rotate(rotationPivot.up, ownRotationAngle * Time.deltaTime, Space.World);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        
        /*Gizmos.DrawLine(
            transform.position + ownRotationAxis,
            transform.position - ownRotationAxis
        );*/

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

    private void EnableRuntimeGizmos() {
        rotationAxisGizmo.gameObject.SetActive(true);
    }

    private void DisableRuntimeGizmos() {
        rotationAxisGizmo.gameObject.SetActive(false);
    }

    public struct InitializationParameters {
        public GameObject sun;
        public float planetScale;
    }

    public enum Season {
        Winter,
        Spring,
        Summer,
        Autumn
    }
}
