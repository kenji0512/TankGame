using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    [SerializeField] private float _initialDirectionY = 1.0f; // 初期射出方向
    [SerializeField] private double _delayTime = 0.5f;

    public Transform _shootpoint; // 弾を発射する位置
    public Transform _shootpointR; // 弾を発射する位置
    public Transform _shootpointH; // 弾を発射する位置
    public GameObject _shootEffectPrefab;
    public float shootEffectLifetime = 2f; // 発射エフェクトの寿命（秒）
    public double cooldownTime = 2.0f;

    public PlayerType shooterType;

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
    public async void Shoot(PlayerType shooterType, Vector3 direction, Quaternion rotation)
    {
        if (!canShoot) return;

        GameObject bulletObject = _bulletPool.Release(_normalBulletTag, _shootpoint.position, rotation);//_bulletPoolを一個でまとめるか種類数に応じた数作るか検討
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.transform.position = _shootpoint.position;
        bullet.transform.rotation = _shootpoint.rotation;

        // 弾を生成して初期位置と方向を設定
        if (bulletObject != null && _shootpoint != null)
        {
            if (tunkController.onPowerUp)
            {
                bullet.BulletdamageAmount += 10;
                Debug.Log($"-10damage");
            }
            if (bullet != null)
            {
                bullet.shooterType = shooterType;
                bullet.SetDirection(direction); // 発射方向を設定
            }
            StartCooldown();
        }
        else
        {
            Debug.LogError("NormalBullet Prefab or Shoot Point is not assigned.");
        }
        //発射エフェクトを生成
        PlayShootEffect(_shootpoint.position, _shootpoint.rotation);
    }
    public void RocketShoot(PlayerType shooter, Quaternion rotation)
    {
        if (!canShoot) return;
        // 発射を遅延させる
        DelayedShootAsync(shooter, rotation).Forget();
        StartCooldown();

        //発射エフェクトを生成
        PlayShootEffect(_shootpointR.position, _shootpointR.rotation);
    }
    public void HomingMissle(PlayerType shooter, Vector3 direction, Quaternion rotation)
    {
        if(!canShoot) return;
        StartCooldown();
        //shooter に応じてホーミング弾のタグを決定
        string selectedHomingTag = shooter switch
        {
            PlayerType.Player1 => _homingBulletTagA, // 青弾
            PlayerType.Player2 => _homingBulletTagB, // 赤弾
            _ => _homingBulletTagA, // デフォルトは青
        };
        GameObject bulletObject = _bulletPool.Release(selectedHomingTag, _shootpointH.position, rotation);

        if (bulletObject == null)
        {
            Debug.LogError("ObjectPool から HomingBullet が取得できませんでした！");
            return; // null なら即 return してアクセスを防ぐ
        }
        HomingMissile missile = bulletObject.GetComponent<HomingMissile>();
        if (missile != null)
        {
            missile.Initialize(shooter); // 追尾弾に発射主の情報を渡す
        }
        else
        {
            Debug.LogError("HomingBullet に HomingMissile コンポーネントが付いていません！");
        }
        if (_shootpointH != null && _shootEffectPrefab != null)
        {
            //発射エフェクトを生成
            PlayShootEffect(_shootpointH.position, _shootpointH.rotation);
        }
    }
    //private IEnumerator DelayedShoot(PlayerType shooter, Quaternion rotation)
    //{
    //    // 指定された遅延時間だけ待機
    //    yield return new WaitForSeconds(_delayTime);
    //    if (_shootpointR == null)
    //    {
    //        Debug.LogError("Rocket shoot point is not assigned.");
    //        yield break;
    //    }
    //    GameObject bulletObject = _bulletPool.Release(_rocketBulletTag, _shootpointR.position, rotation);

    //    if (bulletObject == null)
    //    {
    //        Debug.LogError("Rocket bullet could not be released from pool.");
    //        yield break;
    //    }
    //    bulletObject.transform.position = _shootpointR.position;
    //    bulletObject.transform.rotation = _shootpointR.rotation;

    //    SphereBooster boosterScript = bulletObject.GetComponent<SphereBooster>();
    //    if (boosterScript != null)
    //    {
    //        Vector3 launchDir = new Vector3(transform.forward.x, _initialDirectionY, transform.forward.z);
    //        boosterScript.Initialize(launchDir);
    //        boosterScript.shooterType = shooter;
    //    }
    //    else
    //    {
    //        Debug.LogError("SphereBooster script not found on rocket bullet.");
    //    }
    //}
    private async UniTaskVoid DelayedShootAsync(PlayerType shooter, Quaternion rotation)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(_delayTime));

        if (_shootpointR == null)
        {
            Debug.Log("Roket shoot is not assigned");
            return;
        }
        GameObject bulletObject = _bulletPool.Release(_rocketBulletTag, _shootpointR.position, rotation);
        if (bulletObject == null)
        {
            Debug.LogError("Rocket bullet could not be released from pool.");
            return;
        }
        bulletObject.transform.position = _shootpointR.position;
        bulletObject.transform.rotation = _shootpointR.rotation;

        SphereBooster boosterScript = bulletObject.GetComponent<SphereBooster>();
        if (boosterScript != null)
        {
            Vector3 launchDir = new Vector3(transform.forward.x, _initialDirectionY, transform.forward.z);
            boosterScript.Initialize(launchDir);
            boosterScript.shooterType = shooter;
        }
        else
        {
            Debug.LogError("SphereBooster script not found on rocket bullet.");
        }
    }
    private void PlayShootEffect(Vector3 position, Quaternion rotation)
    {
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, position, rotation);
            Destroy(shootEffect, shootEffectLifetime);
        }
    }
    private async UniTask StartCooldown()
    {
        canShoot = false;
        await UniTask.Delay(System.TimeSpan.FromSeconds(cooldownTime));
        canShoot = true;
    }
}
