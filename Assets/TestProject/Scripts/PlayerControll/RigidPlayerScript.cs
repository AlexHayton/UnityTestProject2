﻿using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class RigidPlayerScript : MonoBehaviour
{

    private Camera mainCamera;
    public float cameraHeight;
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

    void Start()
    {
        //sets initial camera position``
        mainCamera = Camera.main;
        mainCamera.transform.parent = null;
        mainCamera.transform.position = transform.position - mainCamera.transform.forward;
        cameraOffset = (mainCamera.transform.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        UpdateVelocity();
    }

    void Update()
    {
        HandleSpecialInput();
        UpdateCamera();
        UpdateRotation();
    }

    void LateUpdate()
    {
    }

    void HandleSpecialInput()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(1);
        }
    }

    void UpdateVelocity()
    {
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

        var inputVector = new Vector3(xVel, 0, yVel).normalized;

        //rotate for isometric``
        var targetVelocity = (inputVector.sqrMagnitude > 1 ?
            new Vector3(inputVector.x + inputVector.z, 0, inputVector.z - inputVector.x).normalized : new Vector3(inputVector.x + inputVector.z, 0, inputVector.z - inputVector.x)) * speed;

        Vector3 deltaV = (targetVelocity - rigidbody.velocity);
        deltaV.y = 0;
        rigidbody.AddForce(deltaV, ForceMode.VelocityChange);
        Console.WriteLine(deltaV);
    }

    void UpdateRotation()
    {
        var mouseEnvironmentHit = mainCamera.ScreenPointToRay(Input.mousePosition);
        var collPoints = Physics.RaycastAll(mouseEnvironmentHit);
        if (collPoints.Any())
        {
            var lookAt = collPoints[0].point;
            var forward = lookAt - transform.FindChild("Head").position;
            forward.y = 0;
            rigidbody.MoveRotation(Quaternion.LookRotation(forward));

            Debug.DrawLine(transform.FindChild("Head").position, lookAt, Color.red);
        }
        else
        {
            rigidbody.MoveRotation(Quaternion.LookRotation(rigidbody.velocity));
        }
    }

    void UpdateAnimationParameters()
    {
        Vector3 velocity = this.rigidbody.velocity;
        //Vector3 walkSpeed = new Vector3(velocity.x, 0, velocity.z);
        //this.animator.SetFloat(ANIM_PARAMS_SPEED, walkSpeed.magnitude);
        //this.animator.SetBool(ANIM_PARAMS_RELOADING, false);
    }

    void UpdateCamera()
    {
        //MoveCamera
        Vector3 enemyDir = GetDirectionOfenemies();
        Vector3 cameraDestination = transform.position + cameraOffset * cameraHeight + enemyDir.normalized;
        float cameraDestinationSize = 8 + enemyDir.magnitude * .2f;
        if (mainCamera.isOrthoGraphic)
            mainCamera.orthographicSize += (cameraDestinationSize - mainCamera.orthographicSize) * Time.deltaTime;
        mainCamera.transform.position += (cameraDestination - mainCamera.transform.position) * Time.deltaTime;
        mainCamera.transform.position = cameraDestination;

    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    public void OnTakeDamage(GameObject attacker)
    {
        if (onHitGui)
        {
            Instantiate(onHitGui, onHitGui.transform.position, onHitGui.transform.rotation);
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
        foreach (var enemy in enemies)
        {
            dir += enemy.transform.position - transform.position;
        }
        dir.Normalize();

        float avgDist = enemies.Sum(enemy => Vector3.Dot(enemy.transform.position - transform.position, dir));

        avgDist /= enemies.Length;
        dir.y = 0;
        dir *= avgDist;
        return dir;
    }
}