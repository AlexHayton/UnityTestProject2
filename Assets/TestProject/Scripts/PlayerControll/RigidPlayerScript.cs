using UnityEngine;
using System.Collections;
 
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class RigidPlayerScript : MonoBehaviour {
	
 	public Camera mainCamera;
	public float cameraHeight = 10f;
	public float speed = 5f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 1.0f;
	public float turnSpeed = 400.0f;
	
	private bool grounded = false; 
	private Vector3 lookTarget;
	private Vector3 centerOffset = Vector3.zero;
 
	void Awake () {
	    rigidbody.freezeRotation = true;		
		if (mainCamera) {
			centerOffset = mainCamera.transform.position - this.transform.position;		
			centerOffset.y = cameraHeight;
		}		
	}
 
	void FixedUpdate () {
		
		if (Input.GetKeyDown (KeyCode.F1)) {
			Application.LoadLevel(Application.loadedLevel);	
		}
		
	    if (grounded) {			
			
			//************************************
			// Rotation
			//************************************
						
			// rotate towards target		
			Vector3 lookDelta = (GetMouseOnPlane()-transform.position);
			Quaternion targetRot = Quaternion.LookRotation(lookDelta);
					
			// only rotate around y axis
			targetRot.x = 0;
			targetRot.z = 0;
			
			float rotSpeed = turnSpeed * Time.deltaTime;
			transform.rotation = Quaternion.RotateTowards( transform.rotation, targetRot, rotSpeed );
			
			
			//************************************
			// Movement
			//************************************
						
	        // Calculate how fast we should be moving
	        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	        //targetVelocity = transform.TransformDirection(targetVelocity);
	        targetVelocity *= speed;
 
	        // Apply a force that attempts to reach our target velocity
	        Vector3 velocity = rigidbody.velocity;
	        Vector3 velocityChange = (targetVelocity - velocity);
	        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
	        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
	        velocityChange.y = 0;
	        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
			
	    } 		
					
		//MoveCamera
		if (mainCamera) {
			Vector3 tempPos = this.transform.position + centerOffset;
			mainCamera.transform.position = tempPos;
		}
 
	    grounded = false;
	}
 
	void OnCollisionStay () {
	    grounded = true;    
	}
	
	public Vector3 GetMouseOnPlane() {					
		// search point from the mouse on the plane too look at it
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);			
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			return hit.point;
		} else {
			Vector3 position = Input.mousePosition;
			position.y = transform.position.y;
			return position;	
		}

	}
 
	float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
	
}