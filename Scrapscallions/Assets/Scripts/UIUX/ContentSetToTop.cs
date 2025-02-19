using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentSetToTop : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x, 0);
    }
}
