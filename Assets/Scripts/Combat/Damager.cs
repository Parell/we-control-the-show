using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private int damageAmount;

    private void OnTriggerEnter(Collider collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.Damage(damageAmount);
        }
    }
}
