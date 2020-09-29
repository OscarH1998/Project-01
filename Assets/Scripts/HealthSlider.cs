using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]

public class HealthSlider : MonoBehaviour
{
    [SerializeField] Health _health = null;

    Slider _slider = null;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _health.HealthChanged += OnHealthChange;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnHealthChange;
    }

    private void Start()
    {
        _slider.maxValue = _health.maxHealth;
        _slider.value = _health.currentHealth;
    }

    void OnHealthChange(int newCurrentHealth)
    {
        _slider.value = newCurrentHealth;
    }
}