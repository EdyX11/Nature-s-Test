using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigBearEnemy : MonoBehaviour
{
    public static BigBearEnemy Instance { get; set; }
    private NavMeshAgent navAgent;
    public string enemyName;
    public bool playerInRange;
    public bool IsAlreadyBeingAttacked;
    private bool isHitRecovering = false;
    [SerializeField] private float hitCooldown = 2.0f;


    [SerializeField] public  int HP = 100;
    private Animator animator;

    public bool isDead;
    //private bool isAsleep = false;
    [Header("Bear Sound")]

    public AudioClip bearWalking;
    public AudioClip bearChase;
    public AudioClip bearAttack;
    public AudioClip bearHurt;
    public AudioClip bearDeath;
    public AudioClip bearSleep;
    public AudioSource bearChannel;

    public DayNightSystem dayNightSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
   
    private void Start()
    {

        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        //DayNightSystem is also attached to an active GameObject
        dayNightSystem = FindObjectOfType<DayNightSystem>();

    }
    private void Update()
    {
        if (dayNightSystem != null)
        {
            int currentHour = dayNightSystem.currentHour;

            // Make the bear sleep between 23:00 and 07:00
            if ((currentHour >= 23 || currentHour < 7) && !animator.GetBool("isAsleep"))
            {
                animator.SetTrigger("BearSleep");
                animator.SetBool("isAsleep", true);
              //  bearChannel.PlayOneShot(bearSleep);
                navAgent.isStopped = true; // Stop movement while sleeping
            }
            // Wake the bear up only between 07:00 and 23:00
            else if (currentHour >= 7 && currentHour < 23 && animator.GetBool("isAsleep"))
            {
                animator.ResetTrigger("BearSleep");
                animator.SetBool("isAsleep", false);
                navAgent.isStopped = false; // Resume movement
            }
        }
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
                //bearChannel.PlayOneShot(bearDeath);
                Debug.Log($"{enemyName} DIED , SOUND TO BE FOUND");
            }
        }
        else
        {

            animator.SetTrigger("BearHit");
            //hurt sound
           
            isHitRecovering = true;
            bearChannel.PlayOneShot(bearHurt);
            StartCoroutine(ResetHitRecovery());
           
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
