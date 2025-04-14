using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    /*public bool _introDone = false;
    public bool _workshopDone = false;
    public bool _shopOpened = false;
    public bool _boughtPart = false;
    public bool _workshop2Done = false;
    public bool _botCustomized = false;
    public bool _scrapyardDone = false;
    public bool _shop2Done = false;
    public bool _arenaTried = false;
    public bool _tutorialDone = false;*/
    public List<bool> flags;

    /*public TutorialPopup introPopup;
    public TutorialPopup workshopPopup;
    public TutorialPopup workshopInteriorPopup;
    public TutorialPopup shopPopup;
    public TutorialPopup shopInteriorPopup;
    public TutorialPopup workshop2Popup;
    public TutorialPopup workshop2InteriorPopup;
    public TutorialPopup scrapyardPopup;
    public TutorialPopup shop2Popup;
    public TutorialPopup shop2InteriorPopup;
    public TutorialPopup arenaPopup;
    public TutorialPopup finishPopup;*/
    public List<TutorialPopup> tutorialPopups;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetPopups();
        }
        else
        {
            Instance.tutorialPopups = tutorialPopups;
            SetPopups();
            Destroy(gameObject);
        }
    }

    public void SetPopups()
    {
        for (int i = 0; i < tutorialPopups.Count; i++)
        {
            Debug.Log("Flag " + i + " is " + Instance.flags[i]);
            if (!Instance.flags[i])
            {
                Instance.tutorialPopups[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void SetFlag(int relevantFlag)
    {
        flags[relevantFlag] = true;
    }

    public void OpenTutorial(int popupToOpen)
    {
        tutorialPopups[popupToOpen].gameObject.SetActive(true);
    }
}
