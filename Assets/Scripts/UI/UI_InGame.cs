using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    [Header("Souls Info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 2000;
    private SkillManager skill;

    void Start()
    {
        if (playerStats != null)  // ������ֲ�����Ѫ�Ļ������������
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        skill = SkillManager.instance;
    }

    void Update()
    {
        UpdateSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.F) && skill.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        if (Input.GetKeyDown(KeyCode.Mouse3) && skill.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.Mouse1) && skill.sword.swordUnlocked)
            SetCooldownOf(swordImage);

        if (skill.blackhole.blackholeUnlocked && PlayerManager.instance.player.blackholeState.skillFinished)
            SetCooldownOf(blackholeImage);

        if (Input.GetKeyDown(KeyCode.R) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        CheckCooldownOf(dashImage, skill.dash.cooldown);
        CheckCooldownOf(parryImage, skill.parry.cooldown);
        CheckCooldownOf(crystalImage, skill.crystal.cooldown);
        CheckCooldownOf(swordImage, skill.sword.cooldown);
        CheckCooldownOf(blackholeImage, skill.blackhole.cooldown);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrency())
        {
            soulsAmount += Time.deltaTime * increaseRate;
        }
        else
        {
            soulsAmount = PlayerManager.instance.GetCurrency();
        }

        currentSouls.text = ((int)soulsAmount).ToString("#,#");
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetTotalMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
