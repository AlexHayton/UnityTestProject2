using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public Vector3 CameraAngle;
    public float CameraDistance = 12;
    public float CameraHeightOffset = 0;

    public bool MouseLead = false;
    public float LeadAmount = .1f;

    [HideInInspector]
    public Transform MainCameraTransform;

    // Use this for initialization
    void Awake()
    {
        MainCameraTransform = transform.FindChild("PlayerCamera");
        MainCameraTransform.parent = null;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //start out with normal orientation and position
        MainCameraTransform.rotation = Quaternion.Euler(CameraAngle);
        MainCameraTransform.position = transform.position - MainCameraTransform.forward * CameraDistance;

        //add lead
        if (MouseLead)
        {
            var mouseDirection = GetComponent<MouseLooker>().LookDirection;
            MainCameraTransform.position += mouseDirection * LeadAmount;
        }

    }
}
