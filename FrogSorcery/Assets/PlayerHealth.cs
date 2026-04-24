using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth;
    [SerializeField] TMP_Text counter;
    [SerializeField] GameObject player;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        counter.text = "Health: "+currentHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ApplyDamage(int amount)
    {
        currentHealth -= amount;
        counter.text = "Health: "+currentHealth; 
        if (currentHealth <= 0) Die();
        return true;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
