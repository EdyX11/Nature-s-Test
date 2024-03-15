using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHealthBar : MonoBehaviour
{
    private Slider slider;
    private float currentHealth, maxHealth;

    public GameObject globalState;



    private void Awake()
    {

        slider = GetComponent<Slider>();


    }

    private void Update()
    {
        // Check if globalState is not null and GlobalState component exists before accessing its members
        if (globalState != null && globalState.GetComponent<GlobalState>() != null)
        {
            currentHealth = globalState.GetComponent<GlobalState>().resourceHealth;
            maxHealth = globalState.GetComponent<GlobalState>().resourceMaxHealth;

            float fillValue = currentHealth / maxHealth; // slider runs between 1 and 0, 0 empty, 1 full 
            slider.value = fillValue;
        }
        else
        {
            Debug.LogWarning("globalState or GlobalState component is null in ResourceHealthBar.Update");
        }
    }

}
