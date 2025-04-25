using UnityEngine;
using Cysharp.Threading.Tasks;

public class RocketShotStrategy : IShootStrategy
{
    public async void Shoot(BulletShoot bulletShoot, PlayerType shooterType, Vector3 direction, Quaternion rotation)
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;
        if (!bulletShoot.CanShoot) return;

        ObjectPool pool = bulletShoot.BulletPool;
        Transform shootPointR = bulletShoot.ShootPointR;

        await UniTask.Delay(System.TimeSpan.FromSeconds(0.5)); // 遅延

        GameObject bulletObject = pool.Release("RocketBullet", shootPointR.position, rotation);

        if (bulletObject != null)
        {
            SphereBooster booster = bulletObject.GetComponent<SphereBooster>();
            if (booster != null)
            {
                Vector3 launchDir = new Vector3(bulletShoot.transform.forward.x, bulletShoot.InitialDirectionY, bulletShoot.transform.forward.z);
                booster.Initialize(launchDir);
                booster.shooterType = shooterType;
            }

            bulletShoot.PlayShootEffectAsync(shootPointR.position, shootPointR.rotation);
            bulletShoot.StartCooldown().Forget();
        }
    }
}
