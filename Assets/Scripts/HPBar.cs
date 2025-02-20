using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPBar : MonoBehaviour
{
    public Slider hpSlider;

    public void UpdateHP(float currentHP, float maxHP)
    {
        hpSlider.DOValue(currentHP / maxHP, 0.5f).SetEase(Ease.OutQuad);
    }
}
