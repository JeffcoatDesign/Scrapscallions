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

    [SerializeField]
    private Slider playerHP, enemyHP;

    void FixedUpdate()
    {
        if (isTimerGoing && isBattleOpen)
        {
            timePassed = timePassed - Time.deltaTime;
            timerText.text = timePassed.ToString("F0");
        }
        if (timePassed <= 0)
            isTimerGoing = false;
    }

    public void DamagePlayer()
    {
        if (playerHP.value > 0)
            playerHP.value -= 10;
    }

    public void DamageEnemy()
    {
        if (enemyHP.value > 0)
            enemyHP.value -= 10;
    }

    public void HealPlayer()
    {
        if (playerHP.value < 100)
            playerHP.value += 10;
    }

    public void HealEnemy()
    {
        if (enemyHP.value < 100)
            enemyHP.value += 10;
    }

    public void ResetBattle()
    {
        timePassed = 99;
        isTimerGoing = true;
        playerHP.value = 100;
        enemyHP.value = 100;
    }
}
