using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider slider;

    public Text healthCounter;

    public GameObject playerState;

    [Header("HEALTH")]
   private float currentHealth, maxHealth;



    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;


        float fillValue = currentHealth / maxHealth; // slider runs between 1 and 0 , 0 empty , 1 full 
        slider.value = fillValue;

        healthCounter.text = currentHealth + "/" + maxHealth; // eg 80/100
    }
}
