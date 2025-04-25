using UnityEngine;
using TMPro; // TextMeshProを使う場合
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;


public class CountdownManager : MonoBehaviour
{
    public　double imageDisplayTime = 4.0f;
    public Action OnCountdownComplete;
    [SerializeField] private Image countdownSpriteRenderer;
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private Sprite countdown3Sprite;
    [SerializeField] private Sprite countdown2Sprite;
    [SerializeField] private Sprite countdown1Sprite;
    [SerializeField] private Sprite startSprite;


    public async UniTask StartCountdownAsync()
    {
        countdownPanel.SetActive(true);
        countdownSpriteRenderer.sprite = countdown3Sprite;
        await UniTask.Delay(1000);

        countdownSpriteRenderer.sprite = countdown2Sprite;
        await UniTask.Delay(1000);

        countdownSpriteRenderer.sprite = countdown1Sprite;
        await UniTask.Delay(1000);

        countdownSpriteRenderer.sprite = startSprite; 
        await UniTask.Delay(1000);

        countdownSpriteRenderer.enabled = false;
        countdownPanel.SetActive(false);

        OnCountdownComplete?.Invoke(); // カウントダウン終了イベント

    }
}
