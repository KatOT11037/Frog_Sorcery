using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth;
    [SerializeField] TMP_Text counter;
    [SerializeField] GameObject player;
    private SpriteRenderer rend;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        counter.text = "Health: "+currentHealth; 
        rend = player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ApplyDamage(int amount)
    {
        currentHealth -= amount;
        counter.text = "Health: "+currentHealth; 
        rend.color =Color.red;
        rend.DOColor(Color.white, 0.5f);
        if (currentHealth <= 0) Die();
        return true;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
