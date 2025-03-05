using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Texture2D cursor;
    public bool isMainMenuOpen;
    public MusicPlayer musicPlayer;
    public SFXPlayer sfxPlayer;
    public Button battleButton;

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject debugMenu;
    [SerializeField] private GameObject battleUI;
    [SerializeField] private GameObject workshopUI;
    [SerializeField] private GameObject heapUI;
    [SerializeField] private GameObject shopUI;

    void Start()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
        isMainMenuOpen = true;
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
        InventoryManager invMan = FindAnyObjectByType<InventoryManager>();
        if (invMan.equippedBody != null && invMan.equippedHead != null && invMan.equippedLArm != null && invMan.equippedRArm != null && invMan.equippedLegs != null)
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
        battleUI.SetActive(true);
        battleUI.GetComponent<BattleUI>().ResetBattle();
        musicPlayer.Battle();
    }

    public void CloseBattle()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(true);
        battleUI.SetActive(false);
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
        InventoryManager invMan = FindAnyObjectByType<InventoryManager>();
        if (invMan.equippedBody != null && invMan.equippedHead != null && invMan.equippedLArm != null && invMan.equippedRArm != null && invMan.equippedLegs != null)
            battleButton.interactable = true;
        else
            battleButton.interactable = false;

    }

    public void OpenHeap()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(false);
        heapUI.SetActive(true);
        musicPlayer.Heap();
    }

    public void CloseHeap()
    {
        sfxPlayer.ButtonClick();
        debugMenu.SetActive(true);
        heapUI.SetActive(false);
        musicPlayer.MainMenu();
        InventoryManager invMan = FindAnyObjectByType<InventoryManager>();
        if (invMan.equippedBody != null && invMan.equippedHead != null && invMan.equippedLArm != null && invMan.equippedRArm != null && invMan.equippedLegs != null)
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
        InventoryManager invMan = FindAnyObjectByType<InventoryManager>();
        if (invMan.equippedBody != null && invMan.equippedHead != null && invMan.equippedLArm != null && invMan.equippedRArm != null && invMan.equippedLegs != null)
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
