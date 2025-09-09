using UnityEngine;
using DG.Tweening;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    private AudioSource audioSource;
    private Tween fadeTween;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 既存のInstanceがある場合、新規生成オブジェクトを破棄
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource が未設定なら自動で取得
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    public void PlayBGM(AudioClip clip, float fadeTime = 1f, float volume = 1f, bool loop = true)
    {
        if (clip == null) return;

        // audioSource が消えてたら補完
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogWarning("AudioSource が見つかりません！");
                return;
            }
        }

        // 同じ曲ならスキップ
        if (audioSource.clip == clip) return;

        fadeTween?.Kill();
        audioSource.loop = loop; 

        fadeTween = audioSource.DOFade(0f, fadeTime).OnComplete(() =>
        {
            audioSource.clip = clip;
            audioSource.Play();
            audioSource.DOFade(volume, fadeTime);
        });
    }

    public void StopBGM(float fadeTime = 1f)
    {
        if (audioSource == null) return;

        fadeTween?.Kill();
        fadeTween = audioSource.DOFade(0f, fadeTime).OnComplete(() =>
        {
            audioSource.Stop();
            audioSource.clip = null;
        });
    }
}
