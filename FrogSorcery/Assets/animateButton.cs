using UnityEngine;
using DG.Tweening;

public class animateButton : MonoBehaviour
{
    public void Click()
    {
        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
    }
}
