using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

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
    [Header("Status Parameters")]
    private float calorieLossCooldown = 0.5f;  // Cooldown in seconds
    private float currentCooldown = 0;
    public float caloriesSpentSprinting = 5;
    



    [Header("Animator")]
    public Animator _animator;

    public GameObject bloodyScreen;
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
        //if (!IsOwner)
           // return;
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log($"On Ground: {onGround}"); // Uncomment when needed for debugging

        if (!PlayerState.Instance.isDead) // Ensures no updates run if the player is dead
        {
            HandleStamina();
            HandleHealth();

            if (CanMove)
            {
                HandleMovementInput();
                HandleMouseLook();
                if (canJump && onGround) // Ensure player can only jump if on the ground
                {
                    HandleJump();
                }
                ApplyFinalMovements();
            }
        }
    }




    private void HandleMovementInput()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        float movementSpeed = IsSprinting ? sprintSpeed : walkSpeed;
        currentInput = new Vector2(movementSpeed * verticalAxis, movementSpeed * horizontalAxis);
        float moveDirectionY = moveDirection.y;

        // Transform forward/backward movement
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) +
                        (transform.TransformDirection(Vector3.right) * currentInput.y);

        moveDirection.y = moveDirectionY;

        // Update animator parameters
        _animator.SetFloat("Vertical", verticalAxis * (IsSprinting ? 1.5f : 1f));// blend tree value for sprint is 1.5
        _animator.SetFloat("Horizontal", horizontalAxis);

        // Update the IsSprinting parameter (if applicable)
        //_animator.SetBool("IsSprinting", IsSprinting);
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
    

    private void HandleStamina()
    {
        // Check if the player is sprinting and moving
        if (IsSprinting && currentInput != Vector2.zero)
        {
            // Update the cooldown timer
            if (currentCooldown <= 0)
            {
                // Deduct calories spent sprinting and reset cooldown
                PlayerState.Instance.currentCalories -= 5;  // Deduct 5 calories
                currentCooldown = calorieLossCooldown;

                // Check if calories have fallen below a threshold
                if (PlayerState.Instance.currentCalories <= 500)
                {
                    canSprint = false;
                }
                // Ensure calorie count doesn't fall below zero and adjust health
                if (PlayerState.Instance.currentCalories < 0)
                {
                    PlayerState.Instance.currentCalories = 0;
                    PlayerState.Instance.currentHealth -= 10;
                }
            }
            else
            {
                // Decrease cooldown
                currentCooldown -= Time.deltaTime;  // Make sure to update this every frame
            }
        }
        else
        {
            canSprint = true;  // Allow sprinting when not currently sprinting or if movement stops
            currentCooldown = 0;  // Reset cooldown when not sprinting
        }
    }
   

    private void HandleHealth()
    {
        if (PlayerState.Instance.currentHealth <= 0 )
        {
            print("player dead fps ");
            PlayerState.Instance.isDead = true;

           StartCoroutine(ShowGameOverUI());
        }
        else if (PlayerState.Instance.currentHealth < 100)
        {
            print("player took damage");
            bloodyScreen.SetActive(true);


        }
        else
        {
            bloodyScreen.SetActive(false);
        }   
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<ScreenFader>().StartFade();
        MenuManager.Instance.gameOverUI.SetActive(true);

        StartCoroutine(ReturnToMainMenu());
    }


    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(4f);

        MenuManager.Instance.OpenMenu();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!PlayerState.Instance.isDead) // Check if the player is not dead at the start 
        {
            if (other.CompareTag("ZombieHand"))
            {
                
                PlayerState.Instance.TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
                // additional reactions specific to a zombie attack here, if necessary
            }
            else if (other.CompareTag("BearClaw"))
            {
                
                PlayerState.Instance.TakeDamage(other.gameObject.GetComponent<BearClaw>().damage);
                // Additional reactions specific to a bear attack can be added here
            }
        }
    }


    private void ApplyFinalMovements()
    {
        Vector3 beforeMovePosition = characterController.transform.position;

        // Apply gravity and move the character controller
        moveDirection.y -= gravity * Time.deltaTime;
        if (onGround && moveDirection.y < 0) { moveDirection.y = -2f; }
        characterController.Move(moveDirection * Time.deltaTime);

        Vector3 afterMovePosition = characterController.transform.position;
       // Debug.Log($"Before Move: {beforeMovePosition}, After Move: {afterMovePosition}");
    }


}
