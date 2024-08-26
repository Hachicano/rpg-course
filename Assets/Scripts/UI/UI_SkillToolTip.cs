using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private int defaultItemNameTextFontSize = 30;

    public void ShowToolTip(string _name, string _description)
    {
        skillName.text = _name;
        skillDescription.text = _description;
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        skillDescription.fontSize = defaultItemNameTextFontSize;
    }
}
