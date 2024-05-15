using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;
    public bool IsAlreadyBeingAttacked;
    private bool isHitRecovering = false;  // Flag to indicate hit recovery
    private float hitCooldown = 2.0f;
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip rabbitHit;
    [SerializeField] AudioClip rabbitDie;
    [SerializeField] AudioClip bearHit;   // ADDED: Bear hit sound
    [SerializeField] AudioClip bearDie;   // ADDED: Bear die sound

    private Animator animator;
    public bool isDead;

   // [SerializeField] ParticleSystem bloodSplashParticles;
    [SerializeField] public GameObject bloodPuddle;
    enum AnimalType
    {
        Rabbit,
        Bear,
    }

    [SerializeField] AnimalType thisAnimalType;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    

    public void TakeDamage(int damage)
    {
        Debug.Log($"{animalName} took damage: {damage}. Current Health: {currentHealth}");

        if (isDead || isHitRecovering)
        {
            Debug.Log("No further damage as already dead or recovering from a hit.");
            return; // Exit if already dead or recovering
        }

        currentHealth -= damage;
        //bloodSplashParticles.Play();

        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                Debug.Log("Animal is dying now.");
                isDead = true;
                PlayDyingSound();
                animator.SetTrigger("Death");
                Debug.Log("Animator State: " + animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));

                GetComponent<AnimalMovement>().enabled = false;
                // to make sure the kinematic body stops 
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.isKinematic = true;
                }
                StartCoroutine(PuddleDelay());
            }
        }
        else
        {
            if (thisAnimalType == AnimalType.Bear)
            {
                animator.SetTrigger("HitTrigger");
                isHitRecovering = true;
                StartCoroutine(ResetHitRecovery());
            }
            PlayHitSound();
        }
    }

    IEnumerator ResetHitRecovery()
    {
        yield return new WaitForSeconds(hitCooldown);  // Wait for the cooldown period
        isHitRecovering = false;  // Reset hit recovery state
    }




    IEnumerator PuddleDelay()
    {
        yield return new WaitForSeconds(2f);
        bloodPuddle.SetActive(true);
        yield return new WaitForSeconds(10f);
        bloodPuddle.SetActive(false);

    }

    private void PlayDyingSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbitDie);
                break;
            case AnimalType.Bear:
                soundChannel.PlayOneShot(bearDie);  
                break;
            default:
                break;
        }
    }

    private void PlayHitSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbitHit);
                break;
            case AnimalType.Bear:
                soundChannel.PlayOneShot(bearHit);  
                break;
            default:
                break;
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
