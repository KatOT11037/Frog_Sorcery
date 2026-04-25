using UnityEngine;
using UnityEngine.Tilemaps;

public class KnightBang : MonoBehaviour
{
    public Vector3 direction;
    //private Transform transform;
    private bool myTurn;
    public int bLastTurnMoved =-1;
    [SerializeField] int bangDamage;
    [SerializeField] int pitch;
    [SerializeField] int speed;
    private bool meComplete = false;
    //private Tilemap[] tilemaps;

    void Start()
    {   
        Debug.Log(direction.ToString());
        switch (direction.ToString()){

            case "(0.00, 1.00, 0.00)":
            Debug.Log("Up");
            break;

            case "(0.00, -1.00, 0.00)":
            transform.Rotate(0,0,180);
            Debug.Log("Down");
            break;

            case "(1.00, 0.00, 0.00)":
            transform.Rotate(0,0,270);
            Debug.Log("Right");
            break;

            case "(-1.00, 0.00, 0.00)":
            transform.Rotate(0,0,90);
            Debug.Log("Left");
            break;

            case null:
            Debug.Log("null");
            break;
        }
        meComplete = true;

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
        if(GameManager.Instance.magicTurn && bLastTurnMoved!=GameManager.Instance.turnCount){
            myTurn = true;
        }

        if(myTurn)
        {
        Debug.Log("Moving");
            if(pitch!=0)
            {
                for(int i = 0; i!=speed; i++)
                {
                transform.position += direction;
                pitch--;
                }
            TurnOver();
            }
            else
            {
            Destroy(gameObject);
            }
            }
    }
    
    void TurnOver()
    {
        bLastTurnMoved = GameManager.Instance.turnCount;
        myTurn = false;
        GameManager.Instance.bulletInit++;
    }
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
            other.GetComponent<IEnemyDamageable>().ApplyDamage(bangDamage);
            Destroy(gameObject);
        }
        }else{Debug.Log("Spell Collided before fully formed.");}
    }
    void OnDestroy()
    {
        GameManager.Instance.bulletAmount--;
        if(bLastTurnMoved==GameManager.Instance.turnCount)
        {
            GameManager.Instance.bulletInit--;
        }
        Debug.Log(GameManager.Instance.bulletAmount);
        Debug.Log(GameManager.Instance.bulletInit);
    }
}

