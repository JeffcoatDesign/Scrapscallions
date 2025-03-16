using Scraps.Parts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scraps.UI
{
    public class CollectionItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image m_itemImage;
        [SerializeField] private DragDrop m_dragDrop;
        [SerializeField] public TextMeshProUGUI[] itemDescription;
        private RectTransform toolTipPos;
        private RobotPart m_part;

        public GameObject toolTip;
        private void Awake()
        {
            toolTipPos = toolTip.GetComponent<RectTransform>();
        }
        public void SetPart(RobotPart part)
        {
            m_part = part;
            m_itemImage.sprite = part.Sprite;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Does nothing if DragDrop doesn't have a ToolTip
            if (toolTip == null)
                return;

            string partType = "";

            //Checks and sets the text field of the ToolTip according to the botPart type
            if (m_part is RobotPartArm arm)
            {
                partType = "Arm";
                itemDescription[3].gameObject.SetActive(true);
                itemDescription[4].gameObject.SetActive(true);
                itemDescription[3].text = "Attack Speed: " + arm.AttackSpeed;
                itemDescription[4].text = "Damage: " + arm.AttackDamage;
            }
            else if (m_part is RobotPartHead head)
            {
                partType = "Head";
                itemDescription[3].gameObject.SetActive(true);
                itemDescription[3].text = "Quirks: "; //Need quirks done for quirk description
                if (itemDescription.Length > 4)
                    itemDescription[4].gameObject.SetActive(false);
            }
            else
            {
                if (m_part is RobotPartLegs legs)
                {
                    partType = "Legs";
                }
                else if (m_part is RobotPartBody body)
                {
                    partType = "Body";
                }
                if (itemDescription.Length > 3)
                {
                    itemDescription[3].gameObject.SetActive(false);
                    itemDescription[4].gameObject.SetActive(false);
                }
            }
            itemDescription[0].text = partType + ": \"" + m_part.PartName + "\"";
            itemDescription[1].text = "Maximum HP: " + m_part.MaxHP.ToString();
            itemDescription[2].text = "Price: $" + m_part.Price.ToString();
            toolTip.SetActive(true);

            //Moves ToolTip position if it's in a position that it can go off screen
            if ((int)toolTipPos.position.x == 1848)
                toolTipPos.position = new Vector2(toolTipPos.position.x - 400, toolTipPos.position.y);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (toolTip == null)
                return;
            toolTip.SetActive(false);
        }
    }
}