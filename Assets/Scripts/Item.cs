using System.Collections;
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
    private float originalMoveSpeed; // 元の移動速度を保持するための変数
    private float originalDamage;

    public void Take(TunkController tunkController)
    {
        if (!_taken)
        {
            _taken = true;
            ApplyBonus(tunkController); // 先に効果を適用
            StartCoroutine(DeactivateAndRespawnCoroutine());
        }
    }

    private void ApplyBonus(TunkController tunkController)
    {
        if (tunkController != null)
        {
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
    private void ApplyPowerBoost(TunkController tunkController)
    {
        Debug.Log($"{tunkController.name} にパワーアップ適用");
        tunkController.onPowerUp = true;
        originalDamage = tunkController._damageAmount;// 元のパワーを保存
        tunkController._damageAmount += _value;
        StartCoroutine(ResetPowerAfterDuration(tunkController));
    }
    private IEnumerator ResetPowerAfterDuration(TunkController tunkController)
    {
        yield return new WaitForSeconds(_duration);
        tunkController._damageAmount = originalDamage;
        tunkController.onPowerUp = false;
        Debug.Log($"false");
    }
    private void ApplySpeedBoost(TunkController tunkController)
    {
        originalMoveSpeed = tunkController._moveSpeed; // 元のスピードを保存
        tunkController._moveSpeed *= (1f + (_value / 100f));
        StartCoroutine(ResetSpeedAfterDuration(tunkController));
    }
    private IEnumerator ResetSpeedAfterDuration(TunkController tunkController)
    {
        yield return new WaitForSeconds(_duration);
        tunkController._moveSpeed = originalMoveSpeed;
    }
    private void ApplyInvulnerability(TunkController tunkController)
    {
        tunkController.IsInvulnerable = true;  // 無敵を有効にする
        StartCoroutine(DisableInvulnerabilityAfterTime(tunkController));
    }

    private IEnumerator DisableInvulnerabilityAfterTime(TunkController tankController)
    {
        yield return new WaitForSeconds(_duration);
        tankController.IsInvulnerable = false;  // 無敵を無効にする
    }

    private IEnumerator DeactivateAndRespawnCoroutine()
    {
        sprite.enabled = false; // アイテムを非表示にする
        sphereCollider.enabled = false; // コライダーを無効にする

        yield return new WaitForSeconds(_itemRespawnDuration); // 休止時間を待つ

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
