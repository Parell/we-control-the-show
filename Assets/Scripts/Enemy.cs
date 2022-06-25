using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int m_health = 1;

    HealthSystem healthSystem;

    private void Start()
    {
        healthSystem = new HealthSystem();

        healthSystem.OnHealthChanged += healthSystem._OnHealthChanged;

        healthSystem.healthMax = m_health;
        healthSystem.health = healthSystem.healthMax;

        //Debug.Log(gameObject.name + healthSystem.health);
    }

    public void Damage(int damageAmount)
    {
        healthSystem.health -= damageAmount;

        if (healthSystem.health < 0) healthSystem.health = 0;
        if (healthSystem.OnHealthChanged != null) healthSystem.OnHealthChanged(this, EventArgs.Empty);

        //Debug.Log(gameObject.name + healthSystem.health);

        m_health = healthSystem.health;

        if (healthSystem.isDead)
        {
            Destroy(this.gameObject);
        }
    }
}
