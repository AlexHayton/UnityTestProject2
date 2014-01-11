using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    /// <summary>
    /// y should always be 45deg
    /// </summary>
    public Vector3 CameraAngle;

    /// <summary>
    /// how high above the player the camera will be
    /// </summary>
    public float CameraDistance;

    /// <summary>
    /// how far the camera will look in the direction of the mouse - set to 0 to disable
    /// </summary>
    public float CameraMouseLead;

    /// <summary>
    /// if true, camera will slide to lead the player, otherwise it will pivot
    /// </summary>
    public bool SlideLead = false;


    public Transform mainCameraTransform;

    // Use this for initialization
    void Awake()
    {
        mainCameraTransform = transform.FindChild("PlayerCamera");
        mainCameraTransform.parent = null;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //start out with normal orientation and position
        mainCameraTransform.rotation = Quaternion.Euler(CameraAngle);
        mainCameraTransform.position = transform.position - mainCameraTransform.forward * CameraDistance;

        //add lead
        if (SlideLead)
        {
            var mouseDirection = GetComponent<MouseLooker>().LookDirection;
            mainCameraTransform.position += mouseDirection * CameraMouseLead;
        }
        else
        {
            //yet to be implemented
        }
    }
}
