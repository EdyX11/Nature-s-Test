using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    private Animator animator;
    public float moveSpeed = 0.2f;

    private Vector3 stopPosition;
    private float walkTime;
    public float walkCounter;
    private float waitTime;
    public float waitCounter;
    private int walkDirection;

    public bool isWalking;

    private static readonly Quaternion[] Directions = {
        Quaternion.Euler(0f, 0f, 0f),    // North
        Quaternion.Euler(0f, 90f, 0f),   // East
        Quaternion.Euler(0f, -90f, 0f),  // West
        Quaternion.Euler(0f, 180f, 0f)   // South
    };

    void Start()
    {
        animator = GetComponent<Animator>();
        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);

        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();
    }

    void Update()
    {
        if (isWalking)
        {
            Walk();
        }
        else
        {
            Wait();
        }
    }

    private void Walk()
    {
        animator.SetBool("isRunning", true);
        walkCounter -= Time.deltaTime;

        transform.localRotation = Directions[walkDirection];
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (walkCounter <= 0)
        {
            StopWalking();
        }
    }

    private void Wait()
    {
        waitCounter -= Time.deltaTime;

        if (waitCounter <= 0)
        {
            ChooseDirection();
        }
    }

    private void StopWalking()
    {
        isWalking = false;
        stopPosition = transform.position; // Update stop position
        transform.position = stopPosition; // Make sure the position is exactly at the stop point
        animator.SetBool("isRunning", false);
        waitCounter = waitTime;
    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, Directions.Length);
        isWalking = true;
        walkCounter = walkTime;
    }
}
