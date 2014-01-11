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
            var head = transform;
            LookPosition = collPoints[0].point;
            LookDirection = LookPosition - head.position;
            LookDirection.y = 0;
            head.rotation = Quaternion.LookRotation(LookDirection);
            Debug.DrawRay(head.position, head.forward, Color.red);
        }
        else
        {
            LookPosition = transform.position;
            LookDirection = Vector3.zero;
        }
    }
}
