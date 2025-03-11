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
    public Button heapButton;

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject hubMenu;
    [SerializeField] private GameObject workshopUI;
    [SerializeField] private GameObject shopUI;

    private MusicPlayer musicPlayer;

    void Start()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
        if (InventoryManager.Instance.isFirstTime)
            isMainMenuOpen = true;
        else
        {
            isMainMenuOpen = false;
            OpenHub();
        }
        musicPlayer = MusicPlayer.Instance;
        sfxPlayer = SFXPlayer.Instance;
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

    public void OpenHub()
    {
        sfxPlayer.ButtonClick();
        mainMenu.SetActive(false);
        isMainMenuOpen = false;
        hubMenu.SetActive(true);
        if (InventoryManager.Instance.IsFullyEquipped)
        {
            battleButton.interactable = true;
            heapButton.interactable = true;
        }
        else
        {
            battleButton.interactable = false;
            heapButton.interactable = false;
        }
    }

    public void CloseHub()
    {
        sfxPlayer.ButtonClick();
        mainMenu.SetActive(true);
        isMainMenuOpen = true;
        hubMenu.SetActive(false);
    }

    public void OpenBattle()
    {
        sfxPlayer.ButtonClick();
        hubMenu.SetActive(false);
        //battleUI.SetActive(true);
        //battleUI.GetComponent<BattleUI>().ResetBattle();
        musicPlayer.Battle();
        SceneManager.LoadScene("Arena");
        InventoryManager.Instance.isFirstTime = false;
    }

    public void CloseBattle()
    {
        sfxPlayer.ButtonClick();
        hubMenu.SetActive(true);
        musicPlayer.MainMenu();
    }

    public void OpenWorkshop()
    {
        sfxPlayer.ButtonClick();
        hubMenu.SetActive(false);
        workshopUI.SetActive(true);
        musicPlayer.Workshop();
    }

    public void CloseWorkshop()
    {
        sfxPlayer.ButtonClick();
        hubMenu.SetActive(true);
        workshopUI.SetActive(false);
        musicPlayer.MainMenu();
        if (InventoryManager.Instance.IsFullyEquipped)
        {
            battleButton.interactable = true;
            heapButton.interactable = true;
        }
        else
        {
            battleButton.interactable = false;
            heapButton.interactable = false;
        }

    }

    public void OpenHeap()
    {
        sfxPlayer.ButtonClick();
        hubMenu.SetActive(false);
        musicPlayer.HeapBattle();
        SceneManager.LoadScene("Heap");
        InventoryManager.Instance.isFirstTime = false;
    }

    public void CloseHeap()
    {
        sfxPlayer.ButtonClick();
        hubMenu.SetActive(true);
        musicPlayer.MainMenu();
        if (InventoryManager.Instance.IsFullyEquipped)
        {
            battleButton.interactable = true;
            heapButton.interactable = true;
        }
        else
        {
            battleButton.interactable = false;
            heapButton.interactable = false;
        }
    }

    public void OpenShop()
    {
        sfxPlayer.ButtonClick();
        hubMenu.SetActive(false);
        shopUI.SetActive(true);
        musicPlayer.Shop();
    }

    public void CloseShop()
    {
        sfxPlayer.ButtonClick();
        hubMenu.SetActive(true);
        shopUI.SetActive(false);
        musicPlayer.MainMenu();
        if (InventoryManager.Instance.IsFullyEquipped)
        {
            battleButton.interactable = true;
            heapButton.interactable = true;
        }
        else
        {
            battleButton.interactable = false;
            heapButton.interactable = false;
        }
    }

    public void QuitGame()
    {
        sfxPlayer.ButtonClick();
        Application.Quit();
    }
}
