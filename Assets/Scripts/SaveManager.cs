using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }

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

        DontDestroyOnLoad(gameObject);
    }


    #region || ------------SETTINGS SECTION--------- || 



    #region || ------------Volume Settings --------- || 
    [System.Serializable]
    public class VolumeSettings
    {

        public float music;
        public float effects;   
        public float master; // all the volumes
    }

    public void SaveVolumeSettings(float music_Volume, float effects_Volume , float master_Volume)
    {
        VolumeSettings volumeSettings = new VolumeSettings()
        {

            music = music_Volume,
            effects = effects_Volume,
            master = master_Volume
        };
        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings)); // converting the class into a json using  SetString, json saved as a string into player prefs
        PlayerPrefs.Save();
        print("save to player prefs");
    }
   
    public VolumeSettings LoadVolumeSettings()
    {
        var settings = JsonUtility.FromJson <VolumeSettings>(PlayerPrefs.GetString("Volume")); // converting back to class 

        return settings;



    }
    #endregion

















    #endregion




}
