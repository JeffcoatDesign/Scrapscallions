using Scraps.Gameplay;
using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scraps.UI
{
    public class ScrapyardCollectionUI : MonoBehaviour
    {
        ScrapyardCollection m_collection;

        List<Image> m_images = new();

        private void Awake()
        {
            m_collection = ScrapyardManager.Instance.collection;
        }

        private void OnEnable()
        {
            if(m_images.Count > 0)
            {
                foreach(var image in m_images)
                {
                    Destroy(image.gameObject);
                }
                m_images.Clear();
            }
            foreach(RobotPart part in m_collection.collectedParts)
            {
                Image image = new GameObject().AddComponent<Image>();
                image.transform.parent = transform;
                image.sprite = part.Sprite;
                m_images.Add(image);
            }
        }
    }
}