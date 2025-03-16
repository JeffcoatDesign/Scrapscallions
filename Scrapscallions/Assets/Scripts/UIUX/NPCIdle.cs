using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : MonoBehaviour
{
    [SerializeField] private Animator[] animators;
    public float time;
    public static NPCIdle Instance;

    private void OnEnable()
    {
        Instance = this;
        Reset();
        foreach(Animator animator in animators)
        {
            StartCoroutine(Animate(animator));
            time += 0.5f;
        }
    }

    IEnumerator Animate(Animator animator)
    {
        yield return new WaitForSeconds(time);
        animator.Play("Idle");
    }

    public void Reset()
    {
        foreach (Animator animator in animators)
        {
            animator.Play("none");
        }
    }
}
