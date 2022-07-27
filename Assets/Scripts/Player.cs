using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] float health = 3f;
    [SerializeField] float regenRate = 1f;
    [SerializeField] bool canRegen = false;

    [Space]
    [SerializeField] float healCooldown;
    [SerializeField] float maxHealCooldown = 3f;
    [SerializeField] bool startCooldown = false;

    [Space]
    [SerializeField] float hurtTime = 0.1f;
    [SerializeField] CanvasGroup splatterOverlay;
    [SerializeField] CanvasGroup damageOverlay;

    HealthSystem healthSystem;
    PlayerMovement movement;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();

        healthSystem = new HealthSystem();

        //healthSystem.OnHealthChanged += healthSystem._OnHealthChanged;

        healthSystem.healthMax = health;
        healthSystem.health = healthSystem.healthMax;
    }

    void UpdateHealth()
    {
        health = healthSystem.health;
        // add a tartget fade and smooth it
        splatterOverlay.alpha = 1f - healthSystem.GetHealthPercent();
    }

    IEnumerator DamageFlash()
    {
        float alpha = 1f;

        yield return new WaitForEndOfFrame();

        while (alpha <= 1f)
        {
            alpha += Time.deltaTime * (1f / hurtTime) * -1f;
            damageOverlay.alpha = alpha;

            yield return null;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        healthSystem.health -= damageAmount;

        if (healthSystem.health < 0f) healthSystem.health = 0f;
        //if (healthSystem.OnHealthChanged != null) healthSystem.OnHealthChanged(this, EventArgs.Empty);

        if (healthSystem.health <= 0f)
        {
            //SystemManager.Instance.isCameraLocked = true;
            //SystemManager.Instance.isMovementLocked = true;
            canRegen = false;
            startCooldown = false;
        }
        else
        {
            canRegen = false;
            healCooldown = maxHealCooldown;
            startCooldown = true;
            StartCoroutine(DamageFlash());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TakeDamage(1f);
        }

        UpdateHealth();

        if (startCooldown)
        {
            healCooldown -= Time.deltaTime;
            if (healCooldown <= 0f)
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
                //if (healthSystem.OnHealthChanged != null) healthSystem.OnHealthChanged(this, EventArgs.Empty);
            }
            else
            {
                healthSystem.health = healthSystem.healthMax;
                healCooldown = maxHealCooldown;
                canRegen = false;

                //if (healthSystem.OnHealthChanged != null) healthSystem.OnHealthChanged(this, EventArgs.Empty);
            }
        }
    }
}
