using UnityEngine;
using DG.Tweening;
using System.Collections;

public class IndividualEnemy : MonoBehaviour
{
    [SerializeField] int pLeft;
    [SerializeField] int pRight; 
    [SerializeField] int pUp; 
    [SerializeField] int pDown;
    private int stepsRemaining;
    [SerializeField] int currentNESW = 1;
    public int enemyHealth;
    public bool awoken;
    private SpriteRenderer rend;
    private bool myTurn;
    private int lastTurnMoved;
    private Transform trans;
    private Vector3 v3;
    private bool dead = false;

    void Awake(){
        DOTween.Init();
        enemyHealth = 3;
        awoken = false;
        dead = false;
        lastTurnMoved = -1;
        rend = GetComponent<SpriteRenderer>();
        rend.color = Color.white;
        //GameManager.Instance.enemyAmount++;
        GetSteps(false);
    }

    void Update()
    {
        if(GameManager.Instance.enemyTurn && lastTurnMoved!=GameManager.Instance.turnCount && !dead)
        {
            myTurn=true;
        }
        if (myTurn)
        {
            Move(currentNESW);
        }
    }

    public void Damage(int amount)
    {
        enemyHealth -= amount; 
//        StartCoroutine(ColourRed(0.8f));
        rend.color =Color.red;
        rend.DOColor(Color.white, 0.5f);
        if(enemyHealth <= 0){StartCoroutine(Die());}
    }

/*    private IEnumerator ColourRed(float duration)
    {
        rend.color =Color.red;
        yield return new WaitForSeconds(duration);
        rend.color =Color.white;
    }
*/
    void Move(int direction)
    {
        if(stepsRemaining!=0){
            if (direction == 1)
            {
                transform.position += Vector3.up;
            }
            if (direction == 2)
            {
                transform.position += Vector3.right;
            }
            if (direction == 3)
            {
                transform.position -= Vector3.up;
            }
            if (direction == 4)
            {
                transform.position -= Vector3.right;
            }
            stepsRemaining--;
            Debug.Log(stepsRemaining);
            endTurn();
        }
        else
        {
        currentNESW++;
        GetSteps(true);
        }
    }
            
    void GetSteps(bool move)
    {
        if(currentNESW>4){currentNESW=1;}
            switch (currentNESW)
                {
                case 1:
                    stepsRemaining = pUp;
                    break;

                case 2:
                    stepsRemaining = pRight;
                    break;

                case 3:
                    stepsRemaining = pDown;
                    break;
                
                case 4:
                    stepsRemaining = pLeft;
                    break;
                }
            if(move){
            Move(currentNESW);}
    }
    void endTurn()
    {
        myTurn =false;
        lastTurnMoved = GameManager.Instance.turnCount;
        GameManager.Instance.enemyInit++;
    }
    private IEnumerator Die()
    {
        if(!dead){
        dead = true;
        GameManager.Instance.enemyAmount--;
        if (lastTurnMoved == GameManager.Instance.turnCount){GameManager.Instance.enemyInit--;}
        rend.color = Color.red;
        rend.DOFade(0f, 2f);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        }
    }
}
