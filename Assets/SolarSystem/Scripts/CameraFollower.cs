using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    [SerializeField] private Transform sun;
    [SerializeField] private Transform planet;
    [SerializeField] private float distance;
    [SerializeField] private float angle;
    [SerializeField] private Transform cameraPivot;

    private Transform _sun;
    private Transform _planet;
    private float _distance;
    private float _angle;

    private Transform Sun => _sun != null ? _sun : sun;
    private Transform Planet => _planet != null ? _planet : planet;
    private float Distance => _distance != 0.0f ? _distance : distance;
    private float Angle => _angle != 0.0f ? _angle : angle;

    public Transform CameraPivot => cameraPivot;


    public void Initialize(CameraFollowerInitializationData initializationData) {
        _sun = initializationData.sun;
        _planet = initializationData.planet;
        _distance = initializationData.distance;
        _angle = initializationData.angle;
    }
    
    private void Start() {
        UpdatePosition();
    }
    
    // Update is called once per frame
    private void LateUpdate() {
        UpdatePosition();
    }

    private void UpdatePosition() {
        var a = Sun.position;
        var b = Planet.position;

        var x = (a.x + b.x) / 2;
        var y = (a.y + b.y) / 2;
        var z = (a.z + b.z) / 2;
        
        var midPoint = new Vector3(x, y, z);

        var vector1 = a - midPoint;

        var perpendicular = Vector3.Cross(vector1, Vector3.up).normalized;
        
        transform.position = midPoint + perpendicular * Distance;
        
        transform.RotateAround(midPoint, Vector3.up, -Angle);
        
        transform.LookAt(midPoint);
    }

    public struct CameraFollowerInitializationData {
        
        public Transform sun;
        public Transform planet;
        public float distance;
        public float angle;
    }
}
