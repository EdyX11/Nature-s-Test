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


    // ---- PLAYER HYDRATION ----


    public float currentHydrationPercent;
    public float maxHydrationPercent;



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
    }

    


    void Update()
    {

        if (currentHealth > 0 && Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;

        }

        
    }
}
