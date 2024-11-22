using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Most code in this script is credited to Brackey's tutorial
// https://www.youtube.com/watch?v=YOaYQrN1oYQ
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    
    public AudioMixer audioMixer;

    public TMPro.TMP_Dropdown resolutionDropdown;
    
    private Resolution[] _resolutions;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        // loop through array to convert options to strings to put in our dropdown menu
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetViolenceVolume(float volume)
    {
        audioMixer.SetFloat("ViolenceVolume", volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    
    public void SetAmbienceVolume(float volume)
    {
        audioMixer.SetFloat("AmbienceVolume", volume);
        
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
