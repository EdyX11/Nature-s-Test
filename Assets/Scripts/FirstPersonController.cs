using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;


    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float gravity = -9.81f * 2;
    [SerializeField] private float jumpHeight = 3f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

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



    public Transform groundCheck;
    public LayerMask groundMask; // detact layer when ground checking

    [SerializeField] private float groundDistance = 0.4f;
    Vector3 velocity; // of the fall

    bool isGrounded;
    // private Vector3 lastPosition = new Vector3(0f, 0f, 0f);


    // Update is called once per frame
    void Update()
    {
        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        //check sphere acts as an area that check for connections of the player groundCheck trasnform situated below the player with the groundLayer and its as long as the groundDistance
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();
            ApplyFinalMovements();
        }


        //check if the player is on the ground so he can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
         
        characterController.Move(velocity * Time.deltaTime);

    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));
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

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        {

            moveDirection.y -= gravity * Time.deltaTime;

        }

            characterController.Move(moveDirection * Time.deltaTime);
        

    }
}
