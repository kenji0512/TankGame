using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPBar : MonoBehaviour
{
    [Header("�Q��")]
    [SerializeField] private Slider _frontBar;   // ��������o�[�i�ԁj
    [SerializeField] private Slider _delayedBar; // �x��Č���o�[�i���Ȃǁj

    public void UpdateHP(float currentHP, float maxHP)
    {
        float targetValue = currentHP / maxHP;

        // �������f�o�[
        _frontBar.value = targetValue;

        // �x��Č�������o�[
        _delayedBar.DOValue(targetValue, 0.5f)
                  .SetEase(Ease.OutQuad);
    }
}
