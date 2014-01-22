/// <summary>
/// UnityTutorials - A Unity Game Design Prototyping Sandbox
/// <copyright>(c) John McElmurray and Julian Adams 2013</copyright>
/// 
/// UnityTutorials homepage: https://github.com/jm991/UnityTutorials
/// 
/// This software is provided 'as-is', without any express or implied
/// warranty.  In no event will the authors be held liable for any damages
/// arising from the use of this software.
///
/// Permission is granted to anyone to use this software for any purpose,
/// and to alter it and redistribute it freely, subject to the following restrictions:
///
/// 1. The origin of this software must not be misrepresented; you must not
/// claim that you wrote the original software. If you use this software
/// in a product, an acknowledgment in the product documentation would be
/// appreciated but is not required.
/// 2. Altered source versions must be plainly marked as such, and must not be
/// misrepresented as being the original software.
/// 3. This notice may not be removed or altered from any source distribution.
/// </summary>

using UnityEngine;
using System.Collections;

/// <summary>
/// #DESCRIPTION OF CLASS#
/// </summary>
public class CharacterControllerLogic : MonoBehaviour 
{
	#region Variables (private)
	
	public ThirdPersonCamera gamecam;
	public float rotationDegreePerSecond = 120f;
	public float directionSpeed = 1.5f;
	public float directionDampTime = 0.25f;
	public float speedDampTime = 0.05f;
	public float maxSpeedFall = 0.5f;
	public float fovDampTime = 3f;
	public float jumpMultiplier = 1f;
	public float jumpDist = 1f;	
	public float LocomotionThreshold = 0.2f;
	
	// Private global only
	private float leftX = 0f;
	private float leftY = 0f;
	private AnimatorStateInfo stateInfo;
	private AnimatorTransitionInfo transInfo;
	private float speed = 0f;
	private float direction = 0f;
	private float charAngle = 0f;
	private const float SPRINT_SPEED = 2.0f;	
	private const float SPRINT_FOV = 75.0f;
	private const float NORMAL_FOV = 60.0f;
	private float capsuleHeight;	
	private bool oldStrafingValue = false;
	private float oldSpeedMagnitude = 0f;
	private float distToGround = 0f;
	private RaycastHit groundedHit;
	private bool oldOnGround = true;

	// components
	private Animator animator;
	private CapsuleCollider capCollider;

	// Hashes
    private int m_LocomotionId = 0;
	private int m_LocomotionPivotLId = 0;
	private int m_LocomotionPivotRId = 0;	
	private int m_LocomotionPivotLTransId = 0;	
	private int m_LocomotionPivotRTransId = 0;	

	private int m_LocomotionJump = 0;	
	private int m_IdleJump = 0;	
	private int m_JumpDown = 0;	
	private int m_Fall = 0;	
	private int m_JumpUp = 0;

	
	#endregion
		
	
	#region Properties (public)

	public Animator Animator
	{
		get
		{
			return this.animator;
		}
	}

	public float Speed
	{
		get
		{
			return this.speed;
		}
	}

	#endregion
	
	
	#region Unity event functions
	
	/// <summary>
	/// Use this for initialization.
	/// </summary>
	void Start() 
	{
		animator = GetComponent<Animator>();
		capCollider = GetComponent<CapsuleCollider>();
		capsuleHeight = capCollider.height;

		if(animator.layerCount >= 2)
		{
			animator.SetLayerWeight(1, 1);
		}		
		
		// Hash all animation names for performance
        m_LocomotionId = Animator.StringToHash("Move.Locomotion");
		m_LocomotionPivotLId = Animator.StringToHash("Move.LocomotionPivotL");
		m_LocomotionPivotRId = Animator.StringToHash("Move.LocomotionPivotR");
		m_LocomotionPivotLTransId = Animator.StringToHash("Move.Locomotion -> Move.LocomotionPivotL");
		m_LocomotionPivotRTransId = Animator.StringToHash("Move.Locomotion -> Move.LocomotionPivotR");

		m_LocomotionJump = Animator.StringToHash("Jump.LocomotionJump");
		m_IdleJump = Animator.StringToHash("Jump.IdleJump");
		m_JumpDown = Animator.StringToHash("Jump.JumpDown");
		m_Fall= Animator.StringToHash("Jump.Fall");
		m_JumpUp = Animator.StringToHash("Jump.JumpUp");

		distToGround = collider.bounds.extents.y -0.2f;
	}
	
