using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Public variables accessible from the Unity editor
    public GameTimer gameTimer;           // Reference to the game timer for ending the game
    [SerializeField] float speed = 6.0f;            // Player movement speed
    [SerializeField] float gravity = 9.8f;          // Gravity force
   [SerializeField]  Transform hologram;            // Hologram to indicate gravity direction change
    [SerializeField] Camera playerCamera;           // Camera following the player
    [SerializeField] float gameOverTime = 3.0f;     // Time before game over if the player is airborne

    // Private variables for internal logic
    private Vector3 moveDirection;        // Direction of player movement
    private Rigidbody rb;                 // Player's Rigidbody component for physics interactions
    private Animator animator;            // Player's Animator component for animations
    private Vector3 newGravity = Vector3.zero; // New gravity direction selected by the player
    private float airborneTime = 0.0f;    // Time the player has been airborne

    // Ground check variables
    [SerializeField] Transform groundCheck;         // Transform used to check if the player is grounded
   [SerializeField] LayerMask groundLayer;         // Layer mask to identify ground objects
    public bool isGrounded;               // Flag indicating if the player is grounded
    public float groundCheckRadius = 0.2f;// Radius of the ground check sphere
    [SerializeField] float jumpForce = 5f;          // Force applied for jumping

    void Start()
    {
        // Initialize Rigidbody and Animator components
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Disable the hologram initially
        hologram.gameObject.SetActive(false);

        // Set the initial gravity direction
        Physics.gravity = Vector3.down * gravity;
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle gravity direction selection and movement
        HandleGravitySelection();
        HandleMovement();
        UpdateHologramPosition();

        // Update airborne status and animations
        if (isGrounded)
        {
            airborneTime = 0.0f;
            animator.SetBool("isFalling", false);
        }
        else
        {
            animator.SetBool("isFalling", true);
            airborneTime += Time.deltaTime;

            // End the game if airborne for too long
            if (airborneTime >= gameOverTime)
            {
                gameTimer.EndGame();
            }
        }
    }

    // Handle player input for selecting gravity direction
    void HandleGravitySelection()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            hologram.gameObject.SetActive(true);
            RotateHologram(Vector3.forward);
            newGravity = -transform.forward;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            hologram.gameObject.SetActive(true);
            RotateHologram(Vector3.back);
            newGravity = transform.forward;
            
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            hologram.gameObject.SetActive(true);
            RotateHologram(Vector3.left);
            newGravity = transform.right;
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            hologram.gameObject.SetActive(true);
            RotateHologram(Vector3.right);
            newGravity = -transform.right;
            
        }

        // Apply the new gravity direction when the Enter key is pressed
        if (Input.GetKeyUp(KeyCode.Return) && hologram.gameObject.activeSelf)
        {
            transform.rotation = hologram.rotation;
            ChangeGravityDirection(newGravity);
            hologram.gameObject.SetActive(false);
            newGravity = Vector3.zero;
        }

        // Reset the hologram if arrow keys are released without pressing Enter
        if ((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) && hologram.gameObject.activeSelf)
        {
            hologram.gameObject.SetActive(false);
            hologram.rotation = transform.rotation;
            playerCamera.transform.rotation = transform.rotation;
            newGravity = Vector3.zero;
        }
    }

    // Rotate the hologram to indicate the new gravity direction
    void RotateHologram(Vector3 direction)
    {
        if (direction == Vector3.forward)
            hologram.rotation *= Quaternion.Euler(90, 0, 0);
        else if (direction == Vector3.back)
            hologram.rotation *= Quaternion.Euler(-90, 0, 0);
        else if (direction == Vector3.left)
            hologram.rotation *= Quaternion.Euler(0, 0, 90);
        else if (direction == Vector3.right)
            hologram.rotation *= Quaternion.Euler(0, 0, -90);
    }

    // Change the gravity direction
    void ChangeGravityDirection(Vector3 direction)
    {
        Physics.gravity = direction * gravity;
    }

    // Handle player movement and animations
    void HandleMovement()
    {
        moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection -= transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection -= transform.right;
        }

        moveDirection = moveDirection.normalized * speed;

        // Update running animation
        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Move the player
        rb.MovePosition(rb.position + moveDirection * Time.deltaTime);

        // Handle jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 jumpDirection = -Physics.gravity.normalized; // Jump in the direction opposite to gravity
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            // animator.SetBool("isFalling", false);
        }
        // if (isGrounded && velocity.y < 0.1f)
        // {
        //     animator.SetBool("isFalling", false);
        // }

        
    }

    // Update the hologram's position to follow the player
    void UpdateHologramPosition()
    {
        hologram.position = transform.position;
    }


}
