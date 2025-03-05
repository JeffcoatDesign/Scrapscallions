using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    bool m_isNegative;

    private void Awake()
    {
        bool isXNegative = transform.parent.localScale.x < 0;
        bool isYNegative = transform.parent.localScale.y < 0;
        bool isZNegative = transform.parent.localScale.z < 0;
        m_isNegative = isXNegative || isYNegative || isZNegative;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        if (!m_isNegative)
            transform.Rotate(0, 180, 0);
    }
}
