using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletBang : MonoBehaviour
{
    public Vector3 direction;
    private Tilemap walkmap;
    private Tilemap covermap;
    private Tilemap collmap;
    //private Tilemap[] tilemaps;
    public int myTurn; 

    void Start()
    {   
        Debug.Log("Prefab Spawned "+ direction);
        GameManager.Instance.bulletAmount++;
        myTurn = GameManager.Instance.bulletAmount;
        //Made a weird way of each instance referencing the tilemaps, for collision, 
        //and because just porting over the player CanMove bool seemed simple and it needs them, 
        //before realizing that i could probably just use a collider and Tilemap collider and it would perform better

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
        
    }

    bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = walkmap.WorldToCell(transform.position + (Vector3)direction);
        if(!(walkmap.HasTile(gridPosition)||covermap.HasTile(gridPosition)))
        {
            Debug.Log("No Walkable Tile");
            return false;
        }else if(collmap.HasTile(gridPosition))
            {
            Debug.Log("Wall in the way, boss");
            return false;
            }
        else
            {
            Debug.Log("Can Move");
            return true;
            }
    }

    void OnDestroy()
    {
        GameManager.Instance.bulletAmount--;
    }
}
