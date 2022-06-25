using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem
{
    public float health;
    public float healthMax;
    public bool isDead = false;

    [Space]
    [SerializeField] private Slider healthSlider;

    public Action<object, EventArgs> OnHealthChanged;

    public void _OnHealthChanged(object sender, EventArgs e)
    {
        Death();

        if (healthSlider != null)
        {
            healthSlider.value = GetHealthPercent();
        }
    }

    public float GetHealthPercent()
    {
        return health / healthMax;
    }

    public void Death()
    {
        if (health == 0)
        {
            isDead = true;
        }
    }
}