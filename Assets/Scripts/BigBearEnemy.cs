using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigBearEnemy : MonoBehaviour
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






    private void Start()
    {

        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

    }

    public void TakeDamageBear(int damageAmount)
    {
        Debug.Log($"{enemyName} took damage: {damageAmount}. Current Health: {HP}");

        if (isDead || isHitRecovering)
        {
            Debug.Log("No further damage as already dead or recovering from a hit.");
            return; // Exit if already dead or recovering
        }

        HP -= damageAmount;

        if (HP <= 0)
        {
            if (!isDead)
            {

               

                    animator.SetTrigger("BearDie");
               
                isDead = true;

                //dead sound
                //SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieDeath);
            }
        }
        else
        {

            animator.SetTrigger("BearHit");
            //hurt sound
           // SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
            isHitRecovering = true;

            StartCoroutine(ResetHitRecovery());
            if (HP <= 40)
            {
               
                animator.SetTrigger("BearBuff");

            }
        }

    }
    IEnumerator ResetHitRecovery()
    {
        yield return new WaitForSeconds(hitCooldown);  // Wait for the cooldown period
        isHitRecovering = false;  // Reset hit recovery state
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 7f); // attacking and stop attacking

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f); // detection (start chasing)

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f); // stop chasing

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
