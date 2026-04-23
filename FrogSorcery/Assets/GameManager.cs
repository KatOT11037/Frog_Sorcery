using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] public int enemySpawnAmount;
    public static GameManager Instance;
    private KnightEnemy knightEnemy;
    private PlayerMovement playerMovement;
    public bool spellComplete = true;
    public bool magicTurn = false;
    public bool enemyTurn = false;
    public bool enemyProjTurn = false;
    public int turnCount = 0;
    public int enemyAmount = 0;
    public int enemyInit = 0;
    public int enemyProjAmount = 0;
    public int enemyProjInit = 0;
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
    knightEnemy = Resources.Load<KnightEnemy>("Knight Enemy");

    }
    void Start()
    {
        SpawnEnemies();
    }
    void SpawnEnemies()
    {
        int [] spawnsTaken = new int[enemySpawnAmount];

        for(int i = 0; i<enemySpawnAmount; i++)
        {
            spawnsTaken[i] = -1;
        }

        for(int i = 0; i < enemySpawnAmount; i++)
        {
            int point = Random.Range(0, 9);
            if(CheckTaken(point, spawnsTaken, i))
            {
            spawnsTaken[i] = point;
            }
            else
            {
                i--;
            }
        }

        for(int i = 0; i<enemySpawnAmount; i++)
        {
            KnightEnemy knight = Instantiate(knightEnemy, spawnPoints[spawnsTaken[i]].position, Quaternion.identity);
        }
    }

    private bool CheckTaken(int point, int[] spawnsTaken, int repetitions)
    {
        Debug.Log("Checking "+point+spawnsTaken+repetitions);
        for(int j = 0; j<=repetitions; j++)
        {
            if(spawnsTaken[j] == point )
            {
                return false;
            }
        }
        return true;
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

        if(enemyProjTurn && enemyProjAmount == enemyProjInit)
        {
            enemyProjTurn = false;
            turnCount++;
            playerMovement.PlayerTurn();
        }

        if(magicTurn && bulletAmount == bulletInit && spellComplete)
        {
            magicTurn = false;
            EnemyProjectileTurn();
        }
    }

    public void EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        enemyInit = 0;
        enemyTurn = true;
    }

    public void EnemyProjectileTurn()
    {
        Debug.Log("Enemy Projectile Turn");
        enemyProjInit = 0;
        enemyProjTurn = true;
    }

    public void MagicTurn()
    {
        Debug.Log("Magic Turn");
        bulletInit = 0;
        magicTurn = true;
    }
}
