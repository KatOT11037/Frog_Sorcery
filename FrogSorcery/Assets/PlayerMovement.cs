using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Tilemap walkmap;
    [SerializeField] private Tilemap covermap;
    [SerializeField] private Tilemap collmap;
    private PlayerInput playerInput;
    private Renderer rend;
    private bool isMoving = false;
    private bool isCasting = false;
    private bool isTurn = false;
    private InputAction ribbit;
    private InputAction move;
    private InputAction cast;
    private BulletBang bangPrefab;
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rend = GetComponent<SpriteRenderer>();
        ribbit = playerInput.actions["Ribbit"];
        move = playerInput.actions["Move"];
        cast = playerInput.actions["Cast"];
        bangPrefab = Resources.Load<BulletBang>("Bang");
    }

    private void OnEnable()
    {
        PlayerTurn();
        ribbit.started += Ribbit;
        move.performed += ctx => MovePlayer(ctx.ReadValue<Vector2>());
        cast.started += Cast;
    }

    private void OnDisable()
    {
        ribbit.started -= Ribbit;
        move.performed -= ctx => MovePlayer(ctx.ReadValue<Vector2>());
        cast.started -= Cast;
    }

    private void Ribbit(InputAction.CallbackContext ctx)
    {
        if(!isTurn){
            Debug.Log("Not Player Turn");
            return;}
            else{

            Debug.Log("Ribbited");
            TurnOver();
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        if(!isTurn){
            Debug.Log("Not Player Turn");
            Debug.Log(GameManager.Instance.enemyAmount);
            Debug.Log(GameManager.Instance.enemyInit);
            return;}
            else{
        if(!isCasting)
        {
            isMoving = true;
            if (CanMove(direction))
            {
                Debug.Log("Moving");
                transform.position += (Vector3)direction;
                TurnOver();
            }
            isMoving = false;
        }
        else
        {
            GameManager.Instance.spellComplete = false;
            StartCoroutine(Spell((Vector3)direction));
            rend.material.color = Color.white;
            isCasting = false;
            TurnOver();
        }
    }
    }

    private void Cast(InputAction.CallbackContext ctx)
    {
        if(!isTurn){
            Debug.Log("Not Player Turn");
            Debug.Log(GameManager.Instance.spellComplete);
            return;}
            else{
        if (!isMoving)
        {
            isCasting = true;
            rend.material.color = Color.blue;
        }
        }
    }

    private IEnumerator Spell(Vector3 direction)
    {
        Debug.Log("Spell cast "+direction);

        if (bangPrefab != null)
        {

            BulletBang bangproj = Instantiate(bangPrefab, transform.position, Quaternion.identity);
            bangproj.direction = new Vector3(direction.x, direction.y, direction.z);
            Debug.Log("Bang spawned");
        }
        else
        {
            Debug.LogWarning("No bang. where is bang");
        }
//        if(GameManager.Instance.spellComplete){yield break;}
        yield break;
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

    void TurnOver()
    {
        isTurn = false;
        GameManager.Instance.TurnEnd();
    }

    public void PlayerTurn()
    {
        isTurn = true;
        Debug.Log("Player Turn "+isTurn+gameObject);
    }
}
