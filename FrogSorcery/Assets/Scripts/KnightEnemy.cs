using UnityEngine;
using DG.Tweening;
using System.Collections;

public class KnightEnemy : MonoBehaviour, IEnemyDamageable
{
    public int speed;
    public int torpor;
    public int enemyHealth;
    public bool awoken;
    private int distanceX;
    private int distanceY;
    private SpriteRenderer rend;
    private Transform player;
    private bool myTurn;
    private int lastTurnMoved;
    private Transform trans;
    private PlayerMovement playerMovement;
    private Vector3 v3;
    private bool dead = false;
    private float currentDestinationDist = -1;
    private Vector3 currentChoice = new Vector3(-99,-99,-99);
    private Vector3 estabChoice;
    private int choiceDir;
    private Vector3 finalDist = Vector3.positiveInfinity;
    private EnemyBubble bubble;

    void Awake(){
        DOTween.Init();
        enemyHealth = 2;
        dead = false;
        lastTurnMoved = -1;
        rend = GetComponent<SpriteRenderer>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if(playerHealth != null){
        trans = GetComponent<Transform>();
        player = playerHealth.transform;
        Debug.Log(player.position);}
        bubble = Resources.Load<EnemyBubble>("EnemyBubble");
    }

    void Start()
    {
        GameManager.Instance.enemyAmount++;
    }

    void Update()
    {
        if(GameManager.Instance.enemyTurn && lastTurnMoved!=GameManager.Instance.turnCount && !dead)
        {
            myTurn=true;
        }
        if (myTurn)
        {   
            for(int i =0; i!=speed; i++){  
            Act(ChooseDestination()); 
            }
            endTurn();
        }
    }

    public bool ApplyDamage(int amount)
    {
        enemyHealth -= amount; 
//        StartCoroutine(ColourRed(0.8f));
        rend.color =Color.red;
        rend.DOColor(Color.white, 0.5f);
        if(enemyHealth <= 0){StartCoroutine(EnemyDie());}
        return true;
    }

    bool CanMove(Vector3 destination)
    {
        Vector3Int gridPosition = playerMovement.walkmap.WorldToCell(destination);
        if(!(playerMovement.walkmap.HasTile(gridPosition)))
        {
            return false;
        }else if(playerMovement.collmap.HasTile(gridPosition))
            {
            return false;
            }
        else
            {
            return true;
            }
    }
    Vector3 ChooseDestination()
    {
        currentChoice = new Vector3(-99,-99,-99);
        for(int i =0; i<=7; i++)
        {
            for(int j=0; j<=7; j++)
            {
                if(estabChoice == playerMovement.knightVulnerable[j]){
                    currentChoice = estabChoice;
                    choiceDir = j;
                    Debug.Log("Stuck with established choice "+j);
                    Debug.Log(estabChoice); Debug.Log(playerMovement.knightVulnerable[j]);
                }
            }
            if((currentChoice[2] == -99)&&(CanMove(playerMovement.knightVulnerable[i]))){
                Debug.Log("Current Destination is default");
                currentChoice = playerMovement.knightVulnerable[i];
                choiceDir = i;
                currentDestinationDist = Vector3.Distance(trans.position, currentChoice);
                Debug.Log("Set current");
            }else if((currentDestinationDist >= Vector3.Distance(trans.position, playerMovement.knightVulnerable[i]))&&(CanMove(playerMovement.knightVulnerable[i]))){
                if((currentDestinationDist == Vector3.Distance(trans.position, playerMovement.knightVulnerable[i])))
                {
                    int random = Random.Range(0,2);
                    if(random == 1){
                        currentChoice = playerMovement.knightVulnerable[i];
                        choiceDir = i;
                        Debug.Log("Equal, switched");
                    }else{
                        Debug.Log("Equal, unswitched");
                    }
                }else{
                currentChoice = playerMovement.knightVulnerable[i];
                choiceDir = i;
                currentDestinationDist = Vector3.Distance(trans.position, currentChoice);
                Debug.Log("Switched current");
                }
            }
        }

        finalDist = new Vector3(currentChoice.x-trans.position.x, currentChoice.y-trans.position.y, 0);
        estabChoice = currentChoice;
        Debug.Log("Final Distance to Destination: "+finalDist);
        Debug.Log("Final Choice: "+currentChoice);
        return finalDist;
    }
    
    void Act(Vector3 distToDestination){
        if(torpor<=0){
            if(distToDestination!=Vector3.zero){
                Debug.Log("Enemy Moving");
                Move(distToDestination);
            }else{
                Attack();
            }
        }
        else
        {
            torpor =- 1;
        }
    }

    void Move(Vector3 distToDestination)
    {
        int moveAxis = -1;
        distanceX = (int) Mathf.Abs(distToDestination.x);
        distanceY = (int) Mathf.Abs(distToDestination.y);

        if(distanceX == distanceY){
            Debug.Log("Random Move");
            moveAxis = Random.Range(0, 2);
        }else if(distanceX > distanceY){
            if(distanceY == 1)
            {
            Debug.Log("Moved Y from 1-Clause");
            moveAxis = 1;
            }else{
            Debug.Log("Moved X from Distance");
            moveAxis = 0;
            }
        }else if(distanceY > distanceX){
            if(distanceX == 1)
            {
            Debug.Log("Moved X from 1-Clause");
            moveAxis = 0;
            }else{
            Debug.Log("Moved Y from Distance");
            moveAxis = 1;
            }
        }else{
            Debug.Log("Enemy Move calc error");
        }

        Vector3 movement = Vector3.zero;
        movement[moveAxis] = movement[moveAxis] + Mathf.Sign(distToDestination[moveAxis]);
        trans.position = trans.position + movement;
        Debug.Log("Moved to "+trans.position);
    }

    void Attack(){
        int bangDir = choiceDir + 4;
        if(bangDir>=8) bangDir-=8;
        Debug.Log("Attack");
        EnemyBubble eBubble = Instantiate(bubble, trans.position, Quaternion.identity);
        eBubble.aimDirection = bangDir;
        torpor =+ 1;
    }

    void endTurn()
    {
        myTurn =false;
        lastTurnMoved = GameManager.Instance.turnCount;
        GameManager.Instance.enemyInit++;
    }
    private IEnumerator EnemyDie()
    {
        if(!dead){
        GameManager.Instance.EnemyDead();
        dead = true;
        if (lastTurnMoved == GameManager.Instance.turnCount){GameManager.Instance.enemyInit--;}
        rend.color = Color.red;
        rend.DOFade(0f, 2f);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        }
    }
}