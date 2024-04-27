using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerState : MonoBehaviour
{


    public static PlayerState Instance { get;  set; }
    


    [Header("---- PLAYER HEALTH ---- ")]
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth;
    public Action<float> OnTakeDamage;
    public Action<float> OnDamage;
    public Action<float> OnHeal;


    [Header("---- PLAYER CALORIES ----")]

    [SerializeField] public float currentCalories;
    [SerializeField] public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;
   

    [Header("---- PLAYER HYDRATION ----")]

    [SerializeField] public float currentHydrationPercent;
    [SerializeField] public float maxHydrationPercent;
   // [SerializeField] public GameObject bloodyScreen;

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
        LoseCalories();


        
    }

    private void OnEnable()
    {


        OnTakeDamage += ApplyDamage;

    }

    private void OnDisable()
    {

        OnTakeDamage -= ApplyDamage;
    }

    #region ---SETTERS----

    public void setHealth(float newHealth)
    {



        currentHealth = newHealth;
    }

    public void setCalories(float newCalories)
    {

        currentCalories = newCalories;


    }

    public void setHydration(float newHydraytion)
    {

        currentHydrationPercent = newHydraytion;

    }
    #endregion

    public void TakeDamage(float damageAmount)
    {
        if (OnTakeDamage != null)
        {
            OnTakeDamage(damageAmount);
        }
        else
        {
            // Default damage handling if no specific action is subscribed
            ApplyDamage(damageAmount);
        }
    }

    private void LoseCalories()
    {

        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position; // last position is current position

        if (distanceTravelled >= 5) // when travelling distance of value 5 lose 1 calorie
        {
            distanceTravelled = 0;
            currentCalories -= 1; // change calorie loss here 
        }
    }
    private void ApplyDamage(float dmg)
    {

        print("taking damage");
      // StartCoroutine(BloodyScreenEffect());
        currentHealth -= dmg;
        OnDamage?.Invoke(currentHealth);// short way to if OnDamage == null or not do something
        if (currentHealth <= 0)
        {
            KillPlayer();

        }
        else
        {

            print("player hit");
        }
            
       
    }
   
    private void KillPlayer()
    {
        currentHealth = 0;
        print("player dead");
        //go to main menu 
        // game over
        //dying animation

    }
   


}
