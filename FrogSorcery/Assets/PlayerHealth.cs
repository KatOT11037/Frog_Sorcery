using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ApplyDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
        return true;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
