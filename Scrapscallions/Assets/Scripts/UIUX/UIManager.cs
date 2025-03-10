using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Texture2D cursor;
    public bool isMainMenuOpen;
    public SFXPlayer sfxPlayer;
    public Button battleButton;

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject debugMenu;
    [SerializeField] private GameObject workshopUI;
    [SerializeField] private GameObject shopUI;

    private MusicPlayer musicPlayer;
    void Start()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
        isMainMenuOpen = true;
        musicPlayer = MusicPlayer.Instance;
    }

    public void OpenOptions()
    {

        sfxPlayer.ButtonClick();
        mainMenu.SetActive(false);
        isMainMenuOpen = false;
        optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        sfxPlayer.ButtonClick();
        mainMenu.SetActive(true);
        isMainMenuOpen = true;
        optionsMenu.SetActive(false);
    }

    public void OpenDebug()
    {
        sfxPlayer.ButtonClick();
        mainMenu.SetActive(false);
        isMainMenuOpen = false;
        debugMenu.SetActive(true);
        if (InventoryManager.Instance.IsFullyEquipped)
            battleButton.interactable = true;
        else
            battleButton.interactable = false;
    }

    public void CloseDebug()
    {
        sfxPlayer.ButtonClick();
        mainMenu.SetActive(true);
        isMainMenuOpen = true;
        debugMenu.SetActive(false);
    }

    public void OpenBattle()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(false);
        //battleUI.SetActive(true);
        //battleUI.GetComponent<BattleUI>().ResetBattle();
        musicPlayer.Battle();
        SceneManager.LoadScene("Arena");
        InventoryManager.Instance.isFirstTime = false;
    }

    public void CloseBattle()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(true);
        musicPlayer.MainMenu();
    }

    public void OpenWorkshop()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(false);
        workshopUI.SetActive(true);
        musicPlayer.Workshop();
    }

    public void CloseWorkshop()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(true);
        workshopUI.SetActive(false);
        musicPlayer.MainMenu();
        if (InventoryManager.Instance.IsFullyEquipped)
            battleButton.interactable = true;
        else
            battleButton.interactable = false;

    }

    public void OpenHeap()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(false);
        musicPlayer.Heap();
        SceneManager.LoadScene("Heap");
        InventoryManager.Instance.isFirstTime = false;
    }

    public void CloseHeap()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(true);
        musicPlayer.MainMenu();
        if (InventoryManager.Instance.IsFullyEquipped)
            battleButton.interactable = true;
        else
            battleButton.interactable = false;
    }

    public void OpenShop()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(false);
        shopUI.SetActive(true);
        musicPlayer.Shop();
    }

    public void CloseShop()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(true);
        shopUI.SetActive(false);
        musicPlayer.MainMenu();
        if (InventoryManager.Instance.IsFullyEquipped)
            battleButton.interactable = true;
        else
            battleButton.interactable = false;
    }

    public void QuitGame()
    {
        sfxPlayer.ButtonClick();
        Application.Quit();
    }
}
