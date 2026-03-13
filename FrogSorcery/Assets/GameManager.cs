using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlayerMovement playerMovement;
    public bool spellComplete = true;
    public bool magicTurn = false;
    public bool enemyTurn = false;
    public int turnCount = 0;
    public int enemyAmount = 0;
    public int enemyInit = 0;
    public int bulletAmount = 0;
    public int bulletInit = 0;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            spellComplete = true;
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
    }

    void Update() 
    {
        if(enemyTurn && enemyAmount == enemyInit)
        {
            enemyTurn = false;
            MagicTurn();
        }

        if(magicTurn && bulletAmount == bulletInit && spellComplete)
        {
            magicTurn = false;
            turnCount++;
            playerMovement.PlayerTurn();
        }
    }

    public void EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        enemyInit = 0;
        enemyTurn = true;
    }

    public void MagicTurn()
    {
        Debug.Log("Magic Turn");
        bulletInit = 0;
        magicTurn = true;
    }
}
