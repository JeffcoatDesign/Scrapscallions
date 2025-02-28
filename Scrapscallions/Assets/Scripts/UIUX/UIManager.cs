using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Texture2D cursor;
    public bool isMainMenuOpen;

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
        mainMenu.SetActive(false);
        isMainMenuOpen = false;
        optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        mainMenu.SetActive(true);
        isMainMenuOpen = true;
        optionsMenu.SetActive(false);
    }

    public void OpenDebug()
    {
        mainMenu.SetActive(false);
        isMainMenuOpen = false;
        debugMenu.SetActive(true);
    }

    public void CloseDebug()
    {
        mainMenu.SetActive(true);
        isMainMenuOpen = true;
        debugMenu.SetActive(false);
    }

    public void OpenBattle()
    {
        debugMenu.SetActive(false);
        battleUI.SetActive(true);
        battleUI.GetComponent<BattleUI>().ResetBattle();
    }

    public void CloseBattle()
    {
        debugMenu.SetActive(true);
        battleUI.SetActive(false);
    }

    public void OpenWorkshop()
    {
        debugMenu.SetActive(false);
        workshopUI.SetActive(true);
    }

    public void CloseWorkshop()
    {
        debugMenu.SetActive(true);
        workshopUI.SetActive(false);
    }

    public void OpenHeap()
    {
        debugMenu.SetActive(false);
        heapUI.SetActive(true);
    }

    public void CloseHeap()
    {
        debugMenu.SetActive(true);
        heapUI.SetActive(false);
    }

    public void OpenShop()
    {
        debugMenu.SetActive(false);
        shopUI.SetActive(true);
    }

    public void CloseShop()
    {
        debugMenu.SetActive(true);
        shopUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
