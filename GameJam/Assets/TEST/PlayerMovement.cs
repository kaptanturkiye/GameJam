using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    
    private CharacterController playerControler;
    private Vector3 velocity;
    
    private int doubleJumpCount = 0;
    
    private float speed = 12f;
    private float gravity = -49.05f;
    private float jumpHeight = 3.75f;
    private float groundDistance = 0.4f;
    
    private bool isGrounded;
    private bool doubleJump = false;
    
    private void Awake()
    {
        playerControler = this.gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        Gravity();
        Movement();
        Jumping();
        DoubleJumping();
    }

    private void Jumping()
    {
        if (Input.GetButtonDown("Jump") && doubleJumpCount == 0)
        {
            if (isGrounded == true)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                doubleJump = true;
                doubleJumpCount++;
            }
        }
    }

    private void DoubleJumping()
    {
        if (Input.GetButtonDown("Jump") && doubleJumpCount == 1)
        {
            if (isGrounded == false)
            { 
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                doubleJump = false;
            }
        }
    }
    
    private void Movement()
    {
        float HorizontalAxes = Input.GetAxis("Horizontal");
        float VerticalAxes = Input.GetAxis("Vertical");

        Vector3 move = transform.right * HorizontalAxes + transform.forward * VerticalAxes;
        playerControler.Move(move * speed * Time.deltaTime);
    }
    
    private void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded == true && velocity.y < 0)
        {
            velocity.y = -2f;
            doubleJumpCount = 0;
        }
        
        velocity.y += gravity * Time.deltaTime;
        playerControler.Move(velocity * Time.deltaTime);
        
    }
}
