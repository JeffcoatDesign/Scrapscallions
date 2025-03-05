using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public AudioSource sfxPlayer;
    [SerializeField, Header("SFX")] private AudioClip blaster;
    [SerializeField] private AudioClip buy;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip flameblaster;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip equipPart;
    [SerializeField] private AudioClip buzzsaw;
    [SerializeField] private AudioClip toasterDie;
    [SerializeField] private AudioClip trash;
    [SerializeField, Header("Hit Sounds")] private AudioClip[] hitSounds;

    void Start()
    {
        sfxPlayer = GetComponent<AudioSource>();
    }

    public void Blaster()
    {
        sfxPlayer.clip = blaster;
        sfxPlayer.Play();
    }

    public void Buy()
    {
        sfxPlayer.clip = buy;
        sfxPlayer.Play();
    }

    public void Death()
    {
        sfxPlayer.clip = death;
        sfxPlayer.Play();
    }

    public void Flameblaster()
    {
        sfxPlayer.clip = flameblaster;
        sfxPlayer.Play();
    }

    public void ButtonClick()
    {
        sfxPlayer.clip = buttonClick;
        sfxPlayer.Play();
    }

    public void EquipPart()
    {
        sfxPlayer.clip = equipPart;
        sfxPlayer.Play();
    }

    public void ToasterDie()
    {
        sfxPlayer.clip = toasterDie;
        sfxPlayer.Play();
    }

    public void Trash()
    {
        sfxPlayer.clip = trash;
        sfxPlayer.Play();
    }
    public void Hit()
    {
        sfxPlayer.clip = hitSounds[Random.Range(0, hitSounds.Length)];
        sfxPlayer.Play();
    }
}
