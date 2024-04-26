using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNPC : MonoBehaviour
{
    private NavMeshAgent navAgent;
    public string enemyName;
    public bool playerInRange;
    public bool IsAlreadyBeingAttacked;
    private bool isHitRecovering = false;
    [SerializeField] private float hitCooldown = 2.0f;

    
    [SerializeField] private int HP = 100;
    private Animator animator;
   
    public bool isDead;
    

    
    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip enemyHit;
    [SerializeField] AudioClip enemyDie;
   

  




    private void Start()
    {

        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }
    
    public void TakeDamage(int damageAmount)
    {

        HP-=damageAmount;

        if (HP <= 0)
        {
            if (!isDead)
            {
                isDead = true;
                int randomValue = Random.Range(0, 2); // 0 or 1
                if (randomValue == 0)
                {

                    animator.SetTrigger("DIE1");
                }
                else
                {

                    animator.SetTrigger("DIE2");
                }
            }
        }
        else
        {

            animator.SetTrigger("DAMAGE");
        }

    }

    private void Update()
    {
        if(navAgent.velocity.magnitude > 0.1f)// moving start =>start walk
        
        
        {

            animator.SetBool("isWalking",true);
   
        }
        else
        {
            animator.SetBool("isWalking", false);
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
