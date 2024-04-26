using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNPC : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;
    public bool playerInRange;
    public bool isDead;
    public string enemyName;

    private bool isHitRecovering = false;
    private float hitCooldown = 2.0f;
    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip enemyHit;
    [SerializeField] AudioClip enemyDie;
    public bool IsAlreadyBeingAttacked;




    private void Start()
    {

        animator = GetComponent<Animator>();

    }
    
    public void TakeDamage(int damageAmount)
    {

        HP-=damageAmount;   

        if(HP <= 0 ) 
        {
            animator.SetTrigger("DIE");

        
        }
        else
        {

            animator.SetTrigger("DAMAGE");
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
