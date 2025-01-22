using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;   // 弾の移動速度
    [SerializeField] private float lifetime = 5f; // 弾の寿命（秒）
    [SerializeField] private GameObject _hitEffectPrefab; // 衝突エフェクトのプレハブ
    [SerializeField] private int damageAmount = 10; // ダメージ量
    public PlayerType shooterType; // 発射者のプレイヤータイプ
    private Vector3 _direction;  // 弾の移動方向

    protected virtual void Start()
    {
        // 一定時間後に弾を自動で破壊
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized; // 弾の移動方向を正規化
    }
    private void Update()
    {
        // 弾を前方に移動させる
        _direction = _direction.normalized;
        transform.position += _direction * speed * Time.deltaTime;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤータグで判定
        {
            var hitPlayer = other.GetComponent<TunkController>(); // プレイヤーがタグ付きであればコンポーネント取得
            if (hitPlayer != null && hitPlayer._playerType != shooterType)
            {
                HandleCharacterCollision(hitPlayer);
                Debug.Log($"Bullet hit {hitPlayer.gameObject.name} and dealt damage! Remaining Health: {hitPlayer.GetCurrentHealth()}");
                Destroy(gameObject); // 弾を消去
            }
        }
        else if (other.CompareTag("BreakableWall"))
        {
            var breakableWall = other.GetComponent<BreakableWall>();
            if (breakableWall != null)
            {
                HandleWallCollision(breakableWall);
            }
            Debug.Log($"Bullet hit {breakableWall}");
        }
    }
    protected virtual void HandleWallCollision(BreakableWall breakableWall)
    {
        breakableWall.Damage(); // 壁にダメージを与える
        CreateHitEffect(); // 衝突エフェクトを生成
        Destroy(gameObject); // 弾を破壊
    }

    protected virtual void HandleCharacterCollision(TunkController hitPlayer)
    {
        hitPlayer.TakeDamage(damageAmount); // プレイヤーにダメージを与える
        CreateHitEffect(); // 衝突エフェクトを生成
        Destroy(gameObject); // 弾を破壊
    }

    protected void CreateHitEffect()
    {
        if (_hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(_hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, 2f); // エフェクトを2秒後に消去
        }
    }
}
