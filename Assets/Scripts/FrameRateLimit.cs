using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLimit : MonoBehaviour
{
    public enum limits { 
    
        noLimit=0,
        limit30 = 30,
        limit60 = 60,
        limit120 = 120,
        limit180 = 180,
     
    }
    public limits limit;

    void Awake()

    {
        Application.targetFrameRate = (int)limit;
    }
}
