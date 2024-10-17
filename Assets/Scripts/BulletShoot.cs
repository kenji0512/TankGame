using UnityEngine;
using static Bullet;

public class BulletShoot : MonoBehaviour
{
    public GameObject _bulletpre; // 弾のプレハブ
    public Transform _shootpoint; // 弾を発射する位置
    [SerializeField] private BulletType _type = BulletType.Player; // 弾の種類（デフォルトはプレイヤー用）
    public GameObject _shootEffectPrefab;

    void Update()
    {
        // 左クリックなどの入力があったときに弾を発射
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        //発射エフェクトを生成
        if (_shootEffectPrefab != null)
        {
            Instantiate(_shootEffectPrefab, _shootpoint.position,_shootpoint.rotation);
        }
         
        // 弾を生成して初期位置と方向を設定
        GameObject bullet = Instantiate(_bulletpre, _shootpoint.position, _shootpoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetBulletType(_type); // 弾のタイプを設定（プレイヤー用か敵用か）
        }
    }
}

