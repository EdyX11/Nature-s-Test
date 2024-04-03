using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && onGround;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
   // [SerializeField] private bool useFootsteps = true;

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
    [SerializeField] Transform headPos;
    [SerializeField] Transform Cam;



    [Header("Ground Parameters")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask; // detect layer when ground checking
    [SerializeField] private bool onGround;
    [SerializeField] private float groundDistance = 0.2f;

    [Header("Footstep Parameters")]
    //[SerializeField] private float baseStepSpeed = 0.5f;
   // [SerializeField] private float sprintStepMultiplier = 0.6f;
    //[SerializeField] private AudioSource footStepAudioSource = default;
   // [SerializeField] private AudioClip[] grassClips = default;
    //[SerializeField] private AudioClip[] gravelClips = default;
  //  private float footstepTimer = 0;

    [Header("Animator")]
    public Animator _animator;
  //  private Rigidbody rb;

    

    //Vector3 velocity; 
    private Camera playerCamera;
    [SerializeField]  private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector2 currentInput;

    public float rotationX = 0;

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }


    void Update()
    {
        // Update onGround status every frame
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log($"On Ground: {onGround}");
        //HandleStamina();
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();
            if (canJump)
            {
                HandleJump();
            }
            ApplyFinalMovements();

        }



       
    }





    private void HandleMovementInput()
    {

        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis= Input.GetAxis("Horizontal");

        currentInput = new Vector2((IsSprinting ? sprintSpeed : walkSpeed) * verticalAxis , (IsSprinting ? sprintSpeed : walkSpeed) * horizontalAxis);
        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        
        moveDirection.y = moveDirectionY;
        // Update animator with the current movement values
        _animator.SetFloat("Vertical", verticalAxis);
        _animator.SetFloat("Horizontal", horizontalAxis);

        // Update the IsSprinting parameter in the animator based on the IsSprinting variable
        _animator.SetBool("IsSprinting", IsSprinting);
    }

    private void HandleMouseLook()
    {

      Cam.position = headPos.position;
        
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
        if (ShouldJump)
        {
            moveDirection.y = jumpHeight;
            _animator.SetBool("Jump", true); // Indicate jumping start
        }
        else if (onGround)
        {
            _animator.SetBool("Jump", false); // Indicate jumping end
        }
    }
    /*private void HandleStamina()
    {
        if(IsSprinting && currentInput != Vector2.zero)
        {
            PlayerState.Instance.currentCalories -= 1 * Time.deltaTime;
            if (PlayerState.Instance.currentCalories < 0)
                PlayerState.Instance.currentCalories = 0;
            if (PlayerState.Instance.currentCalories <= 500) ;
         
            canSprint = false;
        }
        canSprint = true;


    }*/




    private void ApplyFinalMovements()
    {
        Vector3 beforeMovePosition = characterController.transform.position;

        // Apply gravity and move the character controller
        moveDirection.y -= gravity * Time.deltaTime;
        if (onGround && moveDirection.y < 0) { moveDirection.y = -2f; }
        characterController.Move(moveDirection * Time.deltaTime);

        Vector3 afterMovePosition = characterController.transform.position;
        Debug.Log($"Before Move: {beforeMovePosition}, After Move: {afterMovePosition}");
    }


}
