using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    [SerializeField] float contrastBase;
    [SerializeField] float hueShiftBase;
    [SerializeField] float saturationBase;
    public bool isPolluted;
    private float contrastInterval;
    private float hueShiftInterval;
    private float saturationInterval;
    private ColorAdjustments colorAdjustments;
    void Start()
    {
        isPolluted = true;
        globalVolume.profile.TryGet(out colorAdjustments);
        colorAdjustments.contrast.value = contrastBase;
        colorAdjustments.hueShift.value = hueShiftBase;
        colorAdjustments.saturation.value = saturationBase;
        CalculateInterval();
    }

    void CalculateInterval()
    {
        float goal = (float) GameManager.Instance.enemyGoal;
        contrastInterval = contrastBase / goal;
        hueShiftInterval = hueShiftBase / goal;
        saturationInterval = saturationBase / goal;
    }

    public void ClearPollution()
    {
        if(isPolluted){
        colorAdjustments.contrast.value -= contrastInterval;
        colorAdjustments.hueShift.value -= hueShiftInterval;
        colorAdjustments.saturation.value -= saturationInterval;
        }

        if(colorAdjustments.contrast.value == 0 && colorAdjustments.hueShift.value == 0 && colorAdjustments.saturation.value == 0)
        {
            isPolluted = false;
            Debug.Log("Victory!");
        }
    }
}
