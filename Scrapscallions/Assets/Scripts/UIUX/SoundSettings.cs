using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SoundSettings /*Also Display Settings*/: MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] AudioMixer masterMixer;

    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Start()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
        SetMusicVolume(PlayerPrefs.GetFloat("SavedMusicVolume", 100));
        SetSFXVolume(PlayerPrefs.GetFloat("SavedSFXVolume", 100));

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    //master
    public void SetMasterVolume(float _value)
    {
        if (_value < 1)
        {
            _value = .001f;
        }

        RefreshMasterSlider(_value);
        PlayerPrefs.SetFloat("SavedMasterVolume", _value);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(_value / 100) * 20f);
    }

    public void SetMasterVolumeFromSlider()
    {
        SetMasterVolume(masterSlider.value);
    }

    public void RefreshMasterSlider(float _value)
    {
        masterSlider.value = _value;
    }

    //music
    public void SetMusicVolume(float _value)
    {
        if (_value < 1)
        {
            _value = .001f;
        }

        RefreshMusicSlider(_value);
        PlayerPrefs.SetFloat("SavedMusicVolume", _value);
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(_value / 100) * 20f);
    }

    public void SetMusicVolumeFromSlider()
    {
        SetMusicVolume(musicSlider.value);
    }

    public void RefreshMusicSlider(float _value)
    {
        musicSlider.value = _value;
    }

    //sfx
    public void SetSFXVolume(float _value)
    {
        if (_value < 1)
        {
            _value = .001f;
        }

        RefreshSFXSlider(_value);
        PlayerPrefs.SetFloat("SavedSFXVolume", _value);
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(_value / 100) * 20f);
    }

    public void SetSFXVolumeFromSlider()
    {
        SetSFXVolume(sfxSlider.value);
    }

    public void RefreshSFXSlider(float _value)
    {
        sfxSlider.value = _value;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
