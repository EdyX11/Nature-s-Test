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
    [SerializeField] public bool isDead;

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

        StartCoroutine(DecreaseHydration());


    }

    IEnumerator DecreaseHydration()
    {
        while (!isDead && currentHydrationPercent > 0)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(10);
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
        if (Vector3.Distance(playerBody.transform.position, lastPosition) >= 5)
        {
            distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
            lastPosition = playerBody.transform.position;
            currentCalories = Mathf.Max(0, currentCalories - 1);
        }
    }
    private void ApplyDamage(float dmg)
    {

        
        // StartCoroutine(BloodyScreenEffect());
        currentHealth = Mathf.Max(0, currentHealth - dmg);
        OnDamage?.Invoke(currentHealth);// short way to if OnDamage == null or not do something
        if (currentHealth <= 0)
        {
            KillPlayer();

        }
            
       
    }
   
    private void KillPlayer()
    {
       
        currentHealth = 0;
        print("player dead player state");
        //go to main menu 
        // game over
        //dying animation

    }
   


}
