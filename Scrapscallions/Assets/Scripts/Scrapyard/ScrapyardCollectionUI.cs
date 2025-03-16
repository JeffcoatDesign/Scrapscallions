using Scraps.Gameplay;
using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scraps.UI
{
    public class ScrapyardCollectionUI : MonoBehaviour
    {
        private ScrapyardCollection m_collection;
        [SerializeField] private CollectionItem m_collectedItemPrefab;

        List<GameObject> m_parts = new();

        private void Awake()
        {
            m_collection = (GameManager.Instance as ScrapyardManager).collection;
        }

        private void OnEnable()
        {
            if(m_parts.Count > 0)
            {
                foreach(var image in m_parts)
                {
                    Destroy(image.gameObject);
                }
                m_parts.Clear();
            }
            foreach(RobotPart part in m_collection.collectedParts)
            {
                var item = Instantiate(m_collectedItemPrefab, transform);
                item.SetPart(part);
                m_parts.Add(item.gameObject);
            }
        }
    }
}