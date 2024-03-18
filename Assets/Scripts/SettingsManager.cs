using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static SaveManager;


public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; set; }

    public Slider masterSlider;
    public GameObject masterValue;

    public Slider musicSlider;
    public GameObject musicValue;

    public Slider effectsSlider;
    public GameObject effectsValue;

    public Button backBTN;

   

    private void Start()
    {

        backBTN.onClick.AddListener(() => 
        { 
        
            SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value );
            print("Saved to playerprefs");
        });


        StartCoroutine(LoadAndApplySettings());

    }
    private IEnumerator LoadAndApplySettings() //generic load and apply setttings
    {

        LoadAndSetVolume();
        //load graphic settings
        //load keybind
        //load refresh rate
        yield return new WaitForSeconds(0.1f);
    }
    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();
        masterSlider.value = volumeSettings.master;
        musicSlider.value = volumeSettings.music;
        effectsSlider.value = volumeSettings.effects;

        print("VOLUME SETTING LOADED");
    }
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

    private void Update()
    {

        masterValue.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicValue.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        effectsValue.GetComponent<TextMeshProUGUI>().text = "" + (effectsSlider.value) + "";
    }

}