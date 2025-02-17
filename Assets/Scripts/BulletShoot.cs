using System.Collections;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    [SerializeField] private float _initialDirectionY = 1.0f; // 初期射出方向
    [SerializeField] private float _delayTime = 0.5f;

    public Bullet _bulletpre; // 弾のプレハブ
    public GameObject _roketBulletpre; // 弾のプレハブ
    public GameObject _homingBulletpre; // 弾のプレハブ
    public Transform _shootpoint; // 弾を発射する位置
    public Transform _shootpointR; // 弾を発射する位置
    public Transform _shootpointH; // 弾を発射する位置
    public GameObject _shootEffectPrefab;
    public float shootEffectLifetime = 2f; // 発射エフェクトの寿命（秒）
    public PlayerType shooterType;
    [SerializeField] TunkController tunkController;

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
            Bullet bullet = Instantiate(_bulletpre, _shootpoint.position, _shootpoint.rotation);
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
        }
        else
        {
            Debug.LogError("Bullet Prefab or Shoot Point is not assigned.");
        }
    }
    public void RocketShoot(PlayerType shooter)
    {
        //発射エフェクトを生成
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpoint.position, _shootpoint.rotation);
            Destroy(shootEffect, shootEffectLifetime); // エフェクトを一定時間後に消去
        }

        // 発射を遅延させる
        StartCoroutine(DelayedShoot(shooter));
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
    private IEnumerator DelayedShoot(PlayerType shooter)
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
                boosterScript.Initialize(new Vector3(transform.forward.x, transform.forward.y + _initialDirectionY, transform.forward.z)); // direction を渡す
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
