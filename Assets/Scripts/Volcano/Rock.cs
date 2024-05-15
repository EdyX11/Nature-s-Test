using System;
using System.Collections;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] public int rockDamage = 100;  // Damage the rock can inflict
    private Action<Rock> _killAction;              // Action to perform when killing the rock
    [SerializeField] private bool canDamage = true;                 // Indicates if the rock can still damage the player
    private int bounceCount = 0;                   // Counter for the number of bounces
    [SerializeField] private int maxBounces;                        // Maximum number of bounces before deactivation, set randomly
    [SerializeField] private float timeout = 5.0f;                  // Timeout in seconds before forced deactivation
    public AudioClip impactSound; // Optional: Add an impact sound
    void Start()
    {
        Init(_killAction);  // Initialize maxBounces and start the timeout countdown
        StartCoroutine(DeactivationTimeout());  // Start the timeout coroutine
    }

    public void Init(Action<Rock> killAction)
    {
        _killAction = killAction;
        maxBounces = UnityEngine.Random.Range(1, 4);  // Randomly choose between 1 and 3 bounces (inclusive)
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && canDamage)
        {
            PlayerState.Instance.TakeDamage(rockDamage);
            //impactSound.PlayOneShot();
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            bounceCount++;
            if (bounceCount >= maxBounces)
            {
                canDamage = false;  // Stop damage after enough bounces
                StartCoroutine(DeactivateRock());  // Start the deactivation process
            }
        }
    }

    IEnumerator DeactivationTimeout()
    {
        yield return new WaitForSeconds(timeout);  // Wait for the timeout period
        if (_killAction != null)
        {
            _killAction(this);  // Perform the kill action if still active
        }
        ResetRock();  // Reset the rock properties
    }

    IEnumerator DeactivateRock()
    {
        yield return new WaitForSeconds(1);  // Short delay to allow for last bounce
        if (_killAction != null)
        {
            _killAction(this);  // Deactivate the rock
        }
        ResetRock();
    }

    private void ResetRock()
    {
        canDamage = true;
        bounceCount = 0;
        maxBounces = UnityEngine.Random.Range(1, 4);
    }
}
