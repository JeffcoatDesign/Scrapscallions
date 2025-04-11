using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialPages;
    public int currentPage;
    public Button relevantButton;
    public int relevantFlag;

    void Start()
    {
        if (TutorialManager.Instance.flags[relevantFlag])
        {
            gameObject.SetActive(false);
        }
        else
        {
            currentPage = 0;
            tutorialPages[currentPage].gameObject.SetActive(true);
            if (relevantButton != null)
            {
                Button butt = Instantiate(relevantButton);
                butt.transform.SetParent(transform, false);
                butt.onClick.AddListener(() =>
                {
                    Close();
                });
            }
        }
    }

    public void NextPage()
    {
        tutorialPages[currentPage].gameObject.SetActive(false);
        currentPage++;
        if (currentPage >= tutorialPages.Count)
        {
            Debug.Log("Exceeded page count");
            Close();
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
        TutorialManager.Instance.SetFlag(relevantFlag);
        if (relevantFlag + 1 < TutorialManager.Instance.flags.Count)
        {
            TutorialManager.Instance.OpenTutorial((relevantFlag + 1));
        }
        gameObject.SetActive(false);
    }
}
