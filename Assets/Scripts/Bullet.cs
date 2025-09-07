using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;   // 弾の移動速度
    [SerializeField] private float _lifetime = 5f; // 弾の寿命（秒）
    [SerializeField] private GameObject _hitEffectPrefab; // 衝突エフェク トのプレハブ
    [SerializeField] private float _bulletdamageAmount = 10; // ダメージ量

    public PlayerType shooterType; // 発射者のプレイヤータイプ
    public ObjectPool myPool; // プールから渡しておく

    private Vector3 _direction;  // 弾の移動方向
    private CancellationTokenSource _cancellationTokenSource;

    private void OnEnable()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        StartLifetimeTimerAsync(_cancellationTokenSource.Token).Forget();

        TrailRenderer trail = GetComponent<TrailRenderer>();
        if (trail != null) trail.Clear();
    }
    private async UniTaskVoid StartLifetimeTimerAsync(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_lifetime), cancellationToken: token);
            if (gameObject.activeInHierarchy)
            {
                ReturnToPool();
            }
        }
        catch (OperationCanceledException)
        {
            // 無効化された時に発生するけど、無視してOK
        }
    }
    private void OnDisable()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }

    public float BulletdamageAmount { get => _bulletdamageAmount; set => _bulletdamageAmount = value; }

    protected virtual void Start()
    {
        if (myPool == null)
        {
            Debug.LogWarning($"Bullet: myPool が未設定です（{gameObject.name}）");
        }
        //StartCoroutine(AutoReturnToPool());
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
            Debug.Log($"[DEBUG] {gameObject.name} hit {other.name}");

            TunkController hitPlayer = other.GetComponentInParent<TunkController>(); // プレイヤーがタグ付きであればコンポーネント取得
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
        if (myPool != null)
        {
            myPool.Catch("NormalBullet", gameObject);
        }
        else
        {
            Debug.LogWarning("myPool is null, destroying instead.");
            Destroy(gameObject);
        }
    }
}
