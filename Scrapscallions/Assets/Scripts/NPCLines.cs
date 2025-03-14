using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCLines : MonoBehaviour
{
    [SerializeField] private string[] lines;

    void OnEnable()
    {
        GetComponent<TextMeshProUGUI>().text = lines[Random.Range(0, lines.Length)];
    }
}
