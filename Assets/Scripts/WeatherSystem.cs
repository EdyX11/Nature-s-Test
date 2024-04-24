using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{   
    [Range(0f, 1f)] 
    public float chanceToRainSpring = 0.3f; // 30%
    [Range(0f, 1f)]
    public float chanceToRainSummer = 0.1f; // 10%
    [Range(0f, 1f)]
    public float chanceToRainAutumn = 0.4f; // 40%
    [Range(0f, 1f)]
    public float chanceToRainWinter= 0.7f; // 70%


    public GameObject rainEffect;
    public Material rainSkyBox;

    public bool isSpecialWeather;

    public AudioSource rainChannel;
    public AudioClip rainSound;
    public bool isPlaying;


    public enum WeatherConditon
    {

        Sunny,
        Rainy
    }

    private WeatherConditon currentWeather = WeatherConditon.Sunny;


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

        if(Random.value <= chanceToRain)
        {
            currentWeather = WeatherConditon.Rainy;
            isSpecialWeather = true;

            Invoke("StartRain", 1f);


        }
        else
        {

            currentWeather = WeatherConditon.Sunny;
            isSpecialWeather = false;

            StopRain();
        }

    }

    private void StartRain()
    {
        if(rainChannel.isPlaying == false)
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



}
