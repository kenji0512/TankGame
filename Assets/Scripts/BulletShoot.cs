using System.Collections;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    public GameObject _bulletpre; // 弾のプレハブ
    public GameObject _roketBulletpre; // 弾のプレハブ
    public GameObject _homingBulletpre; // 弾のプレハブ
    public Transform _shootpoint; // 弾を発射する位置
    public Transform _shootpointR; // 弾を発射する位置
    public Transform _shootpointH; // 弾を発射する位置
    [SerializeField] Vector3 _initialDirection = new Vector3(1.0f, 1.0f, 0f); // 初期射出方向
    public GameObject _shootEffectPrefab;
    public float shootEffectLifetime = 2f; // 発射エフェクトの寿命（秒）
    [SerializeField] private float _delayTime = 0.5f;
    public PlayerType shooterType;
    public GameObject HomingBulletPrefab => _homingBulletpre; // プロパティとして公開


    public void Awake()
    {
        if (_shootpoint == null)
        {
            Debug.LogError("_shootpoint is not assigned.");
        }
    }
    public void Shoot(PlayerType shooterType, Vector3 direction)
    {
        //発射エフェクトを生成
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpoint.position, _shootpoint.rotation);
            Destroy(shootEffect, shootEffectLifetime); // エフェクトを一定時間後に消去
        }

        // 弾を生成して初期位置と方向を設定
        if (_bulletpre != null && _shootpoint != null)
        { 
            GameObject bullet = Instantiate(_bulletpre, _shootpoint.position, _shootpoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.shooterType = shooterType;
                bulletScript.SetDirection(direction); // 発射方向を設定
            }
        }
        else
        {
            Debug.LogError("Bullet Prefab or Shoot Point is not assigned.");
        }
    }
    public void RocketShoot(PlayerType shooter, Vector3 direction)
    {
        //発射エフェクトを生成
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpoint.position, _shootpoint.rotation);
            Destroy(shootEffect, shootEffectLifetime); // エフェクトを一定時間後に消去
        }

        // 発射を遅延させる
        StartCoroutine(DelayedShoot(shooter, direction));
    }
    public void HomingMissle(PlayerType shooter)
    {
        if (_homingBulletpre == null)
        {
            Debug.LogError("BulletShoot is not assigned.");
        }

        if (_shootpointH == null)
        {
            Debug.LogError("Shoot Point is not assigned.");
        }

        // 他の参照オブジェクトも同様に確認
        if (shooter == null)
        {
            Debug.LogError("Homing target is null.");
        }
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpoint.position, _shootpoint.rotation);
            Destroy(shootEffect, shootEffectLifetime); // エフェクトを一定時間後に消去
        }
        if (_homingBulletpre != null && _shootpointH != null)
        {
            GameObject homingBullet = Instantiate(_homingBulletpre, _shootpointH.position, _shootpointH.rotation);
            HomingMissile homingscript = homingBullet.GetComponent<HomingMissile>();
            if (homingscript != null)
            {
                homingscript.Initialize(shooter);
            }
            else
            {
                Debug.LogError("HomingMissile script not found on homing bullet prefab.");
            }
        }
        else
        {
            Debug.LogError("Homing Bullet Prefab or Shoot Point is not assigned.");
        }
    }
    private IEnumerator DelayedShoot(PlayerType shooter, Vector3 direction)
    {
        // 指定された遅延時間だけ待機
        yield return new WaitForSeconds(_delayTime);

        // 弾を生成して初期位置と方向を設定
        if (_roketBulletpre != null && _shootpointR != null)
        {
            GameObject roketBullet = Instantiate(_roketBulletpre, _shootpointR.position, _shootpointR.rotation);
            SphereBooster boosterScript = roketBullet.GetComponent<SphereBooster>();

            //roketBullet.GetComponent<SphereBooster>().Initialize(direction, shooter); // 発射方向を渡す
            if (boosterScript != null)
            {
                boosterScript.Initialize(_initialDirection); // direction を渡す
                boosterScript.shooterType = shooter;
                roketBullet.GetComponent<SphereBooster>().shooterType = shooter; // shooterType を渡す
            }
        }
        else
        {
            Debug.LogError("Rocket Bullet Prefab or Shoot Point is not assigned.");
        }
    }
}
