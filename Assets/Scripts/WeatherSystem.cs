using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    [Header("Chance to Rain Per Season")]
    [Range(0f, 1f)]
    [SerializeField] private float chanceToRainSpring = 0.3f; // 30%
    [Range(0f, 1f)]
    [SerializeField] private float chanceToRainSummer = 0.1f; // 10%
    [Range(0f, 1f)]
    [SerializeField] private float chanceToRainAutumn = 0.4f; // 40%
    [Range(0f, 1f)]
    [SerializeField] private float chanceToRainWinter = 0.7f; // 70%
    [Header("Chance to Erupt")]
    [Range(0f, 1f)]
    [SerializeField] private float chanceToErupt = 0.2f; // 20% chance of volcanic eruption


    [Header("Rain Effects")]
    [SerializeField] private GameObject rainEffect;
    [SerializeField] private Material rainSkyBox;


    [Header("Volcano Eruption effects")]
    [SerializeField] private GameObject volcanoRockSpawnerManager;
    [SerializeField] private Material volcanoEruptionSkyBox;
    [SerializeField] private GameObject toActivateVolcanoRocks;

    [Header("Earthquake Parameters")]
    [SerializeField] private float startingShakeAngle = 0.8f;
    [SerializeField] private float decreasePercentage = 0.5f;
    [SerializeField] private float shakeSpeed = 50;
    [SerializeField] private int numberOfShakes = 10;

    public bool isSpecialWeather;

    [Header("Player Parameters")]
    [SerializeField] private Camera playerCamera;

    [Header("Audio")]
    [SerializeField] private AudioSource earthquakeChannel;
    [SerializeField] private AudioClip earthquakeSound;
    [SerializeField] private AudioSource rainChannel;
    [SerializeField] private AudioClip rainSound;
    [SerializeField] private AudioSource volcanoChannel;
    [SerializeField] private AudioClip volcanoSound;


    private Coroutine shakeCoroutine;
    private WeatherCondition currentWeather = WeatherCondition.Sunny;
    public bool isPlaying;


    public enum WeatherCondition
    {
        Sunny,
        Rainy,
        VolcanicEruption
    }



    private void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("No camera tagged as Main Camera found in the scene.");
        }

        TimeManager.Instance.OnDayPass.AddListener(GenerateRandomWeather);
    }

    private void GenerateRandomWeather()
    {
        TimeManager.Season currentSeason = TimeManager.Instance.currentSeason;

        float chanceToRain = 0f;
        switch (currentSeason)
        {
            case TimeManager.Season.Spring:
                chanceToRain = chanceToRainSpring;
                break;
            case TimeManager.Season.Summer:
                chanceToRain = chanceToRainSummer;
                break;
            case TimeManager.Season.Autumn:
                chanceToRain = chanceToRainAutumn;
                break;
            case TimeManager.Season.Winter:
                chanceToRain = chanceToRainWinter;
                break;
        }

        float randomValue = Random.value; // Generate a single random value for decision

        // Check if it should rain
        if (randomValue <= chanceToRain)
        {
            currentWeather = WeatherCondition.Rainy;
            Invoke("StartRain", 1f);
        }
        else if (randomValue <= chanceToRain + chanceToErupt) // Check for volcanic eruption
        {
            currentWeather = WeatherCondition.VolcanicEruption;
            Invoke("StartVolcanicEruption", 1f);
            Invoke("StopVolcanicEruption", 30f);
        }
        else
        {
            currentWeather = WeatherCondition.Sunny;
            StopRain();
            StopVolcanicEruption();
        }
    }
    private IEnumerator ShakeCamera()
    {
        Quaternion originalRot = playerCamera.transform.localRotation;
        int shakeCount = numberOfShakes;
        float shakeAngle = startingShakeAngle;

        while (shakeCount > 0)
        {
            float timer = (Time.time * shakeSpeed) % (2 * Mathf.PI);
            Quaternion shakeRot = Quaternion.Euler(0, 0, Mathf.Sin(timer) * shakeAngle);
            playerCamera.transform.localRotation = originalRot * shakeRot;

            if (timer > Mathf.PI * 2)
            {
                shakeAngle *= decreasePercentage;
                shakeCount--;
            }
            yield return null;
        }
        playerCamera.transform.localRotation = originalRot;
    }

    private void StartRain()
    {
        if (!rainChannel.isPlaying)
        {
            rainChannel.clip = rainSound;
            rainChannel.loop = true;
            rainChannel.Play();
        }

        RenderSettings.skybox = rainSkyBox;
        rainEffect.SetActive(true);
    }

    private void StopRain()
    {
        if (rainChannel.isPlaying)
        {
            rainChannel.Stop();
        }

        rainEffect.SetActive(false);
    }

    private void StartVolcanicEruption()
    {
        if (!volcanoChannel.isPlaying)
        {
            volcanoChannel.clip = volcanoSound;
            volcanoChannel.loop = true;
            volcanoChannel.Play();
        }

        RenderSettings.skybox = volcanoEruptionSkyBox;
        volcanoRockSpawnerManager.SetActive(true);
        toActivateVolcanoRocks.SetActive(true);

        if (shakeCoroutine == null)
        {
            StartEarthquakeSound();
            shakeCoroutine = StartCoroutine(ShakeCamera());
        }
    }

    private void StopVolcanicEruption()
    {
        if (volcanoChannel.isPlaying)
        {
            volcanoChannel.Stop();
        }

        volcanoRockSpawnerManager.SetActive(false);
        toActivateVolcanoRocks.SetActive(false);

        if (shakeCoroutine != null)
        {
            StopEarthquakeSound();
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }
    }
    private void StartEarthquakeSound()
    {
        if (!earthquakeChannel.isPlaying)
        {
            earthquakeChannel.clip = earthquakeSound;
            earthquakeChannel.loop = true;
            earthquakeChannel.Play();
        }
    }

    private void StopEarthquakeSound()
    {
        if (earthquakeChannel.isPlaying)
        {
            earthquakeChannel.Stop();
        }
    }
}

