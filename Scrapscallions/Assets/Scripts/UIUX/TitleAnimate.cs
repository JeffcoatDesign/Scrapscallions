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

    void OnEnable()
    {
        if (animator != null)
        {
            animator.Play("Idle");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable)
            animator.Play("Appear");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable)
            animator.Play("Disappear");
    }
}
