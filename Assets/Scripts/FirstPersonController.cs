using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && onGround;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool useFootsteps = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;


    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] private float sprintSpeed = 10.0f;


    [Header("Jumping Parameters")]
    [SerializeField] private float gravity = 30.0f;
    [SerializeField] private float jumpHeight = 10.0f;



    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    
    [Header("Ground Parameters")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask; // detect layer when ground checking
    [SerializeField] private bool onGround;
    [SerializeField] private float groundDistance = 0.2f;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footStepAudioSource = default;
    [SerializeField] private AudioClip[] grassClips = default;
    [SerializeField] private AudioClip[] gravelClips = default;
    private float footstepTimer = 0;




    //Vector3 velocity; // of the fall
    private Camera playerCamera;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector2 currentInput;

    public float rotationX = 0;

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }


    void Update()
    {
        // Update onGround status every frame
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();
        }

        if (canJump && CanMove)
        {
            HandleJump();
        }

        // Reset moveDirection.y when the player is grounded and the downward velocity is significant
        if (characterController.velocity.y < -1 && onGround)
        {
            moveDirection.y = 0;
        }

        ApplyFinalMovements();
    }




    private void HandleMovementInput()
    {
        currentInput = new Vector2((IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));
        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        if (!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !MenuManager.Instance.isMenuOpen)
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
            //we clamp the rotation so we cant Over-rotate (like in real life)
            rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
            //applying both rotations
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);

        }
            
    }
    private void HandleJump()
    {
        // Apply jump force if the jump key is pressed and the player is on the ground
        if (ShouldJump)
        {
            // Instantly provide an upward force by setting moveDirection.y to jumpHeight
            moveDirection.y = jumpHeight;
        }
    }



    private void ApplyFinalMovements()
    {
        // Always apply gravity, pulling the player down
        moveDirection.y -= gravity * Time.deltaTime;

        // When on the ground, reset the vertical speed to keep the player grounded
        if (onGround && moveDirection.y < 0)
        {
            moveDirection.y = -2f;
        }

        // Move the character controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
