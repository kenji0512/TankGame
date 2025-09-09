// DamageFlash.cs
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Renderer[] targets;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private bool includeChildren = true;

    Color[] _originalColors;
    Material[] _mats;

    void Awake()
    {
        var renderers = (targets != null && targets.Length > 0)
            ? targets
            : (includeChildren ? GetComponentsInChildren<Renderer>(true) : new[] { GetComponent<Renderer>() });

        // 必要なマテリアルを確保（ここでは簡単のため materials を使用）
        var matsList = new System.Collections.Generic.List<Material>();
        foreach (var r in renderers)
        {
            if (r == null) continue;
            matsList.AddRange(r.materials);
        }
        _mats = matsList.ToArray();

        _originalColors = new Color[_mats.Length];
        for (int i = 0; i < _mats.Length; i++)
            _originalColors[i] = _mats[i].color;
    }

    public async UniTask FlashAsync(CancellationToken token)
    {
        for (int i = 0; i < _mats.Length; i++)
            _mats[i].color = flashColor;

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(flashDuration), cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            // 破棄時キャンセルは無視
        }

        for (int i = 0; i < _mats.Length; i++)
            _mats[i].color = _originalColors[i];
    }
}
