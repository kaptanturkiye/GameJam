using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

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
    
    [SerializeField] private Light torch;
    private float torchTimer = 5f;
    private float coolDownTimer = 5f;
    private bool coolDownCheck = false;
    
    private void Awake()
    {
        playerControler = this.gameObject.GetComponent<CharacterController>();
    }

    private void Start()
    {
        torch.gameObject.SetActive(false);
    }

    private void Update()
    {
        Gravity();
        Movement();
        Jumping();
        DoubleJumping();
        Lithing();
    }


    private void Lithing()
    {
        
        if (Input.GetKey(KeyCode.Q) && torchTimer >= 0 && coolDownCheck == false)
        {
            torch.gameObject.SetActive(true);
                    
            if (torch.gameObject.activeInHierarchy == true)
            {
                torchTimer -= Time.deltaTime;
                // Debug.Log("IŞIK AÇIK" + (int)torchTimer);
            }
        }
        
        else if (torchTimer < 0)
        {
            torch.gameObject.SetActive(false);
            coolDownCheck = true;

            if (coolDownCheck == true && coolDownTimer >= 0)
            {
                coolDownTimer -= Time.deltaTime;
                // Debug.Log("COOLDOWN" + (int)coolDownTimer);
            }

            if (coolDownTimer < 0)
            {
                torchTimer = 5f;
                coolDownCheck = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Q) && torch.gameObject.activeInHierarchy == true)
        {
            torch.gameObject.SetActive(false);
            torchTimer = 5f;
        }


        #region  IŞIK RENGİ
        
        if (torch.gameObject.activeInHierarchy == true && Input.GetKey(KeyCode.Tab))
        {
            torch.color = Color.cyan;
        }
        else if (torch.gameObject.activeInHierarchy == true && Input.GetKeyUp(KeyCode.Tab))
        {
            torch.color = Color.red;
        }
        
        #endregion
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "kup")
        {
            Debug.Log("E TUŞUNA BASIN");
            if (other.gameObject.name == "kup" && Input.GetKeyDown(KeyCode.E))
            {
                //Destroy(other.gameObject);
                other.gameObject.GetComponent<Transform>().localScale = new Vector3(20f, 20f, 20f);
            }
        }
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
