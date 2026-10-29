using UnityEngine;
using System.Collections;

public class LivingEntity : Entity, IDamageable {
    // Includes code in relation to damageable interface
    public float maxHealth;
    float health;

    public override void Start()
    {
        health = maxHealth;
    }

    public virtual void Damage(float damage)
    {
        health -= damage;
        if (health <= 0) { Die(); }
    }
    public virtual void Die()
    {
        Destroy(transform);
    }
}
