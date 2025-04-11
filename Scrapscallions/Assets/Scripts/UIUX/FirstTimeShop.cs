using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstTimeShop : MonoBehaviour
{
    [SerializeField] Button close;
    public void OnEnable()
    {
        close.interactable = false;
    }
    public void EndTutorial()
    {
        GetComponentInParent<TutorialPopup>().Close();
        close.interactable = true;
        Destroy(this);
    }
}
