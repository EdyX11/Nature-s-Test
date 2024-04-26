using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance { get; private set; }
    [Header("Resource Status")]
    public float resourceHealth;
    public float resourceMaxHealth;


   // public GameObject bloodSprayEffect;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // If there's already an instance and it's not this one, destroy this one
            Destroy(gameObject);
        }
        else
        {
            // This is the first instance or the same as the existing one, set it as the Instance
            Instance = this;
            // Make sure this object persists when loading a new scene
           // DontDestroyOnLoad(gameObject);
        }
    }
}
