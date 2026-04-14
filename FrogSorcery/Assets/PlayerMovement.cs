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
    private bool isAiming = false;
    private bool isTurn = false;
    private InputAction ribbit;
    private InputAction move;
    private InputAction cast;
    private BulletBang bangPrefab;
    private KnightBubble knightproj;
    public GameObject knightindicator;
    void Awake()
    {
        knightindicator = gameObject.transform.GetChild(0).gameObject;
        playerInput = GetComponent<PlayerInput>();
        rend = GetComponent<SpriteRenderer>();
        ribbit = playerInput.actions["Ribbit"];
        move = playerInput.actions["Move"];
        cast = playerInput.actions["Cast"];
        bangPrefab = Resources.Load<BulletBang>("Bang");
        knightproj = Resources.Load<KnightBubble>("KnightBubble");
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
//            gameObject.knightindicator.knightAim.Switch();
            Debug.Log("Ribbited");
            TurnOver();
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        if(!isTurn){
            Debug.Log("Not Player Turn");
            Debug.Log("Enemy Amount "+GameManager.Instance.enemyAmount);
            Debug.Log("Enemy Init "+GameManager.Instance.enemyInit);
            Debug.Log("Bullet Amount "+GameManager.Instance.bulletAmount);
            Debug.Log("Bullet Init "+GameManager.Instance.bulletInit);
            Debug.Log("spellComplete "+GameManager.Instance.spellComplete);
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
            Debug.Log((Vector3)direction);
            switch (direction.x){
                    case 1:
                    TurnOver();
                    break;

                    case -1:
                    TurnOver();
                    break;
            }
            switch (direction.y){
                    case 1:
                    GameManager.Instance.spellComplete = false;
                    AimKnight();
                    rend.material.color = Color.green;
                    isAiming = true;
                    isCasting = false;
                    break;

                    case -1:
                    TurnOver();
                    break;
                }
        }
    }
    }

    private void AimKnight()
    {
        //StartCoroutine(SpellKnight((Vector3)direction));
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

    private IEnumerator SpellKnight(Vector3 direction)
    {
        Debug.Log("Spell cast "+direction);

        if (bangPrefab != null)
        {

            KnightBubble knight = Instantiate(knightproj, transform.position, Quaternion.identity);
            knight.direction = new Vector3(direction.x, direction.y, direction.z);
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
