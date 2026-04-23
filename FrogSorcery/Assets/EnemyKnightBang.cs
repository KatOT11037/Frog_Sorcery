using UnityEngine;

public class EnemyKnightBang : MonoBehaviour
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
    }

    void Update()
    {
        if(GameManager.Instance.enemyProjTurn && bLastTurnMoved!=GameManager.Instance.turnCount){
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
        GameManager.Instance.enemyProjInit++;
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(meComplete){
        Debug.Log("Collision. Collider tag is "+other.gameObject.tag);
        if(other.gameObject.tag == "wall")
        {
            Debug.Log("THUD");
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "player")
        {
            Debug.Log("Hit "+other.gameObject.name);
            other.GetComponent<IDamageable>().ApplyDamage(bangDamage);
            Destroy(gameObject);
        }
        }else{Debug.Log("Spell Collided before fully formed.");}
    }
    void OnDestroy()
    {
        GameManager.Instance.enemyProjAmount--;
        if(bLastTurnMoved==GameManager.Instance.turnCount)
        {
            GameManager.Instance.enemyProjInit--;
        }
        Debug.Log("Enemy projectile amount is "+GameManager.Instance.enemyProjAmount);
        Debug.Log("Enemy projectile initiative is "+GameManager.Instance.enemyProjInit);
    }
}
