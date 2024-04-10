using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    
    
public class HydrationBar : MonoBehaviour
{
    private Slider slider;

    public Text hydrationCounter;

    public GameObject playerState;

    [Header("HYDRATION")]
     private float currentHydration, maxHydration;



    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {
        currentHydration = playerState.GetComponent<PlayerState>().currentHydrationPercent;
        maxHydration = playerState.GetComponent<PlayerState>().maxHydrationPercent;


        float fillValue = currentHydration / maxHydration; // slider runs between 1 and 0 , 0 empty , 1 full 
        slider.value = fillValue;

        hydrationCounter.text = currentHydration + "%"; // eg 1800/2000
    }

}
