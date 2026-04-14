using UnityEngine;
using UnityEngine.U2D.Animation;

public class knightAim : MonoBehaviour
{
    public bool isEnabled = false;
    void Awake()
    {
        Renderer renderer = GetComponent<SpriteRenderer>();
        SpriteLibrary lib = GetComponent<SpriteLibrary>();
        SpriteResolver resolver = GetComponent<SpriteResolver>();
    }
    void OnEnable() {
        
    }
    void Update()
    {
        
    }

    public void Switch(){
        Debug.Log("Swutch");
    }
}
