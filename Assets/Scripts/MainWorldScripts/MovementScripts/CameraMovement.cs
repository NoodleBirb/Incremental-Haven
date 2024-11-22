// Code found on https://web.archive.org/web/20201112011806/https://wiki.unity3d.com/index.php/MouseOrbitImproved
// Author: Veli V & highway900

using UnityEngine;
using System.Collections;
 
[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour {
 
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
 
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
 
    public float distanceMin = 2.5f;
    public float distanceMax = 15f;
 
    private Rigidbody rigid;
 
    float x = 0.0f;
    float y = 0.0f;
    
 
    // Use this for initialization
    void Start () 
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
 
        rigid = GetComponent<Rigidbody>();
 
        // Make the rigid body not change rotation
        if (rigid != null)
        {
            rigid.freezeRotation = true;
        }
    }
 
    void LateUpdate () 
    {
        if (!Inventory.showInventory && target) 
        {
            if (Input.GetMouseButton(1)){
                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }
            y = ClampAngle(y, yMinLimit, yMaxLimit);
 
            Quaternion rotation = Quaternion.Euler(y, x, 0);
 
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);

            if (Physics.Linecast(target.position, transform.position, out RaycastHit hit))
            {
                distance -= hit.distance;
            }
            Vector3 negDistance = new(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;
 
            transform.SetPositionAndRotation(position, rotation);
        }
    }
 
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}