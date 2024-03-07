using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{


    public static PlayerState Instance { get;  set; }

    // ---- PLAYER HEALTH ---- 
    public float currentHealth;
    public float maxHealth;


    // ---- PLAYER CALORIES ----


    public float currentCalories;
    public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;

    // ---- PLAYER HYDRATION ----


    public float currentHydrationPercent;
    public float maxHydrationPercent;

    //public bool isHydrationActive;

    private void Awake() // created in every singleton
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }




    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        StartCoroutine(decreaseHydration());


    }
    
    IEnumerator decreaseHydration()
    {
        while(true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(10); // change hydration here


        }

    }
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position; // last position is current position

        if(distanceTravelled >= 5) // when travelling distance of value 5 lose 1 calorie
        {
            distanceTravelled = 0;
            currentCalories -= 1; // change calorie loss here 
        }



        if (currentHealth > 0 && Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10; // here lose health
        }

        
    }
}
