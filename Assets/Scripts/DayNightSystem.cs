using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public Light directionalLight;

    public float dayDurationInSeconds = 24.0f; // duration of a full day
    public int currentHour;
    float currentTimeOfDay = 0.0f;


    public List<SkyboxTimeMapping> timeMappings;

    float blendedValue = 0.0f;  
    // Update is called once per frame
    void Update()
    {
        //calculate current time based on game time
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1; // stays between 0 and 1

        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);

        // update directional light
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));

        //update sky box material based on time of the day
        UpdateSkybox();
    }


    private void UpdateSkybox()
    {
        // find the appropriate skybox material for the current hour;

        Material currentSkybox = null;
        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentSkybox = mapping.skyboxMaterial;

                if(currentSkybox.shader != null)
                {
                    if(currentSkybox.shader.name == "Custom/SkyboxTransition")
                    {
                        blendedValue += Time.deltaTime; // game time
                        blendedValue = Mathf.Clamp01(blendedValue); //clamped between 0 and 1

                        currentSkybox.SetFloat("_TransitionFactor", blendedValue);

                    }
                    else
                    {
                        blendedValue = 0;

                    }
                }

                break;

            }

        }
        if (currentSkybox != null)
        {

            RenderSettings.skybox = currentSkybox;
        }

    }
}

[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour;// 0-23 
    public Material skyboxMaterial; // corresponding material for the actual hour

}
