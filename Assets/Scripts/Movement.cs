using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    [SerializeField] public float walkSpeed = 4f;
    [SerializeField] public float sprintSpeed = 14f;
    [SerializeField] public float maxVelocityChange = 10f;
    [Space]
    [SerializeField] public float airControl = .5f;
    [Space]
    [SerializeField] public float jumpHeight = 5f;

    private Vector2 input;
    private Rigidbody rb;

    private bool isSprinting;
    private bool isJumping;

    private bool isGrounded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();

        isSprinting = Input.GetButton("Sprint");
        isJumping = Input.GetButton("Jump");
    }

    private void OnTriggerStay(Collider other)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            if (isJumping)
            {
                rb.velocity = new Vector3(rb.velocity.x,jumpHeight,rb.velocity.z);
            }

            else if (input.magnitude > .5f)
            {
                rb.AddForce(CalculateMovement(isSprinting ? sprintSpeed : walkSpeed), ForceMode.VelocityChange);
            }

            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * .2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * .2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }
        else
        {
            if (input.magnitude > .5f)
            {
                rb.AddForce(CalculateMovement(isSprinting ? sprintSpeed  * airControl: walkSpeed * airControl), ForceMode.VelocityChange);
            }

            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * .2f * Time.fixedDeltaTime, velocity1.y, velocity1.z * .2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }

        isGrounded=false;
        
    }

    private Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVel = new Vector3(input.x, 0, input.y);
        targetVel = transform.TransformDirection(targetVel);

        targetVel *= _speed;

        Vector3 velocity = rb.velocity;

        if (input.magnitude > .5f)
        {
            Vector3 velocityChange = targetVel - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

            velocityChange.y = 0;

            return (velocityChange);
        }
        else
        {
            return new Vector3();
        }


    }

}
