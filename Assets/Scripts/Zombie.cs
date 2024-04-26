using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip enemyHit;
    [SerializeField] AudioClip enemyDie;

    public ZombieHand zombieHand;
    public int zombieDamage;
    private void Start()
    {
        zombieHand.damage = zombieDamage;


    }


   
}
