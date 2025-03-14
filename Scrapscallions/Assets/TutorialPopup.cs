using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialPages;
    private int currentPage;
    public Button hubCloseButton;

    void Start()
    {
        if (!InventoryManager.Instance.isFirstTime)
        {
            hubCloseButton.interactable = true;
            Destroy(gameObject);
        }
    }

    public void NextPage()
    {
        tutorialPages[currentPage].gameObject.SetActive(false);
        currentPage++;
        if (currentPage >= tutorialPages.Length)
        {
            gameObject.SetActive(false);
            hubCloseButton.interactable = true;
        }
        else
            tutorialPages[currentPage].gameObject.SetActive(true);
    }

    public void PreviousPage()
    {
        tutorialPages[currentPage].gameObject.SetActive(false);
        currentPage--;
        tutorialPages[currentPage].gameObject.SetActive(true);
    }

    public void Close()
    {
        hubCloseButton.interactable = true;
        gameObject.SetActive(false);
    }
}
