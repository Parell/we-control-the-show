using System;
using UnityEngine.UI;

public class HealthSystem
{
    public float health;
    public float healthMax;

    public Slider healthSlider;

    public Action<object, EventArgs> OnHealthChanged;

    public void _OnHealthChanged(object sender, EventArgs e)
    {
        healthSlider.value = GetHealthPercent();
    }

    public float GetHealthPercent()
    {
        return health / healthMax;
    }
}