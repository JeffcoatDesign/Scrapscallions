using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public List<bool> flags;

    public List<TutorialPopup> tutorialPopups;

    public void Awake()
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
