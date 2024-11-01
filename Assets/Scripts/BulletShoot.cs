using UnityEngine;
using static Bullet;

public class BulletShoot : MonoBehaviour
{
    public GameObject _bulletpre; // 弾のプレハブ
    public Transform _shootpoint; // 弾を発射する位置
    public GameObject _shootEffectPrefab;
    public float shootEffectLifetime = 2f; // 発射エフェクトの寿命（秒）

    public void Shoot()
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
        }
        else
        {
            Debug.LogError("Bullet Prefab or Shoot Point is not assigned.");
        }
    }
}
