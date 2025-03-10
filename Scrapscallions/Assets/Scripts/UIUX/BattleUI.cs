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

    public void DamagePlayer(int dmg)
    {
        if (playerHP.value > 0)
            playerHP.value -= dmg;
    }

    public void DamageEnemy(int dmg)
    {
        if (enemyHP.value > 0)
            enemyHP.value -= dmg;
    }

    public void HealPlayer(int dmg)
    {
        if (playerHP.value < 100)
            playerHP.value += dmg;
    }

    public void HealEnemy(int dmg)
    {
        if (enemyHP.value < 100)
            enemyHP.value += dmg;
    }

    public void ResetBattle()
    {
        timePassed = 99;
        isTimerGoing = true;
        playerHP.value = 100;
        enemyHP.value = 100;
    }
}
