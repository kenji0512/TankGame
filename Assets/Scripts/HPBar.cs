using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPBar : MonoBehaviour
{
    [Header("�Q��")]
    [SerializeField] private Slider frontBar;   // ��������o�[�i�ԁj
    [SerializeField] private Slider delayedBar; // �x��Č���o�[�i���Ȃǁj

    public void UpdateHP(float currentHP, float maxHP)
    {
        float targetValue = currentHP / maxHP;

        // �������f�o�[
        frontBar.value = targetValue;

        // �x��Č�������o�[
        delayedBar.DOValue(targetValue, 0.5f)
                  .SetEase(Ease.OutQuad);
    }
}
