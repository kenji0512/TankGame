using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPBar : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private Slider _frontBar;   // 即時減るバー（赤）
    [SerializeField] private Slider _delayedBar; // 遅れて減るバー（黄など）

    public void UpdateHP(float currentHP, float maxHP)
    {
        float targetValue = currentHP / maxHP;

        // 即時反映バー
        _frontBar.value = targetValue;

        // 遅れて減少するバー
        _delayedBar.DOValue(targetValue, 0.5f)
                  .SetEase(Ease.OutQuad);
    }
}
