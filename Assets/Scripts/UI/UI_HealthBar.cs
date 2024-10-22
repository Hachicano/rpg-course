using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats stats;
    private RectTransform rectTransform;
    private Slider slider;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.GetTotalMaxHealthValue();
        slider.value = stats.currentHealth;
    }

    private void FlipUI() => rectTransform.Rotate(0, 180, 0);

    private void OnEnable()
    {
        entity = GetComponentInParent<Entity>();
        stats = GetComponentInParent<CharacterStats>();
        entity.onFlipped += FlipUI;
        stats.onHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;
        if (slider != null)
            stats.onHealthChanged -= UpdateHealthUI;
    }

}
