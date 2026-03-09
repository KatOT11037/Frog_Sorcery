using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Tilemap walkmap;
    [SerializeField] private Tilemap covermap;
    [SerializeField] private Tilemap collmap;
    private PlayerInput playerInput;
    private bool isMoving = false;
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private float gridSize = 1f;
    private InputAction ribbit;
    private InputAction move;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        ribbit = playerInput.actions["Ribbit"];
        move = playerInput.actions["Move"];
    }
    void Update()
    {
        if (!isMoving)
        {
//            playerInput = Input.GetKey;
//            if(Input.GetKey(KeyCode));
        }
        
    }

    private void OnEnable()
    {
        ribbit.started += Ribbit;
        move.performed += ctx => MovePlayer(ctx.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        ribbit.started -= Ribbit;
        move.performed -= ctx => MovePlayer(ctx.ReadValue<Vector2>());
    }

    private void Ribbit(InputAction.CallbackContext ctx)
    {
        Debug.Log("Ribbited");
    }

    private void MovePlayer(Vector2 direction)
    {
        isMoving = true;

        if (CanMove(direction))
        {
            Debug.Log("Moving");
            transform.position += (Vector3)direction;
        }
        isMoving = false;
    }
    
    private bool CanMove(Vector2 direction)
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
