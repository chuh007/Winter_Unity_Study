using Code.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class DescriptionPanelUI : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI descriptionText;

        public void SetItemData(ItemDataSO itmeData)
        {
            if (itemImage == null)
            {
                itemImage.sprite = null;
                itemImage.color = Color.clear;
                descriptionText.text = string.Empty;
            }
            else
            {
                itemImage.sprite = itmeData?.icon;
                itemImage.color = Color.white;
                descriptionText.text = itmeData?.GetDescription();
            }
        }
    }
}