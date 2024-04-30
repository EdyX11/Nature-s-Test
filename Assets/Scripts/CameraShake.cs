using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
        [Header("Earthquake Parameters")]
        [SerializeField] private float startingShakeAngle = 0.8f;
        [SerializeField] private float decreasePercentage = 0.5f;
        [SerializeField] private float shakeSpeed = 50;
        [SerializeField] private int numberOfShakes = 10;


        [Header("Player Parameters")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] public bool playerInRange ;
        private Coroutine shakeCoroutine; // Reference to the shake coroutine

         [Header("Audio")]
         [SerializeField] private AudioSource earthquakeChannel;
         [SerializeField] private AudioClip earthquakeSound;
         [SerializeField] private bool isPlaying;

    public GameObject vulcanoActivate;
    //public GameObject vulcanoActivateFallRocks;
    private void Start()
        {
            playerCamera = Camera.main; // Ensure the camera is assigned
            if (playerCamera == null)
            {
                Debug.LogError("No camera tagged as Main Camera found in the scene.");
            }
        }



    private IEnumerator ShakeCamera()
    {
        Quaternion originalRot = playerCamera.transform.localRotation;
        int shake = numberOfShakes;
        float shakeAngle = startingShakeAngle; // Consider renaming to `startingShakeAngle`

        while (shake > 0)
        {
            float timer = (Time.time * shakeSpeed) % (2 * Mathf.PI);
            Quaternion shakeRot = Quaternion.Euler(0, 0, Mathf.Sin(timer) * shakeAngle); // Shake on Z-axis
            playerCamera.transform.localRotation = originalRot * shakeRot;

            if (timer > Mathf.PI * 2)
            {
                shakeAngle *= decreasePercentage;
                shake--;
            }
            yield return null;
        }
        playerCamera.transform.localRotation = originalRot;
    }


   
        // Example trigger to start the shake.
        private void Update()
        {
            if (playerInRange)
            {
                if (shakeCoroutine == null) // Start the coroutine only if it's not already running
                {
                    StartEarthQuake();
                    shakeCoroutine = StartCoroutine(ShakeCamera());
                    vulcanoActivate.SetActive(true);
                    //vulcanoActivateFallRocks.SetActive(true);
                }
            }
            else
            {
                if (shakeCoroutine != null)
                {
                    StopEarthQuake();
                    StopCoroutine(shakeCoroutine); // Stop the coroutine if the player is out of range
                    shakeCoroutine = null; // Nullify the reference
                   // ResetCameraPosition(); // Optionally reset camera position to its original state
                }
            }
        }

    private void StartEarthQuake()
    {
        if (earthquakeChannel.isPlaying == false)
        {
            earthquakeChannel.clip = earthquakeSound;
            earthquakeChannel.loop = true;
            earthquakeChannel.Play();

        }

    }
    private void StopEarthQuake()
    {

        if (earthquakeChannel.isPlaying)
        {

            earthquakeChannel.Stop();

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

