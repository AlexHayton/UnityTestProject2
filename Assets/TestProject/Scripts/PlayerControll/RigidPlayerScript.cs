﻿using System.Threading;
using UnityEngine;
using System.Collections;
using TestProject;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class RigidPlayerScript : MonoBehaviour
{

    private Camera mainCamera;
    public float cameraHeight;
    private Vector3 camOffsetFromEnemies;
    public float speed = 5f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 1.0f;
    public float turnSpeed = 400.0f;
	public GameObject onHitGui;

    private Transform gripPoint;
	private Animator animator;

    [HideInInspector]
    public Transform LaserTransform;

    private bool grounded = false;
    private Vector3 lookTarget;
    private Vector3 cameraOffset = Vector3.zero;

	private const string ANIM_PARAMS_SPEED = "Speed";
	private const string ANIM_PARAMS_RELOADING = "Reloading";

    void Awake()
    {
        rigidbody.freezeRotation = true;

    }

    void Start()
    {
        camOffsetFromEnemies = new Vector3(0, 0, 0);
        gripPoint = transform.FindChildRecursive("PlayerGrabPoint");
        //sets initial camera position
        mainCamera = transform.root.GetComponentInChildren<Camera>();
        mainCamera.transform.parent = null;
        mainCamera.transform.position = transform.position - mainCamera.transform.forward * cameraHeight * 10;
        mainCamera.orthographicSize = 13;
        cameraOffset = mainCamera.transform.position - transform.position;

		animator = this.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
		UpdateVelocity();
    }

	void Update()
	{
		HandleSpecialInput();
		UpdateRotation();
		UpdateAnimationParameters();
	}

	void LateUpdate()
	{
		UpdateCamera();	
	}
	
	void HandleSpecialInput()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
		
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.LoadLevel (1);
		}
	}

	void UpdateVelocity()
	{
		if (grounded)
		{
			
			//************************************
			// Rotation
			//************************************
			
			// rotate towards target
			//Vector3 muzzleToMouse = (GetMouseOnPlane(new Plane(Vector3.up, GetComponentInChildren<WeaponScript>().PrimaryWeapon.muzzlePosition.transform.position)) - GetComponentInChildren<WeaponScript>().PrimaryWeapon.muzzlePosition.transform.position);
			//muzzleToMouse.y = 0;
			//Vector3 playerToMuzzle = GetComponentInChildren<WeaponScript>().PrimaryWeapon.muzzlePosition.transform.position - transform.position;
			//playerToMuzzle.y = 0;
			
			//************************************
			// Movement
			//************************************
			
			// Calculate how fast we should be moving
			Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			
			//rotate for isometric
			targetVelocity = new Vector3(targetVelocity.x + targetVelocity.z, 0, targetVelocity.z - targetVelocity.x) * Mathf.Sqrt(2) / 2;
			
			if (targetVelocity.sqrMagnitude > 1f)
				targetVelocity.Normalize();
			//targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;
			
			// Apply a force that attempts to reach our target velocity
			Vector3 velocityChange = (targetVelocity - rigidbody.velocity);
			//velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			//velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			//velocityChange.y = 0;
			rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
			
		}
		
		grounded = false;
	}
	
	void UpdateRotation()
	{
		if(LaserTransform != null)
		{
			var lookDir = GetMouseOnPlane(new Plane(Vector3.up, gripPoint.position)) - transform.position;
			Quaternion targetRot = Quaternion.LookRotation(lookDir);
			
			// only rotate around y axis
			targetRot.x = 0;
			targetRot.z = 0;
			
			float rotSpeed = turnSpeed * Time.deltaTime;
			rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRot, 1000));
		}
	}

	void UpdateAnimationParameters()
	{
		Vector3 velocity = this.rigidbody.velocity;
		Vector3 walkSpeed = new Vector3(velocity.x, 0, velocity.z);
		this.animator.SetFloat(ANIM_PARAMS_SPEED, walkSpeed.magnitude);
		this.animator.SetBool(ANIM_PARAMS_RELOADING, false);
	}

	void UpdateCamera()
	{
		//MoveCamera
		Vector3 enemyDir = GetDirectionOfenemies();
		Vector3 cameraDestination = transform.position + cameraOffset + enemyDir.normalized;
		float cameraDestinationSize = 8 + enemyDir.magnitude * .2f;
		mainCamera.orthographicSize += (cameraDestinationSize - mainCamera.orthographicSize) * Time.fixedDeltaTime;
		mainCamera.transform.position += (cameraDestination - mainCamera.transform.position) * Time.fixedDeltaTime;
	}

    void OnCollisionStay()
    {
        grounded = true;
    }

	public void OnTakeDamage(GameObject attacker) {
		if (onHitGui){
			Instantiate(onHitGui, onHitGui.transform.position, onHitGui.transform.rotation);
		}	
	}

    public Vector3 GetMouseOnPlane(Plane plane)
    {
        // search point from the mouse on the plane too look at it
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hit;
        if (plane.Raycast(ray, out hit))
        {
            return ray.GetPoint(hit);
        }
        else
        {
            Vector3 position = Input.mousePosition;
            position.y = transform.position.y;
            return position;
        }
    }


    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    private Vector3 GetDirectionOfenemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var dir = new Vector3(0, 0, 0);
        if (enemies.Length == 0)
            return dir;
        float avgDist = 0;
        foreach (var enemy in enemies)
        {
            dir += enemy.transform.position - transform.position;
        }
        dir.Normalize();

        foreach (var enemy in enemies)
        {
            avgDist += Vector3.Dot(enemy.transform.position - transform.position, dir);
        }
        avgDist /= enemies.Length;
        dir.y = 0;
        dir *= avgDist;
        return dir;
    }

	public Camera GetPlayerCamera() {
		return this.mainCamera;
	}
}