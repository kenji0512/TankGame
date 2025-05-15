using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public enum PowerupType
{
    None = 0,
    Power = 10,
    Speed = 20, // value in percentage
    Invulnerable = 30
}

/// <summary>
/// Powerups that can be taken by tanks
/// </summary>
public class Item : MonoBehaviour
{
    public PowerupType type;
    public float _value = 10;
    public float _duration = 3f;
    public float _itemRespawnDuration = 10f;

    [Header("Ref")]
    public SpriteRenderer sprite;
    public SphereCollider sphereCollider;
    public ParticleSystem pickupEffect; // 取得時のエフェクト（後で追加用）

    private bool _taken = false;
    private float _originalMoveSpeed; // 元の移動速度を保持するための変数
    private float _originalDamage;
    private ParticleSystem currentEffect; // 現在の効果のエフェクト


    public void Take(TunkController tunkController)
    {
        if (!_taken)
        {
            _taken = true;
            ApplyBonus(tunkController); // 先に効果を適用
            DeactivateAndRespawnCoroutine().Forget();
        }
    }

    private void ApplyBonus(TunkController tunkController)
    {
        if (tunkController != null)
        {
            if (pickupEffect != null)
            {
                currentEffect = Instantiate(pickupEffect, tunkController.transform.position + Vector3.zero, Quaternion.identity);
                currentEffect.transform.SetParent(tunkController.transform); // 効果エフェクトがプレイヤーに追従するように設定

            }

            switch (type)
            {
                case PowerupType.Power:
                    ApplyPowerBoost(tunkController);
                    break;
                case PowerupType.Speed:
                    ApplySpeedBoost(tunkController);
                    break;
                case PowerupType.Invulnerable:
                    ApplyInvulnerability(tunkController);
                    break;
            }
        }
    }

    private async UniTaskVoid ApplyPowerBoost(TunkController tunkController)
    {
        Debug.Log($"{tunkController.name} にパワーアップ適用");
        tunkController.onPowerUp = true;
        _originalDamage = tunkController._damageAmount;
        tunkController._damageAmount += _value;

        await UniTask.Delay(TimeSpan.FromSeconds(_duration));

        tunkController._damageAmount = _originalDamage;
        tunkController.onPowerUp = false;
        currentEffect.Stop();
        Destroy(currentEffect);
        Debug.Log($"{currentEffect} destroy");
    }

    private async UniTaskVoid ApplySpeedBoost(TunkController tunkController)
    {
        if (tunkController.onSpeedUp) return;
        tunkController.onSpeedUp = true;
        _originalMoveSpeed = tunkController._moveSpeed; // 元のスピードを保存
        tunkController._moveSpeed *= (1f + (_value / 100f));

        await UniTask.Delay(TimeSpan.FromSeconds(_duration));
        tunkController._moveSpeed = tunkController._defaultMoveSpeed;
        tunkController.onSpeedUp = false;
        currentEffect.Stop(); // エフェクトを停止
        Destroy(currentEffect);
        Debug.Log(currentEffect + $"destroy");
    }
    private async UniTaskVoid ApplyInvulnerability(TunkController tunkController)
    {
        tunkController.IsInvulnerable = true;  // 無敵を有効にする

        await UniTask.Delay(TimeSpan.FromSeconds(_duration));

        tunkController.IsInvulnerable = false;  // 無敵を無効にする
        currentEffect.Stop(); // エフェクトを停止
        Destroy(currentEffect);
        Debug.Log(currentEffect + $"destroy");
    }

    private async UniTaskVoid DeactivateAndRespawnCoroutine()
    {
        sprite.enabled = false; // アイテムを非表示にする
        sphereCollider.enabled = false; // コライダーを無効にする

        await UniTask.Delay(TimeSpan.FromSeconds(_itemRespawnDuration));// 休止時間を待つ

        sprite.enabled = true; // アイテムを再表示する
        sphereCollider.enabled = true; // コライダーを再度有効にする

        _taken = false; // アイテムを取った状態をリセット
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_taken) return;

        TunkController tank = other.GetComponentInParent<TunkController>();
        if (tank != null) Take(tank); // アイテムを取ったらスキルを発動
    }
}
