using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;
    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null )
            statNameText.text = statName;
    }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

    void Start()
    {
        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();


            if (statType == StatType.maxHealth)
                statValueText.text = Mathf.Round(playerStats.GetTotalMaxHealthValue()).ToString();
            if (statType == StatType.phsicalDamage)
                statValueText.text = Mathf.Round(playerStats.GetTotalPhisicalDamgeValue()).ToString();
            if (statType == StatType.critChance)
                statValueText.text = Mathf.Round(playerStats.GetTotalCritChance()).ToString();
            if (statType == StatType.critPower)
                statValueText.text = Mathf.Round(playerStats.GetTotalCritPower()).ToString();
            if (statType == StatType.evasion)
                statValueText.text = Mathf.Round(playerStats.GetTotalEvasion()).ToString();
            if (statType == StatType.magicResistance)
                statValueText.text = Mathf.Round(playerStats.GetTotalMagicResistance()).ToString();

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);

        Vector2 mousePosition = Input.mousePosition;
        float xOffset = 0;
        float yOffset = 0;
        if (mousePosition.x > (Screen.currentResolution.width / 2)) //600
            xOffset = -150;
        else
            xOffset = 150;

        if (mousePosition.y > (Screen.currentResolution.height / 2)) //320
            yOffset = -50;
        else
            yOffset = 150;
        ui.statToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}