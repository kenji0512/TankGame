using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;   // 弾の移動速度
    [SerializeField] private float _lifetime = 5f; // 弾の寿命（秒）
    [SerializeField] private GameObject _hitEffectPrefab; // 衝突エフェクトのプレハブ
    [SerializeField] private float _bulletdamageAmount = 10; // ダメージ量
    public PlayerType shooterType; // 発射者のプレイヤータイプ
    private Vector3 _direction;  // 弾の移動方向
    public ObjectPool myPool; // プールから渡しておく


    public float BulletdamageAmount { get => _bulletdamageAmount; set => _bulletdamageAmount = value; }

    protected virtual void Start()
    {
        if (myPool == null)
        {
            Debug.LogWarning($"Bullet: myPool が未設定です（{gameObject.name}）");
        }
        StartCoroutine(AutoReturnToPool());

        //Destroy(gameObject, _lifetime);
    }
    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized; // 弾の移動方向を正規化
    }
    protected virtual void Update()
    {
        // 弾を前方に移動させる
        _direction = _direction.normalized;
        transform.position += _direction * _speed * Time.deltaTime;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤータグで判定
        {
            Debug.Log($"Player hit");

            TunkController hitPlayer = other.GetComponent<TunkController>(); // プレイヤーがタグ付きであればコンポーネント取得
            if (hitPlayer == null)
            {
                Debug.LogError("ヒットしたオブジェクトが TunkController を持っていません！");
                return;
            }
            if (hitPlayer != null && hitPlayer.playerType != shooterType)
            {
                HandleCharacterCollision(hitPlayer);
                ReturnToPool();
            }
        }
        else if (other.CompareTag("BreakableWall"))
        {
            var breakableWall = other.GetComponent<BreakableWall>();
            if (breakableWall != null)
            {
                HandleWallCollision(breakableWall);
                ReturnToPool();
            }
            else
            {
                ReturnToPool();
            }
            Debug.Log($"NormalBullet hit {breakableWall}");

        }
    }
    protected virtual void HandleWallCollision(BreakableWall breakableWall)
    {
        breakableWall.Damage(); // 壁にダメージを与える
        CreateHitEffect(); // 衝突エフェクトを生成
        //Destroy(gameObject); // 弾を破壊
    }

    protected virtual void HandleCharacterCollision(TunkController hitPlayer)
    {
        hitPlayer.TakeDamage(BulletdamageAmount); // プレイヤーにダメージを与える
        CreateHitEffect(); // 衝突エフェクトを生成
        ReturnToPool(); // 弾を破壊
    }

    protected void CreateHitEffect()
    {
        if (_hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(_hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, 2f); // エフェクトを2秒後に消去
        }
    }
    protected void ReturnToPool()
    {
        if(myPool != null)
        {
            myPool.Catch("NormalBullet", gameObject);
        }
        else
        {
            Debug.LogWarning("myPool is null, destroying instead.");
            //Destroy(gameObject);
        }
    }
    private System.Collections.IEnumerator AutoReturnToPool()
    {
        yield return new WaitForSeconds(_lifetime);
        ReturnToPool();
    }
}
