using Cysharp.Threading.Tasks;
using UnityEngine;

public class NormalShotStrategy : IShootStrategy
{
    public void Shoot(BulletShoot bulletShoot, PlayerType shooterType, Vector3 direction, Quaternion rotation)
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;
        if (!bulletShoot.CanShoot) return;

        //GameObject bulletObject = _bulletPool.Release(_normalBulletTag, _shootpoint.position, rotation);//_bulletPoolを一個でまとめるか種類数に応じた数作るか検討
        //Bullet bullet = bulletObject.GetComponent<Bullet>();

        var pool = bulletShoot.BulletPool;
        var shootPoint = bulletShoot.ShootPoint;

        GameObject bulletObject = pool.Release("NormalBullet", shootPoint.position, rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = shootPoint.rotation;
            bullet.shooterType = shooterType;
            bullet.SetDirection(direction);

            if (bulletShoot.TunkController.onPowerUp)
            {
                bullet.BulletdamageAmount += 10;
            }

            bulletShoot.PlayShootEffectAsync(shootPoint.position, shootPoint.rotation);
            bulletShoot.StartCooldown().Forget();
        }
    }
}
