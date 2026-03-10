using UnityEngine;

public class BulletBang : MonoBehaviour
{
    public Vector3 direction;

    void Start()
    {   
        
        Debug.Log("Prefab Spawned "+ direction);
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
}
