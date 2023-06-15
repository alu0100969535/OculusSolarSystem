using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

    [SerializeField] private Transform sun;
    [SerializeField] private Transform planet;
    [SerializeField] private float distance;
    [SerializeField] private float angle;

    private void Start() {
        UpdatePosition();
    }
    
    // Update is called once per frame
    private void LateUpdate() {
        UpdatePosition();
    }

    private void UpdatePosition() {
        var a = sun.position;
        var b = planet.position;

        var x = (a.x + b.x) / 2;
        var y = (a.y + b.y) / 2;
        var z = (a.z + b.z) / 2;
        
        var midPoint = new Vector3(x, y, z);

        var vector1 = a - midPoint;

        var perpendicular = Vector3.Cross(vector1, Vector3.up).normalized;
        
        transform.position = midPoint + perpendicular * distance;
        
        transform.RotateAround(midPoint, Vector3.up, -angle);
        
        transform.LookAt(midPoint);
    }
}
