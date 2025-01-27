using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeapUI : MonoBehaviour
{
    public GameObject arrow;
    Animator arrowAnimator;
    public GameObject check1, check2, check3, check4, check5, youWin;

    void Start()
    {
        arrowAnimator = arrow.GetComponent<Animator>();
    }

    public void WinLevel()
    {
        if (!check1.activeSelf)
        {
            check1.SetActive(true);
            arrowAnimator.Play("ArrowMove1");
        }
        else if (!check2.activeSelf)
        {
            check2.SetActive(true);
            arrowAnimator.Play("ArrowMove2");
        }
        else if (!check3.activeSelf)
        {
            check3.SetActive(true);
            arrowAnimator.Play("ArrowMove3");
        }
        else if (!check4.activeSelf)
        {
            check4.SetActive(true);
            arrowAnimator.Play("ArrowMove4");
        }
        else if (!check5.activeSelf)
        {
            check5.SetActive(true);
            youWin.SetActive(true);
        }
    }
}
