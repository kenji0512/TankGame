using UnityEngine;
using static Bullet;

public class BulletShoot : MonoBehaviour
{
    public GameObject _bulletpre; // �e�̃v���n�u
    public Transform _shootpoint; // �e�𔭎˂���ʒu
    [SerializeField] private BulletType _bulletType = BulletType.Player; // �e�̎�ށi�f�t�H���g�̓v���C���[�p�j
    public GameObject _shootEffectPrefab;
    private PlayerType _playerType;

    public void Shoot()
    {
        //���˃G�t�F�N�g�𐶐�
        if (_shootEffectPrefab != null)
        {
            Instantiate(_shootEffectPrefab, _shootpoint.position,_shootpoint.rotation);
        }
         
        // �e�𐶐����ď����ʒu�ƕ�����ݒ�
        GameObject bullet = Instantiate(_bulletpre, _shootpoint.position, _shootpoint.rotation);
        if (bullet.TryGetComponent<Bullet>(out var bulletScript))
        {
            bulletScript.SetBulletType(_bulletType); // �e�̎�ނ�ݒ�
        }
    }
}
