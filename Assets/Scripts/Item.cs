using NetcodePlus.Demo;
using System.Collections;
using UnityEngine;

public enum PowerupType
{
    None = 0,
    Heal = 10,
    Speed = 20, // value in percentage
    Invulnerable = 30
}

/// <summary>
/// Powerups that can be taken by tanks
/// </summary>
public class Item : MonoBehaviour
{
    public PowerupType type;
    public int _value = 10;
    public float _duration = 3f;
    public float _respawnDuration = 10f;

    [Header("Ref")]
    public SpriteRenderer sprite;
    public SphereCollider sphereCollider;
    public ParticleSystem pickupEffect; // 取得時のエフェクト（後で追加用）

    private bool _taken = false;
    //private float _timer = 0f;
    private float originalMoveSpeed; // 元の移動速度を保持するための変数

    private void Update()
    {
        //// Powerupが取られた後の処理
        //if (_taken && _timer > _respawnDuration)
        //    StartCoroutine(DeactivateAndRespawnCoroutine());

        //_timer += Time.deltaTime;
        if (!_taken) return;

        // アイテムが取られた後に再出現するコルーチンを開始
        StartCoroutine(DeactivateAndRespawnCoroutine());
        _taken = false;  // 1度だけ実行されるようにフラグをリセット
    }

    public void Take(TunkController tunkController)
    {
        if (!_taken)
        {
            _taken = true;
            Debug.Log($"Powerup Type: {type}"); // ここでtypeを確認
            ApplyBonus(tunkController); // 先に効果を適用
            StartCoroutine(DeactivateAndRespawnCoroutine());
        }

        // スピードアップの効果が適用される時間を設定
        //if (type == PowerupType.Speed)
        //{
        //    _duration = 3f;  // 必要に応じて設定
        //}
    }

    private void ApplyBonus(TunkController tunkController)
    {
        if (tunkController != null)
        {
            switch (type)
            {
                case PowerupType.Heal:
                    tunkController.AddHealth(_value);
                    break;
                case PowerupType.Speed:
                    //originalMoveSpeed = tunkController._moveSpeed; // 元のスピードを保存
                    //tunkController.IncreaseSpeed(_value);
                    //StartCoroutine(ApplySpeedBoost(tunkController, _duration));
                    ApplySpeedBoost(tunkController);
                    break;
                case PowerupType.Invulnerable:
                    tunkController.IsInvulnerable = true;
                    StartCoroutine(DisableInvulnerabilityAfterTime(tunkController, _duration));
                    break;
            }
        }
    }
    private void ApplySpeedBoost(TunkController tunkController)
    {
        originalMoveSpeed = tunkController._moveSpeed; // 元のスピードを保存
        tunkController._moveSpeed *= 2.5f;
        Debug.Log($"Speed Up Applied: {tunkController._moveSpeed}");
        StartCoroutine(ResetSpeedAfterDuration(tunkController));
    }
    private IEnumerator ResetSpeedAfterDuration(TunkController tunkController)
    {
        yield return new WaitForSeconds(_duration);
        tunkController._moveSpeed = originalMoveSpeed;
        Debug.Log($"Speed Reset: {tunkController._moveSpeed}");
    }

    private IEnumerator DisableInvulnerabilityAfterTime(TunkController tankController, float _duration)
    {
        yield return new WaitForSeconds(_duration);
        tankController.IsInvulnerable = false;
        Debug.Log("Invulnerability ended.");
    }

    private IEnumerator DeactivateAndRespawnCoroutine()
    {
        sprite.enabled = false; // アイテムを非表示にする
        sphereCollider.enabled = false; // コライダーを無効にする

        Debug.Log("Powerup deactivated, waiting to respawn...");

        yield return new WaitForSeconds(_duration); // 休止時間を待つ

        sprite.enabled = true; // アイテムを再表示する
        sphereCollider.enabled = true; // コライダーを再度有効にする

        _taken = false; // アイテムを取った状態をリセット
        //_timer = 0f;
        Debug.Log("Powerup respawned.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_taken) return;

        TunkController tank = other.GetComponentInParent<TunkController>();
        if (tank != null) Take(tank); // アイテムを取ったらスキルを発動
    }
}
