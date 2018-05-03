using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class OptionsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public TMP_Dropdown ResDrop;

    public Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        ResDrop.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResDrop.AddOptions(options);
        ResDrop.value = currentResolutionIndex;
        ResDrop.RefreshShownValue();
        ResDrop.AddOptions(options);
    }

    // Resolution for the game
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Fullscreen option for the game
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Quality for the game
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Volume for the game
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);    
    }

    

  
}
