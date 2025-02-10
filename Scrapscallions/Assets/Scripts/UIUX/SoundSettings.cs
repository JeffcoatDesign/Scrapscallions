using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] AudioMixer masterMixer;

    private void Start()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
        SetMusicVolume(PlayerPrefs.GetFloat("SavedMusicVolume", 100));
        SetSFXVolume(PlayerPrefs.GetFloat("SavedSFXVolume", 100));
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
}
