using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CaloriesBar : MonoBehaviour
{
    private Slider slider;

    public Text caloriesCounter;

    public GameObject playerState;

    [Header("Calories")]
    [SerializeField] private float currentCalories, maxCalories;



   private void Awake()
    {
        slider = GetComponent<Slider>();
    }


    private void Update()
    {
        currentCalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxCalories = playerState.GetComponent<PlayerState>().maxCalories;


        float fillValue = currentCalories / maxCalories; // slider runs between 1 and 0 , 0 empty , 1 full 
        slider.value = fillValue;

        caloriesCounter.text = currentCalories + "/" + maxCalories; // eg 1800/2000
    }
}
