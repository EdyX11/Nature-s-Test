using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour
{

    public BearClaw bearClaw;
    public int bearDamage;
    
    private void Start()
    {
        bearClaw.damage = bearDamage;
    }

}
