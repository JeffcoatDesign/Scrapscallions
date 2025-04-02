using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Texture2D cursor;
    public bool isMainMenuOpen;
    public SFXPlayer sfxPlayer;
    public Button battleButton;
    public Button heapButton;
    public Button continueButton;
    public Canvas canvas;

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject hubMenu;
    [SerializeField] private GameObject workshopUI;
    [SerializeField] private GameObject workshopIntroUI;
    [SerializeField] private GameObject workshopInteriorUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject shopIntroUI;
    [SerializeField] private GameObject shopInteriorUI;
    [Header("NPCs")]
    [SerializeField] private NPCIdle cache;
    [SerializeField] private NPCIdle mouse;

    private MusicPlayer musicPlayer;

    public static UIManager Instance;

    void OnEnable()
    {
        Instance = this;
        canvas = GetComponent<Canvas>();
    }

    void Start()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
        if (InventoryManager.Instance.isFirstTime)
        {
            isMainMenuOpen = true;
            continueButton.interactable = false;
        }
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
        /*workshopUI.SetActive(true);
        workshopInteriorUI.SetActive(true);
        workshopUI.SetActive(false);
        workshopInteriorUI.SetActive(false);*/
        if (!continueButton.interactable)
        {
            continueButton.interactable = true;
            continueButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
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

    public void NewGame()
    {
        if(TutorialPopup.Instance != null)
            TutorialPopup.Instance.gameObject.SetActive(true);
        if(!InventoryManager.Instance.isFirstTime)
            InventoryManager.Instance.NewGame();
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
        workshopIntroUI.SetActive(true);
        mouse.time = 0.5f;
        musicPlayer.Workshop();
    }

    public void OpenWorkshopInterior()
    {
        sfxPlayer.ButtonClick();
        workshopInteriorUI.SetActive(true);
        workshopIntroUI.SetActive(false);
    }

    public void CloseWorkshop()
    {
        sfxPlayer.ButtonClick();
        hubMenu.SetActive(true);
        workshopInteriorUI.SetActive(false);
        workshopUI.SetActive(false);
        workshopIntroUI.SetActive(true);
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
        shopIntroUI.SetActive(true);
        cache.time = 0.5f;
        musicPlayer.Shop();
    }

    public void OpenShopInterior()
    {
        sfxPlayer.ButtonClick();
        shopInteriorUI.SetActive(true);
        shopIntroUI.SetActive(false);
    }

    public void CloseShop()
    {
        sfxPlayer.ButtonClick();
        hubMenu.SetActive(true);
        shopInteriorUI.SetActive(false);
        shopUI.SetActive(false);
        shopIntroUI.SetActive(true);
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
