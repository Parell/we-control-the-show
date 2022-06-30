using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem
{
    public float health;
    public float healthMax;

    [Space]
    [SerializeField] Slider healthSlider;

    public Action<object, EventArgs> OnHealthChanged;

    public void _OnHealthChanged(object sender, EventArgs e)
    {
        if (healthSlider != null)
        {
            healthSlider.value = GetHealthPercent();
        }
    }

    public float GetHealthPercent()
    {
        return health / healthMax;
    }
}