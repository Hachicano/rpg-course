using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui;
    private Image skillImage; // Used for assgin skill icon, now can be hidden(remove [SerializeField]) cause it is not gonna being used.

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked4Relock;



    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        skillImage.color = lockedSkillColor;
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot()); // Skill_UI should be active on the beginning
                                                                             // Otherwise it wont work properly.
    }

    private void Start()
    {
        if (unlocked)
            skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (!unlocked)
        {
            for (int i = 0; i < shouldBeUnlocked.Length; i++)
            {
                if (shouldBeUnlocked[i].unlocked == false)
                {
                    Debug.Log("Cannot unlock skill");
                    return;
                }
            }

            for (int i = 0; i < shouldBeLocked.Length; i++)
            {
                if (shouldBeLocked[i].unlocked == true)
                {
                    Debug.Log("Cannot unlock skill");
                    return;
                }
            }

            if (PlayerManager.instance.HaveEnoughMoney(skillCost) == false)
                return;

            unlocked = true;
            skillImage.color = Color.white;
        }
        else if (unlocked)
        {
            for(int i = 0;i < shouldBeLocked4Relock.Length; i++)
            {
                if (shouldBeLocked4Relock[i].unlocked == true)
                {
                    Debug.Log("Cannot relock skill");
                    return;
                }
            }

            PlayerManager.instance.ReturnMoney(skillCost);
            unlocked = false;
            skillImage.color = lockedSkillColor;
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillName ,skillDescription, skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }
}
