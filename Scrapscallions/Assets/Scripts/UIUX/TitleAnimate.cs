using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleAnimate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.Play("Appear");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.Play("Disappear");
    }
}
