using UnityEngine;

public interface IShootStrategy
{
    void Shoot(BulletShoot bulletShoot, PlayerType playerType,Vector3 direction,Quaternion rotation);
}