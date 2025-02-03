using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewResetter : MonoBehaviour
{
    void Awake()
    {
        GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y - 175, GetComponent<RectTransform>().position.z);
    }
}
