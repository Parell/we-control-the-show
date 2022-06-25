using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 3;
    [SerializeField] private float regenRate = 1;
    [SerializeField] private bool canRegen = false;
    [Space]
    [SerializeField] private float healCooldown = 3.0f;
    [SerializeField] private float maxHealCooldown = 3.0f;
    [SerializeField] private bool startCooldown = false;
    [Space]
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

        healthSystem.healthMax = health;
        healthSystem.health = healthSystem.healthMax;
    }

    void UpdateDamage()
    {
        health = healthSystem.health;
        // add a tartget fade and smooth it
        splatterOverlay.alpha = 1.0f - healthSystem.GetHealthPercent();
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

        if (healthSystem.isDead)
        {
            //movement.moveDisabledTimer = 3f;
        }

        if (healthSystem.health >= 0)
        {
            canRegen = false;
            healCooldown = maxHealCooldown;
            startCooldown = true;
            StartCoroutine(DamageFlash());
            UpdateDamage();
        }
    }

    void Update()
    {

        if (startCooldown)
        {
            healCooldown -= Time.deltaTime;
            if (healCooldown <= 0)
            {
                canRegen = true;
                startCooldown = false;
            }
        }

        if (canRegen)
        {
            if (healthSystem.health <= healthSystem.healthMax - 0.01f)
            {
                healthSystem.health += Time.deltaTime * regenRate;
                if (healthSystem.OnHealthChanged != null) healthSystem.OnHealthChanged(this, EventArgs.Empty);
                UpdateDamage();
            }
            else
            {
                healthSystem.health = healthSystem.healthMax;
                healCooldown = maxHealCooldown;
                canRegen = false;

                if (healthSystem.OnHealthChanged != null) healthSystem.OnHealthChanged(this, EventArgs.Empty);
                UpdateDamage();
            }
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
