using NetcodePlus.Demo;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    public int _value;
    public float _duration;
    public float _respawnDuration = 10f;

    [Header("Ref")]
    public GameObject mesh;
    public ParticleSystem pickupEffect; // 取得時のエフェクト（後で追加用）


    private bool _taken = false;
    private float _timer = 0f;

    private void Update()
    {
        // Powerupが取られた後の処理
        if (_taken)
        {
            _timer += Time.deltaTime;
            if (_timer > _respawnDuration)
                Respawn();
        }
    }

    public void Take(TunkController tunkController)
    {
        if (!_taken)
        {
            _taken = true;
            mesh.SetActive(false); // メッシュを非表示にして、アイテムが取られたことを示す
            ApplyBonus(tunkController);
        }
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
                    tunkController.IncreaseSpeed(_value);
                    StartCoroutine(ApplySpeedBoost(tunkController, _duration));
                    break;
                case PowerupType.Invulnerable:
                    tunkController.IsInvulnerable = true;
                    StartCoroutine(DisableInvulnerabilityAfterTime(tunkController, _duration));
                    break;
            }
        }
    }
    private IEnumerator ApplySpeedBoost(TunkController tankController, float duration)
    {
        float originalSpeed = tankController._moveSpeed;
        tankController._moveSpeed *= (1 + (_value / 100f)); // スピードアップ
        yield return new WaitForSeconds(duration);
        tankController.IncreaseSpeed(-_value); // 元の速度に戻す
    }

    private IEnumerator DisableInvulnerabilityAfterTime(TunkController tankController, float duration)
    {
        yield return new WaitForSeconds(duration);
        tankController.IsInvulnerable = false;
    }
    public void Respawn()
    {
        _taken = false;
        mesh.SetActive(true); // アイテムを再表示する
        _timer = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_taken)
            return;

        TunkController tank = other.GetComponentInParent<TunkController>();
        if (tank != null)
        {
            Take(tank); // アイテムを取ったらスキルを発動
        }
    }
}
