using UnityEngine;

public interface IShootStrategy
{
    void Shoot(Transform firePoint, PlayerType playerType);
}