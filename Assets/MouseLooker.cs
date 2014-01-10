using System.Linq;
using UnityEngine;
using System.Collections;

public class MouseLooker : MonoBehaviour
{

    public Vector3 LookDirection;
    public Vector3 LookPosition;
    private Camera playerCamera;

    // Use this for initialization
    void Start()
    {
        playerCamera = GetComponent<CameraFollow>().mainCameraTransform.camera;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseEnvironmentHit = playerCamera.ScreenPointToRay(Input.mousePosition);
        var collPoints = Physics.RaycastAll(mouseEnvironmentHit);
        if (collPoints.Any())
        {
            LookPosition = collPoints[0].point;
            LookDirection = LookPosition - transform.FindChild("Head").position;

            Debug.DrawLine(transform.FindChild("Head").position, LookPosition, Color.red);
        }
        else
        {
            LookPosition = transform.position;
            LookDirection = Vector3.zero;
        }
    }
}
