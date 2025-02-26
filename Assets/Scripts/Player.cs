using System;
using Unity.Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float airAccelerationMultiplier;
    [SerializeField] private float friction;
    [SerializeField] private float jumpForce;
    [SerializeField] private CinemachineCamera freeLookCamera;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    
    private Rigidbody rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private float defaultAcceleration;
    private bool canDash = true;


    private void Start()
    {
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnSpacePressed.AddListener(Jump);
        inputManager.OnShiftPressed.AddListener(Dash);
        rb = GetComponent<Rigidbody>();
        defaultAcceleration = acceleration;
    }

    private void Update()
    {
        transform.forward = freeLookCamera.transform.forward;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    private void MovePlayer(Vector2 direction)
    {
        //when character is not grounded, apply airAccelerationMultiplier to slow down in-air movement (increases difficulty).
        float currentAcceleration = isGrounded ? defaultAcceleration : defaultAcceleration * airAccelerationMultiplier;

        Vector3 camForward = freeLookCamera.transform.forward;
        Vector3 camRight = freeLookCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * direction.y + camRight * direction.x;


        //apply current acceleration and max speed constraint to horizontal axis (preserve gravity)
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 targetVelocity = moveDirection * maxSpeed;
        Vector3 velocityChange = targetVelocity - horizontalVelocity;

        velocityChange = Vector3.ClampMagnitude(velocityChange, currentAcceleration * Time.deltaTime);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        if (direction.magnitude < 0.1f)
        {
            rb.linearVelocity = new Vector3(
                Mathf.Lerp(rb.linearVelocity.x, 0, friction * Time.deltaTime),
                rb.linearVelocity.y, //gravity stays constant
                Mathf.Lerp(rb.linearVelocity.z, 0, friction * Time.deltaTime)
            );
        }
    }

    private void Dash()
    {
        Debug.Log("Dash function called");

        if (!canDash)
        {
            Debug.Log("Dash cooldown. Cannot dash");
            return;
        }

        Vector3 dashDirection = transform.forward;
        
        rb.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash()
    {
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        Invoke(nameof(EnableDash), dashCooldown);
    }

    private void EnableDash()
    {
        canDash = true;
    }



    private void Jump()
    {
        Debug.Log("jump function called");
        if (isGrounded || jumpCount < 2)
        {
            Debug.Log("Jumped");
            jumpCount++;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((groundLayer & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("grounded");
            jumpCount = 0;
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (groundLayer != 0)
        {
            Debug.Log("in air");
            isGrounded = false;
        }
    }


}
