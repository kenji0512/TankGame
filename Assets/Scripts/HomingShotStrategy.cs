using Cysharp.Threading.Tasks;
using UnityEngine;

public class HomingShotStrategy : IShootStrategy
{
    public void Shoot(BulletShoot bulletShoot, PlayerType shooterType, Vector3 direction, Quaternion rotation)
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;
        if (!bulletShoot.CanShoot) return;

        Transform shootPoint = bulletShoot.ShootPointH;
        string tag = shooterType == PlayerType.Player1 ? "HomingBullet_Blue" : "HomingBullet_Red";
        GameObject bulletObject = bulletShoot.BulletPool.Release(tag, shootPoint.position, rotation);
        if (bulletObject == null) return; // プール切れ

        HomingMissile missile = bulletObject.GetComponent<HomingMissile>();

        if (missile != null)
        {
            missile.Initialize(shooterType);
        }
        bulletShoot.PlayShootEffectAsync(shootPoint.position, shootPoint.rotation);
        bulletShoot.StartCooldown().Forget();
    }
}
