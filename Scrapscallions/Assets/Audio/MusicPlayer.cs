using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;
    public AudioSource musicPlayer;
    [SerializeField, Header("Music Tracks")] private AudioClip mainMenu;
    [SerializeField] private AudioClip battle;
    [SerializeField] private AudioClip workshop;
    [SerializeField] private AudioClip shop;
    [SerializeField] private AudioClip heap;
    [SerializeField] private AudioClip heapBattle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
    }

    public void MainMenu()
    {
        musicPlayer.clip = mainMenu;
        musicPlayer.Play();
    }

    public void Battle()
    {
        musicPlayer.clip = battle;
        musicPlayer.Play();
    }

    public void Workshop()
    {
        musicPlayer.clip = workshop;
        musicPlayer.Play();
    }

    public void Shop()
    {
        musicPlayer.clip = shop;
        musicPlayer.Play();
    }

    public void Heap()
    {
        musicPlayer.clip = heap;
        musicPlayer.Play();
    }

    public void HeapBattle()
    {
        musicPlayer.clip = heapBattle;
        musicPlayer.Play();
    }
}
