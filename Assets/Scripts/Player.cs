using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private int m_health = 3;

    [SerializeField] private float hurtTime = 0.1f;

    [SerializeField] private CanvasGroup splatterOverlay;
    [SerializeField] private CanvasGroup damageOverlay;

    private HealthSystem healthSystem;

    //private Movement movement;

    private void Start()
    {
        //movement = GetComponent<Movement>();

        healthSystem = new HealthSystem();

        healthSystem.OnHealthChanged += healthSystem._OnHealthChanged;

        healthSystem.healthMax = m_health;
        healthSystem.health = healthSystem.healthMax;
    }

    IEnumerator DamageFlash()
    {
        float alpha = 1;

        yield return new WaitForEndOfFrame();

        while (alpha <= 1)
        {
            alpha += Time.deltaTime * (1.0f / hurtTime) * -1;
            damageOverlay.alpha = alpha;

            yield return null;
        }
    }

    public void Damage(int damageAmount)
    {
        healthSystem.health -= damageAmount;

        if (healthSystem.health < 0) healthSystem.health = 0;
        if (healthSystem.OnHealthChanged != null) healthSystem.OnHealthChanged(this, EventArgs.Empty);

        m_health = healthSystem.health;

        if (healthSystem.isDead)
        {
            //movement.moveDisabledTimer = 3f;
        }

        if (m_health >= 0)
        {
            splatterOverlay.alpha = 1.0f - healthSystem.GetHealthPercent();
            StartCoroutine(DamageFlash());
        }
    }

    // public void Respawn()
    // {
    //     isDead = false;

    //     health += healthMax;
    //     if (health < 0) health = 0;
    //     if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    // }
}
