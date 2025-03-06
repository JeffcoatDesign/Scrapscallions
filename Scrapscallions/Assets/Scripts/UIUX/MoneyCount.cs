using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scraps.UI
{
    public class MoneyCount : MonoBehaviour
    {
        private TextMeshProUGUI m_text;
        private void Awake()
        {
            m_text = GetComponent<TextMeshProUGUI>();
        }
        private void OnEnable()
        {
            InventoryManager.Instance.MoneyChanged += OnMoneyChanged;
            m_text.text = InventoryManager.Instance.money.ToString();
        }
        private void OnDisable()
        {
            InventoryManager.Instance.MoneyChanged -= OnMoneyChanged;
        }
        private void OnMoneyChanged(int money)
        {
            m_text.text = money.ToString();
        }
    }
}