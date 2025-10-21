using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        // スライダー初期値反映
        bgmSlider.value = AudioManager.Instance.GetBGMVolume();
        seSlider.value = AudioManager.Instance.GetSEVolume();

        // 値変更時に音量反映
        bgmSlider.onValueChanged.AddListener(value => AudioManager.Instance.SetBGMVolume(value));
        seSlider.onValueChanged.AddListener(value => AudioManager.Instance.SetSEVolume(value));
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance == null) return;
        bgmSlider.onValueChanged.RemoveListener(OnBGMChanged);
        seSlider.onValueChanged.RemoveAllListeners();
    }
    private void OnBGMChanged(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }
}
