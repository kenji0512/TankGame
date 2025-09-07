using UnityEngine;
using Cysharp.Threading.Tasks;

public class RocketShotStrategy : IShootStrategy
{
    public async void Shoot(BulletShoot bulletShoot, PlayerType shooterType, Vector3 direction, Quaternion rotation)
    {
        TunkController playerObj = GameManager.Instance.GetPlayer(shooterType);
        if (playerObj == null) return;

        if (GameManager.Instance.currentState != GameState.Playing) return;
        if (!bulletShoot.CanShoot) return;

        ObjectPool pool = bulletShoot.BulletPool;
        Transform shootPointR = bulletShoot.ShootPointR;

        await UniTask.Delay(System.TimeSpan.FromSeconds(0.5)); // 遅延

        GameObject bulletObject = pool.Release("RocketBullet", shootPointR.position, rotation);

        if (bulletObject != null)
        {
            SphereBooster booster = bulletObject.GetComponent<SphereBooster>();
            TunkController tunkController = playerObj.GetComponent<TunkController>();

            if (booster != null)
            {
                //Transform turretTransform = _tunkController.TurretTransform;

                Vector3 launchDir = new Vector3(tunkController.TurretTransform.forward.x,
                    bulletShoot.InitialDirectionY,
                    tunkController.TurretTransform.forward.z);
                booster.Initialize(launchDir);
                booster.shooterType = shooterType;
            }

            bulletShoot.PlayShootEffectAsync(shootPointR.position, shootPointR.rotation);
            bulletShoot.StartCooldown().Forget();
        }
    }
}
