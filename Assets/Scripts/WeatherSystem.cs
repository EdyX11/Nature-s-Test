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

    public bool isSpecialWeather;

    public AudioSource rainChannel;
    public AudioClip rainSound;
    public AudioSource volcanoChannel;
    public AudioClip volcanoSound;
    public bool isPlaying;
    public enum WeatherCondition
    {
        Sunny,
        Rainy,
        VolcanicEruption
    }

    private WeatherCondition currentWeather = WeatherCondition.Sunny;

    private void Start()
    {
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
    }

    private void StopVolcanicEruption()
    {
        if (volcanoChannel.isPlaying)
        {
            volcanoChannel.Stop();
        }
        volcanoRockSpawnerManager.SetActive(false);
        toActivateVolcanoRocks.SetActive(false);
    }
}
