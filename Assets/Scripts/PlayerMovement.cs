using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    
   [SerializeField] private float speed = 12f;
   [SerializeField] private float gravity = -9.81f * 2;
   [SerializeField] private float jumpHeight = 3f;

    public Transform groundCheck;
   [SerializeField] private float groundDistance = 0.4f;
    public LayerMask groundMask; // detact layer when ground checking

    Vector3 velocity; // of the fall

    bool isGrounded;
   // private Vector3 lastPosition = new Vector3(0f, 0f, 0f);


    // Update is called once per frame
    void Update()
    {
        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        //check sphere acts as an area that check for connections of the player groundCheck trasnform situated below the player with the groundLayer and its as long as the groundDistance
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //right is the red Axis, foward is the blue axis
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime); // made consistent with the frames by time

        //check if the player is on the ground so he can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}
