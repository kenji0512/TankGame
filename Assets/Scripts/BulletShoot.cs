using UnityEngine;
using static Bullet;

public class BulletShoot : MonoBehaviour
{
    public GameObject _bulletpre; // 弾のプレハブ
    public Transform _shootpoint; // 弾を発射する位置
    [SerializeField] private BulletType _bulletType = BulletType.Player; // 弾の種類（デフォルトはプレイヤー用）
    public GameObject _shootEffectPrefab;
    private PlayerType _playerType;

    public void Shoot()
    {
        //発射エフェクトを生成
        if (_shootEffectPrefab != null)
        {
            Instantiate(_shootEffectPrefab, _shootpoint.position,_shootpoint.rotation);
        }
         
        // 弾を生成して初期位置と方向を設定
        GameObject bullet = Instantiate(_bulletpre, _shootpoint.position, _shootpoint.rotation);
        if (bullet.TryGetComponent<Bullet>(out var bulletScript))
        {
            bulletScript.SetBulletType(_bulletType); // 弾の種類を設定
        }
    }
}
