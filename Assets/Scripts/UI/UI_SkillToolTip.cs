using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private int defaultItemNameTextFontSize = 30;

    public void ShowToolTip(string _name, string _description, int _cost)
    {
        skillName.text = _name;
        skillDescription.text = _description;
        skillCost.text = "Cost: " + _cost;

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        skillDescription.fontSize = defaultItemNameTextFontSize;
    }
}