	/// <summary>
	/// Update is called once per frame.
	/// </summary>
	void Update() 
	{
		if (animator && gamecam.CamState != ThirdPersonCamera.CamStates.FirstPerson)
		{
			CanClimb();

			stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			transInfo = animator.GetAnimatorTransitionInfo(0);
						
			// Press A to jump
			if (Input.GetButton("Jump"))
			{
				animator.SetBool("Jump", true);
			}
			else
			{
				animator.SetBool("Jump", false);
			}	
			
			// Pull values from controller/keyboard
			leftX = Input.GetAxis("Horizontal");
			leftY = Input.GetAxis("Vertical");	

			Animator.SetFloat("Horizontal", leftX, directionDampTime, Time.deltaTime);
			Animator.SetFloat("Vertical", leftY, directionDampTime, Time.deltaTime);
			Animator.SetFloat("Velocity", rigidbody.velocity.magnitude, speedDampTime, Time.deltaTime);
			
			charAngle = 0f;
			direction = 0f;	
			float charSpeed = 0f;
		
			// Translate controls stick coordinates into world/cam/character space
            StickToWorldspace(this.transform, gamecam.transform, ref direction, ref charSpeed, ref charAngle, IsInPivot());		
			
			// Press B to sprint
			if (Input.GetButton("Sprint"))
			{
				speed = Mathf.Lerp(speed, SPRINT_SPEED, Time.deltaTime);
				gamecam.camera.fieldOfView = Mathf.Lerp(gamecam.camera.fieldOfView, SPRINT_FOV, fovDampTime * Time.deltaTime);
			}
			else
			{
				speed = charSpeed;
				gamecam.camera.fieldOfView = Mathf.Lerp(gamecam.camera.fieldOfView, NORMAL_FOV, fovDampTime * Time.deltaTime);		
			}
			
			animator.SetFloat("Speed", speed, speedDampTime, Time.deltaTime);
			animator.SetFloat("Direction", direction, directionDampTime, Time.deltaTime);
			
			if (speed > LocomotionThreshold)	// Dead zone
			{
				if (!IsInPivot())
				{
					Animator.SetFloat("Angle", charAngle);
				}
			}

			if (IsInCameraTarget() || speed < LocomotionThreshold && Mathf.Abs(leftX) < 0.05f)    // Dead zone
			{
				animator.SetFloat("Speed", 0f);
				animator.SetFloat("Direction", 0f);
				animator.SetFloat("Angle", 0f);
			}	

			animator.SetBool("Strafing", IsInCameraTarget());

			if (!oldStrafingValue && IsInCameraTarget()) {
				animator.SetTrigger("StrafingTrigger");
			}

			animator.SetBool("Strafing", IsInCameraTarget());
			animator.SetBool("CanClimb", CanClimb());

			oldStrafingValue = IsInCameraTarget();

		} 
	}
	
