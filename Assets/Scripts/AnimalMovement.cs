using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    private Animator animator;
    public AnimalType type;
    public AnimalProperties properties;

    private Vector3 stopPosition;
    private float walkCounter;
    private float waitCounter;
    private int walkDirection;

    public bool isWalking;
    private static readonly Quaternion[] Directions = {
        Quaternion.Euler(0f, 0f, 0f),    // North
        Quaternion.Euler(0f, 90f, 0f),   // East
        Quaternion.Euler(0f, -90f, 0f),  // West
        Quaternion.Euler(0f, 180f, 0f)   // South
    };

    public enum AnimalType
    {
        Rabbit,
        Bear,
    }

    [System.Serializable]
    public class AnimalProperties
    {
        public float moveSpeed = 0.2f;
        public float walkTime = 5f;
        public float waitTime = 5f;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        SetPropertiesByType();

        waitCounter = properties.waitTime;
        walkCounter = properties.walkTime;

        ChooseDirection();
    }

    void Update()
    {
        // Assuming there's an `isDead` variable in this script or another component on the same GameObject
        // For example, if the `isDead` property is part of an 'Animal' component:
        Animal animalScript = GetComponent<Animal>();

        if (animalScript != null && animalScript.isDead)
        {
            // Optionally, ensure the walking animation is not playing
            animator.SetBool("WalkForward", false);
            return; // Exit the update loop if the animal is dead
        }

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
        animator.SetBool("WalkForward", true);
        walkCounter -= Time.deltaTime;

        transform.localRotation = Directions[walkDirection];
        transform.position += transform.forward * properties.moveSpeed * Time.deltaTime;

        if (walkCounter <= 0)
        {
            StopWalking();
        }
    }

    private void Wait()
    {
        animator.SetBool("WalkForward", false);
        waitCounter -= Time.deltaTime;

        if (waitCounter <= 0)
        {
            ChooseDirection();
        }
    }

    private void StopWalking()
    {
        isWalking = false;
        stopPosition = transform.position;
        transform.position = stopPosition;
        waitCounter = properties.waitTime;
    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, Directions.Length);
        isWalking = true;
        walkCounter = properties.walkTime;
    }

    private void SetPropertiesByType()
    {
        switch (type)
        {
            case AnimalType.Rabbit:
                properties = new AnimalProperties { moveSpeed = 0.3f, walkTime = 3.2f, waitTime = 2f };
                break;
            case AnimalType.Bear:
                properties = new AnimalProperties { moveSpeed = 0.5f, walkTime = 8f, waitTime = 10f };
                break;
        }
    }
}
