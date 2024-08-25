using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultItemNameTextFontSize = 36;

    public void ShowTollTip(ItemData_Equipment _item)
    {
        if (_item == null)
            return;

        itemNameText.text = _item.name;
        itemTypeText.text = _item.equipmentType.ToString();
        itemDescription.text = _item.GetDiscription();

        if (itemNameText.text.Length > 15)
        {
            itemNameText.fontSize = itemNameText.fontSize * .85f;
        }
        else
        {
            itemNameText.fontSize = defaultItemNameTextFontSize;
        }

        gameObject.SetActive(true);
    }


    public void HideToolTip()
    {
        gameObject.SetActive(false);
        itemNameText.fontSize = defaultItemNameTextFontSize;
    }
}
