using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private CharacterStats stats;
    private RectTransform rectTransform;
    private Slider slider;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        stats = GetComponentInParent<CharacterStats>();

        entity.onFlipped += FlipUI;
        stats.onHealthChanged += UpdateHealthUI;
    }

    private void Start()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealthValue();
        slider.value = stats.currentHealth;
    }

    private void FlipUI() => rectTransform.Rotate(0, 180, 0);
    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        stats.onHealthChanged -= UpdateHealthUI;
    }

}
