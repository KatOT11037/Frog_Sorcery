using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public Tilemap walkmap;
    [SerializeField] public Tilemap covermap;
    [SerializeField] public Tilemap collmap;
    [SerializeField] public Vector3[] VDistToPlayer;
    private Animator animator;
    public PlayerInput playerInput;
    public KnightAim knightAim;
    private Renderer rend;
    private int animState;
    private bool isMoving = false;
    private bool isCasting = false;
    private bool isAiming = false;
    private bool isTurn = false;
    private InputAction ribbit;
    private InputAction move;
    private InputAction cast;
    public Vector3[] knightVulnerable; 
    private BulletBang bangPrefab;
    private KnightBubble knightproj;
    public GameObject knightindicator;
    void Awake()
    {
        knightVulnerable = new Vector3[8];
        knightindicator = gameObject.transform.GetChild(0).gameObject;
        knightAim = knightindicator.GetComponent<KnightAim>();
        Debug.Log(knightindicator.activeInHierarchy);
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        ribbit = playerInput.actions["Ribbit"];
        move = playerInput.actions["Move"];
        cast = playerInput.actions["Cast"];
        bangPrefab = Resources.Load<BulletBang>("Bang");
        knightproj = Resources.Load<KnightBubble>("KnightBubble");
    }

    private void OnEnable()
    {
        animState = 0;
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
            Debug.Log("Enemy Amount "+GameManager.Instance.enemyAmount);
            Debug.Log("Enemy Init "+GameManager.Instance.enemyInit);
            Debug.Log("Bullet Amount "+GameManager.Instance.bulletAmount);
            Debug.Log("Bullet Init "+GameManager.Instance.bulletInit);
            Debug.Log("spellComplete "+GameManager.Instance.spellComplete);
            return;}
            else{
        if(!isCasting && !isAiming)
        {
            isMoving = true;
            if (CanMove(direction))
            {
                Debug.Log("Moving");
                Debug.Log(direction.ToString());
                transform.position += (Vector3)direction;
                switch (direction.ToString())
                {
                    case "(0.00, -1.00, 0.00)":
                animator.SetInteger("animState", 0);
                break;

                case "(0.00, 1.00, 0.00)":
                animator.SetInteger("animState", 3);
                break;

                case "(1.00, 0.00, 0.00)":
                animator.SetInteger("animState", 2);
                break;

                case "(-1.00, 0.00, 0.00)":
                animator.SetInteger("animState", 1);
                break;
                }

                TurnOver();
            }
            isMoving = false;
        }
/*        else
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
                    knightindicator.SetActive(true);
                    rend.material.color = Color.green;
                    isAiming = true;
                    isCasting = false;
                    break;

                    case -1:
                    TurnOver();
                    break;
                }
        }*/
    }
    }

    public void AimKnight(int direction)
    {
        isAiming = false;
        rend.material.color = Color.white;
        StartCoroutine(SpellKnight(direction));
    }

    private void Cast(InputAction.CallbackContext ctx)
    {
        if(!isTurn){
            Debug.Log("Not Player Turn");
            Debug.Log(GameManager.Instance.spellComplete);
            return;}
            else{
        if (!isMoving && !isAiming)
        {
            //isCasting = true;
            //rend.material.color = Color.blue;
            GameManager.Instance.spellComplete = false;
            knightindicator.SetActive(true);
            rend.material.color = Color.blue;
            isAiming = true;
            isCasting = false;
        }
        }
    }

    private IEnumerator SpellKnight(int direction)
    {
        Debug.Log("Spell cast "+direction);

        if (knightproj!= null)
        {
            KnightBubble knight = Instantiate(knightproj, transform.position, Quaternion.identity);
            knight.aimDirection = direction;
            Debug.Log("Bang spawned");
            TurnOver();
        }
        else
        {
            Debug.LogWarning("No bubble. where is bubble");
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
        UpdateVulnerable();
        isTurn = false;
        GameManager.Instance.TurnEnd();
    }

    void UpdateVulnerable()
    {
        Debug.Log(knightVulnerable.Length);
        for (int i = 0; i <= 7; i++)
        {
            knightVulnerable[i] = transform.position-(VDistToPlayer[i]);
        }
/*        knightVulnerable[0] = new Vector3(transform.position.x + 1, transform.position.y + 2, 0);
        knightVulnerable[1] = new Vector3(transform.position.x + 2, transform.position.y + 1, 0);
        knightVulnerable[2] = new Vector3(transform.position.x + 2, transform.position.y - 1, 0);
        knightVulnerable[3] = new Vector3(transform.position.x + 1, transform.position.y - 2, 0);
        knightVulnerable[4] = new Vector3(transform.position.x - 1, transform.position.y - 2, 0);
        knightVulnerable[5] = new Vector3(transform.position.x - 2, transform.position.y - 1, 0);
        knightVulnerable[6] = new Vector3(transform.position.x - 2, transform.position.y + 1, 0);
        knightVulnerable[7] = new Vector3(transform.position.x - 1, transform.position.y + 2, 0); */
    }
    public void PlayerTurn()
    {
        isTurn = true;
        Debug.Log("Player Turn "+isTurn+gameObject);
    }

    public void OnDestroy()
    {
        isTurn = false;
        Debug.Log("Dead :(");
    }
}
