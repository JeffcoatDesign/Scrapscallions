using Scraps;
using Scraps.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scraps.UI
{
    public class WinnerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_winnerText;
        [SerializeField] GameObject m_winnerBG;
        private void Start()
        {
            GameManager.Instance.AnnounceWinner += OnAnnounceWinner;
        }

        private void OnAnnounceWinner(string winner)
        {
            m_winnerText.text = $"{winner} Won!";
            m_winnerBG.SetActive(true);
        }
    }
}