	/// <summary>
	/// Any code that moves the character needs to be checked against physics
	/// </summary>
	void FixedUpdate()
	{		

		bool isGrounded = IsGrounded();
		if (!oldOnGround && isGrounded) {
			animator.SetTrigger("GroundedTrigger");
			Debug.Log ("Grounded");
		} else if (oldOnGround && !isGrounded && !IsInJump() && !IsInAir()) {
			animator.SetTrigger("InAirTrigger");
			Debug.Log ("InAir");
		}

		animator.SetBool("Grounded", isGrounded);
		oldOnGround = isGrounded;


		// Rotate character model if stick is tilted right or left, but only if character is moving in that direction
		if (!IsInCameraTarget() && gamecam.CamState != ThirdPersonCamera.CamStates.Free && !IsInPivot() && ((direction >= 0 && leftX >= 0) || (direction < 0 && leftX < 0)))
		{
			Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (leftX < 0f ? -1f : 1f), 0f), Mathf.Abs(leftX));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
        	this.transform.rotation = (this.transform.rotation * deltaRotation);
		}		
		
		if (IsInJump())
		{
			float oldY = transform.position.y;
			transform.Translate(Vector3.up * jumpMultiplier * animator.GetFloat("JumpCurve"));
			if (IsInLocomotionJump())
			{
				transform.Translate(Vector3.forward * Time.deltaTime * jumpDist);
			}
			capCollider.height = capsuleHeight + (animator.GetFloat("CapsuleCurve") * 0.5f);
			if (gamecam.CamState != ThirdPersonCamera.CamStates.Free)
			{
				gamecam.ParentRig.Translate(Vector3.up * (transform.position.y - oldY));
			}
		}
	}

	void LateUpdate() {
		animator.ResetTrigger("StrafingTrigger");
		animator.ResetTrigger("GroundedTrigger");
		animator.ResetTrigger("InAirTrigger");
	}
	
	/// <summary>
	/// Debugging information should be put here.
	/// </summary>
	void OnDrawGizmos()
	{	
	
	}
	
	#endregion
	
	
	#region Methods
	
	public bool IsInJump()
	{
		return IsInIdleJump() || 	
			IsInLocomotionJump() ||
				IsInJumpUp();
	}

	public bool IsInAir() {
		return stateInfo.nameHash == m_JumpDown ||
				stateInfo.nameHash == m_Fall ;
	}

	public bool IsInIdleJump()
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Jump.IdleJump");
	}
	
	public bool IsInLocomotionJump()
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump.LocomotionJump")) {
			//animator.MatchTarget(climbHandler.getTargetMatch().position, climbHandler.getTargetMatch().rotation, AvatarTarget.RightHand, new MatchTargetWeightMask(Vector3.one, 1f) ,0.1f);
			return true;
		} 
		return false;
	}

	public bool IsInJumpUp() {
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Jump.JumpUp");
	}
	
	public bool IsInPivot()
	{
		return stateInfo.nameHash == m_LocomotionPivotLId || 
			stateInfo.nameHash == m_LocomotionPivotRId || 
			transInfo.nameHash == m_LocomotionPivotLTransId || 
			transInfo.nameHash == m_LocomotionPivotRTransId;
	}

    public bool IsInLocomotion()
    {
        return stateInfo.nameHash == m_LocomotionId;
    }

	public bool IsInCameraTarget() {
		return gamecam.CamState == ThirdPersonCamera.CamStates.Target;
	}
	
	public void StickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut, ref float angleOut, bool isPivoting)
    {
        Vector3 rootDirection = root.forward;
		Vector3 stickDirection = new Vector3(leftX, 0, leftY);
		
		speedOut = stickDirection.sqrMagnitude;	

        // Get camera rotation
        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f; // kill Y
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(CameraDirection));

        // Convert joystick input in Worldspace coordinates
        Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);
		
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.5f, root.position.z), axisSign, Color.red);
		
		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
		if (!isPivoting)
		{
			angleOut = angleRootToMove;
		}
		angleRootToMove /= 180f;
		
		directionOut = angleRootToMove * directionSpeed;

		float smoothedSpeed = Mathf.Lerp (speedOut, stickDirection.sqrMagnitude, speedDampTime * Time.deltaTime);

		if (oldSpeedMagnitude > smoothedSpeed && oldSpeedMagnitude - smoothedSpeed >= maxSpeedFall) {
			// joystick direction competly changed, smooth it down so it will not go into idle state
			speedOut = oldSpeedMagnitude - 0.01f;
		}

		oldSpeedMagnitude = speedOut;
	}	

	public bool IsGrounded() {
		Physics.Raycast (this.transform.position, -Vector3.up, out groundedHit);
		if (groundedHit.distance <= distToGround) {
			return true;
		} else {
			return false;
		}
	}

	public bool CanClimb() {
		return false;
	}


	
	#endregion Methods
}
