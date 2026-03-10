using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlayerMovement playerMovement;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    playerMovement = FindAnyObjectByType<PlayerMovement>();

    }
    public void TurnEnd()
    {
        EnemyTurn();
        MagicTurn();
        playerMovement.PlayerTurn();
    }

    public void EnemyTurn()
    {
        Debug.Log("Enemy Turn");
    }

    public void MagicTurn()
    {
        Debug.Log("Magic Turn");
    }
}
