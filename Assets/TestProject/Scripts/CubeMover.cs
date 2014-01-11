using UnityEngine;

public class CubeMover : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var forward = transform.forward;
        forward.y = 0;

        var xVel = 0;
        var yVel = 0;
        if (Input.GetKey(KeyCode.A))
            xVel -= 1;
        if (Input.GetKey(KeyCode.D))
            xVel += 1;
        if (Input.GetKey(KeyCode.S))
            yVel -= 1;
        if (Input.GetKey(KeyCode.W))
            yVel += 1;

        var inputVector = new Vector3(xVel, 0, yVel).normalized * 50;

        Vector3 deltaV = (inputVector - rigidbody.velocity);
        deltaV.y = 0;
        rigidbody.AddForce(deltaV * Time.deltaTime, ForceMode.VelocityChange);
        print(deltaV);

        transform.rotation = Quaternion.LookRotation(forward);

    }
}
