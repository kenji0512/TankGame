using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BulletShoot : MonoBehaviour
{
    [SerializeField] private float _initialDirectionY = 1.0f; // 初期射出方向
    [SerializeField] private double _delayTime = 0.5f;
    [SerializeField] private Image _cooldownGauge;

    public Transform _shootpoint; // 弾を発射する位置
    public Transform _shootpointR; // 弾を発射する位置
    public Transform _shootpointH; // 弾を発射する位置
    public GameObject _shootEffectPrefab;
    public float shootEffectLifetime = 2f; // 発射エフェクトの寿命（秒）
    public double cooldownTime = 2.0f;

    public PlayerType shooterType;
    private IShootStrategy _currentStrategy;
    public void SetStrategy(IShootStrategy strategy)
    {
        _currentStrategy = strategy;
    }
    public void ShootByStrategy(PlayerType shooterType, Vector3 direction, Quaternion rotation)
    {
        if (GameManager.Instance.currentState != GameState.Playing) return; 

        if (!canShoot) return;

        if (_currentStrategy != null && canShoot)
        {
            _currentStrategy.Shoot(this, shooterType, direction, rotation);
        }
    }
    // 既存のフィールドやメソッドは公開プロパティとしてアクセスさせる
    public ObjectPool BulletPool => _bulletPool;
    public Transform ShootPoint => _shootpoint;
    public Transform ShootPointR => _shootpointR;
    public Transform ShootPointH => _shootpointH;
    public float InitialDirectionY => _initialDirectionY;
    public TunkController TunkController => tunkController;
    public bool CanShoot => canShoot;


    [SerializeField] TunkController tunkController;
    [SerializeField] private ObjectPool _bulletPool;

    [Header("弾のタグ設定")]
    [SerializeField] private string _normalBulletTag = "NormalBullet";
    [SerializeField] private string _rocketBulletTag = "RocketBullet";
    [SerializeField] private string _homingBulletTagA = "HomingBullet_Blue";
    [SerializeField] private string _homingBulletTagB = "HomingBullet_Red";

    private bool canShoot = true;

    public void Awake()
    {
        if (_shootpoint == null)
        {
            Debug.LogError("_shootpoint is not assigned.");
        }
    }

    public async Task PlayShootEffectAsync(Vector3 position, Quaternion rotation)
    {
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, position, rotation);
            await UniTask.Delay(System.TimeSpan.FromSeconds(shootEffectLifetime));
            Destroy(shootEffect);
        }
    }
    public async UniTask StartCooldown()
    {
        canShoot = false;

        // ゲージの初期値（フル）
        if (_cooldownGauge != null)
            _cooldownGauge.gameObject.SetActive(false);
        
        await UniTask.Delay(100); // 少しだけ非表示のまま待機（演出用）

        if (_cooldownGauge != null)
        {
            _cooldownGauge.gameObject.SetActive(true);
            _cooldownGauge.fillAmount = 0f;
        }

        float timer = 0f;
        while (timer < cooldownTime)
        {
            timer += Time.deltaTime;

            if (_cooldownGauge != null)
            {
                float t = Mathf.Clamp01(timer / (float)cooldownTime);
                _cooldownGauge.fillAmount = t;
            }

            await UniTask.Yield(); // フレームごとに待機
        }
        if (_cooldownGauge != null)
            _cooldownGauge.fillAmount = 1f;

        canShoot = true;
    }
}

