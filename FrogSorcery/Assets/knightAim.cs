using UnityEngine;
using UnityEngine.InputSystem;

public class KnightAim : MonoBehaviour
{
    public bool isEnabled = false;
    private SpriteRenderer aimrenderer;
    private Vector3 mousePos;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    private InputAction click;
    public int aimX;
    public int aimY;
    public int aimPiv;
    private Vector3Int aimDirection;
    public int castDirection;
    [SerializeField] public Sprite[] sprites;
    void Awake()
    {
        castDirection = -1;

        playerInput = GetComponentInParent<PlayerInput>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        aimrenderer = GetComponent<SpriteRenderer>();
        click = playerInput.actions["Click"];
    }

    private void OnEnable()
    {
        if(click != null)
        {
        click.started += Click;
        Debug.Log("Clicking");
        }else{
        Debug.Log("Clickless");
        }
    }
    private void OnDisable()
    {
        click.started -= Click;
    }
    void Update()
    {
        mousePos = Input.mousePosition;
        if (mousePos.x < (Screen.width/2))
        {
            aimX = -1;
        }
        else
        {
            aimX = 1;
        }
        if (mousePos.y < (Screen.height/2))
        {
            aimY = -1;
        }
        else
        {
            aimY = 1;
        }

        if(Mathf.Abs(mousePos.x - (Screen.width/2)) > Mathf.Abs(mousePos.y - (Screen.height/2)))
        {
            aimPiv = 1;
        }
        else
        {
            aimPiv = -1;
        }

        aimDirection = new Vector3Int(aimX, aimY, aimPiv);
        //Debug.Log(aimDirection.ToString());

        switch (aimDirection.ToString()){
            case "(1, 1, -1)":
            castDirection = 0;
            break;

            case "(1, 1, 1)":
            castDirection = 1;
            break;

            case "(1, -1, 1)":
            castDirection = 2;
            break;

            case "(1, -1, -1)":
            castDirection = 3;
            break;

            case "(-1, -1, -1)":
            castDirection = 4;
            break;

            case "(-1, -1, 1)":
            castDirection = 5;
            break;

            case "(-1, 1, 1)":
            castDirection = 6;
            break;

            case "(-1, 1, -1)":
            castDirection = 7;
            break;
        }

        aimrenderer.sprite = sprites[castDirection];
    }

    private void Click(InputAction.CallbackContext ctx)        
    {
        Debug.Log("Cluck");
        playerMovement.AimKnight(castDirection);
        gameObject.SetActive(false);
    }
}
