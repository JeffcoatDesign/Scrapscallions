using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Texture2D cursor;

    [Header("Menus")]
    [SerializeField]
    private GameObject mainMenu, optionsMenu;
    void Start()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    void CloseOptions()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
