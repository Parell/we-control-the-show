using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem
{
    public int health;
    public int healthMax;
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
        return (float)health / (float)healthMax;
    }

    public void Death()
    {
        if (health == 0)
        {
            isDead = true;
        }
    }
}