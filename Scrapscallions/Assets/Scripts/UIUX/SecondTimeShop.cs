using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondTimeShop : MonoBehaviour
{
    public Button close;
    void OnEnable()
    {
        InventoryManager.Instance.isFirstTime = false;
        TutorialManager.Instance.SetFlag(GetComponentInParent<TutorialPopup>().relevantFlag);
        if (GetComponentInParent<TutorialPopup>().relevantFlag + 1 < TutorialManager.Instance.flags.Count)
        {
            TutorialManager.Instance.OpenTutorial(GetComponentInParent<TutorialPopup>().relevantFlag + 1);
        }
    }
}
