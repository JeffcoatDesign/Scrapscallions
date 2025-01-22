using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timePassed = 99;
    public bool isTimerGoing = true;
    public bool isBattleOpen = true;

    void FixedUpdate()
    {
        if (isTimerGoing && isBattleOpen)
        {
            timePassed = timePassed - Time.deltaTime;
            timerText.text = timePassed.ToString("F0");
        }
        if (timePassed <= 0)
            isTimerGoing = false;
        if (!isBattleOpen)
            timePassed = 99;
    }
}
