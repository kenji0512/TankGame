using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SEVolumeSlider : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private AudioClip testSE;

    public void OnPointerUp(PointerEventData eventData)
    {
        if (testSE != null)
            AudioManager.Instance.PlaySE(testSE);
    }
    //この eventData は「どのUIをクリックしたか」「どんな入力デバイスか」「どのボタンだったか」などの情報を持つオブジェクト。
    //今回はその情報は特に使っていないけど、将来的に「どのスライダーで離したか」などを区別したいときに役立つ。

}
