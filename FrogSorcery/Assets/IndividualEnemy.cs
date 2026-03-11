using UnityEngine;
using DG.Tweening;

public class IndividualEnemy : MonoBehaviour
{
    public int enemyHealth;
    public bool awoken;
    private Renderer rend;

    void Awake(){
        enemyHealth = 3;
        awoken = false;
        Renderer rend = GetComponent<SpriteRenderer>();
    }

    void update()
    {

    }

    public void Damage(int amount)
    {
        enemyHealth -= amount; 
        if(enemyHealth == 0) Die();
    }

    void Die()
    {
//        DOFade(0f, 2.5f);
        gameObject.SetActive(false);
    }
}
