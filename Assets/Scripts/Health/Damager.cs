using System.Collections;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] float damageAmount;
    [SerializeField] float hitTime = 1f;
    [SerializeField] int hit;

    IDamageable damageable;

    void OnTriggerEnter(Collider collider)
    {
        damageable = collider.GetComponent<IDamageable>();

        if (damageable != null && hit == 1)
        {
            StartCoroutine(HitTimer());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (damageable != null)
        {
            hit++;
        }
    }

    IEnumerator HitTimer()
    {
        damageable.TakeDamage(damageAmount);
        yield return new WaitForSeconds(hitTime);
        hit = 0;
    }
}
