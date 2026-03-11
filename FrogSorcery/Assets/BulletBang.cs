using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletBang : MonoBehaviour
{
    public Vector3 direction; 
    private bool myTurn;
    private int lastTurnMoved;
    [SerializeField] int bangDamage;
    //private Tilemap[] tilemaps;

    void Start()
    {   
        Debug.Log("Prefab Spawned "+ direction);
        GameManager.Instance.bulletAmount++;
        //Made a weird way of each instance referencing the tilemaps, for collision, 
        //and because just porting over the player CanMove bool seemed simple and it needs them, 
        //before realizing that i could probably just use a collider and it would perform better

        //tilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.InstanceID);
        //covermap = tilemaps[0];
        //walkmap = tilemaps[1];
        //collmap = tilemaps[2];
        //Debug.Log("covermap is "+tilemaps[0]);
        //Debug.Log("walkmap is "+tilemaps[1]);
        //Debug.Log("collmap is "+tilemaps[2]);
    }

    void Update()
    {
        if(GameManager.Instance.magicTurn && lastTurnMoved!=GameManager.Instance.turnCount){
            myTurn = true;
        }

        if(myTurn){
            Debug.Log("Moving");
            transform.position += direction;
            TurnOver();
        }
    }
    
    void TurnOver()
    {
        lastTurnMoved = GameManager.Instance.turnCount;
        myTurn = false;
        GameManager.Instance.bulletInit++;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Collision. Collider tag is "+other.gameObject.tag);
        if(other.gameObject.tag == "wall")
        {
            Debug.Log("THUD");
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "enemy")
        {
            Debug.Log("Hit "+other.gameObject.name);
            GetComponent<IndividualEnemy>().Damage(bangDamage);
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.bulletAmount--;
        if(lastTurnMoved==GameManager.Instance.turnCount)
        {
            GameManager.Instance.bulletInit--;
        }
        Debug.Log(GameManager.Instance.bulletAmount);
        Debug.Log(GameManager.Instance.bulletInit);
    }


}
