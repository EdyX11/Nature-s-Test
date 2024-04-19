using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;
    public bool IsAlreadyBeingAttacked;
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
    [SerializeField] GameObject bloodPuddle;
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

        if (isDead)
        {
            Debug.Log("No further damage as already dead.");
            return; // Exit if already dead to avoid any processing
        }

        currentHealth -= damage;
       //bloodSplashParticles.Play();

        if (currentHealth <= 0)
        {
            if (!isDead) // Additional safeguard
            {
                Debug.Log("Animal is dying now.");
                isDead = true;
                PlayDyingSound();
                animator.SetTrigger("Death");
                Debug.Log("Animator State: " + animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));

                GetComponent<AnimalMovement>().enabled = false;
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero; // Ensure no ongoing movement
                    rb.isKinematic = true; // Stop responding to physics
                }
                StartCoroutine(PuddleDelay());
            }
        }
        else
        {
            if (thisAnimalType == AnimalType.Bear) // Check animal type before triggering hit animation
            {
                animator.SetTrigger("HitTrigger");  // Trigger the "being hit" animation only for the bear
            }
            PlayHitSound(); // Play hit sound if applicable
        }
    }



    IEnumerator PuddleDelay()
    {
        yield return new WaitForSeconds(1f);
        bloodPuddle.SetActive(true);
    }

    private void PlayDyingSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbitDie);
                break;
            case AnimalType.Bear:
                soundChannel.PlayOneShot(bearDie);  // UPDATED to play bear die sound
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
                soundChannel.PlayOneShot(bearHit);  // UPDATED to play bear hit sound
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
