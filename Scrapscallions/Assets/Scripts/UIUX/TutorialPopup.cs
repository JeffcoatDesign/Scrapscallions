using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialPages;
    private int currentPage;
    public Button relevantButton;

    void Start()
    {
        currentPage = 0;
        tutorialPages[currentPage].gameObject.SetActive(true);
        if (relevantButton != null)
        {
            Button butt = Instantiate(relevantButton);
            butt.transform.SetParent(transform, false);
        }
    }

    public void NextPage()
    {
        tutorialPages[currentPage].gameObject.SetActive(false);
        currentPage++;
        if (currentPage >= tutorialPages.Length)
        {
            gameObject.SetActive(false);
            Debug.Log("Exceeded page count");
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
        InventoryManager.Instance.isFirstTime = false;
        gameObject.SetActive(false);
    }
}
