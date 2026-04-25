using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBubble : MonoBehaviour
{
    public int aimDirection; 
    private bool isPopped = false;
    private Animator animator;
    private Vector3 bubbleDirection;
    private Vector3 throwDirection;
    //private Transform transform;
    private bool myTurn;
    private int lastTurnMoved =-1;
    [SerializeField] int bangDamage;
    [SerializeField] int pitch;
    [SerializeField] int speed;
    private EnemyKnightBang bang;
    //private Tilemap[] tilemaps;

    void Start()
    {   
        switch (aimDirection){

            case 0:
            bubbleDirection = new Vector3(0,1,0);
            throwDirection = new Vector3(1,0,0);
            transform.Rotate(0,0,270);
            break;

            case 1:
            bubbleDirection = new Vector3(1,0,0);
            throwDirection = new Vector3(0,1,0);
            break;

            case 2:
            bubbleDirection = new Vector3(1,0,0);
            throwDirection = new Vector3(0,-1,0);
            transform.Rotate(0,0,180);
            break;

            case 3:
            bubbleDirection = new Vector3(0,-1,0);
            throwDirection = new Vector3(1,0,0);
            transform.Rotate(0,0,270);
            break;

            case 4:
            bubbleDirection = new Vector3(0,-1,0);
            throwDirection = new Vector3(-1,0,0);
            transform.Rotate(0,0,90);
            break;

            case 5:
            bubbleDirection = new Vector3(-1,0,0);
            throwDirection = new Vector3(0,-1,0);
            transform.Rotate(0,0,180);
            break;

            case 6:
            bubbleDirection = new Vector3(-1,0,0);
            throwDirection = new Vector3(0,1,0);
            break;

            case 7:
            bubbleDirection = new Vector3(0,1,0);
            throwDirection = new Vector3(-1,0,0);
            transform.Rotate(0,0,90);
            break;
        }
        Debug.Log("Prefab Spawned "+ aimDirection + bubbleDirection + throwDirection);
        GameManager.Instance.enemyProjAmount++;
        GameManager.Instance.spellComplete = true;
        animator = GetComponent<Animator>();
        bang = Resources.Load<EnemyKnightBang>("EnemyKBang");
    }

    void Update()
    {
        if(GameManager.Instance.enemyProjTurn && lastTurnMoved!=GameManager.Instance.turnCount && !isPopped){
            myTurn = true;
        }

        if(myTurn){
            Debug.Log("Moving");
            if(pitch!=0){
            Debug.Log("i go");
                for (int i = 0; i!=speed; i++)
                {
                Debug.Log("goin");
                transform.position += bubbleDirection;
                pitch--;
                }
            TurnOver();
            }
            else
            {
                Pop();
            }
        }
    }
    
    void TurnOver()
    {
        lastTurnMoved = GameManager.Instance.turnCount;
        myTurn = false;
        GameManager.Instance.enemyProjInit++;
    }

    //collision detection. Vestigial as this projectile does not deal damage
/*
    void OnTriggerEnter2D(Collider2D other) {
        if(meComplete){
        Debug.Log("Collision. Collider tag is "+other.gameObject.tag);
        if(other.gameObject.tag == "wall")
        {
            Debug.Log("THUD");
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "enemy")
        {
            Debug.Log("Hit "+other.gameObject.name);
            other.GetComponent<IndividualEnemy>().Damage(bangDamage);
            Destroy(gameObject);
        }
        }else{Debug.Log("Spell Collided before fully formed.");}
    }
    */

    void Pop(){
        EnemyKnightBang kbangbullet = Instantiate(bang, transform.position, Quaternion.identity);
        kbangbullet.direction = throwDirection;
        kbangbullet.bLastTurnMoved = lastTurnMoved;
        animator.SetTrigger("Pop");
        isPopped = true;
        myTurn = false;
        Destroy(gameObject, 0.6f);
    }
    void OnDestroy()
    {
        Debug.Log("Enemy projectile amount is "+GameManager.Instance.enemyProjAmount);
        Debug.Log("Enemy projectile initiative is "+GameManager.Instance.enemyProjInit);
    }


}

