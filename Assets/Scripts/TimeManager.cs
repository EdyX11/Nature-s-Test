using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }

    public UnityEvent OnDayPass = new UnityEvent();//day passed

    public enum Season
    {

      Spring,//0
      Summer,
      Autumn,
      Winter//4

    }


    public Season currentSeason = Season.Spring;

    private int daysPerSeason = 2;
    private int daysInCurrentSeason = 1;



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

    public int dayInGame = 1;
    public TextMeshProUGUI dayUI;

    private void Start()
    {
        UpdateUI();
    }

    public void TriggerNextDay()
    {

        dayInGame += 1;
        daysInCurrentSeason += 1;

        if(daysInCurrentSeason > daysPerSeason)
        {

            daysInCurrentSeason = 1;
            currentSeason = GetNextSeason();

        }

        UpdateUI();
        OnDayPass.Invoke();
    }

    private Season GetNextSeason()
    {

        int currentSeasonIndex = (int)currentSeason;// convert to int from enum, 0 (spring) 
        int nextSeasonIndex = (currentSeasonIndex+ 1) % 4; // add 1 , summer, mod 4 for when we reach>4 to reset back to 0 

        return (Season)nextSeasonIndex;
    }

    private void UpdateUI()
    {

        dayUI.text = $"Day:  {dayInGame}, {currentSeason}";
    }
}
