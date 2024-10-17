using UnityEngine;
using static Bullet;

public class BulletShoot : MonoBehaviour
{
    public GameObject _bulletpre; // �e�̃v���n�u
    public Transform _shootpoint; // �e�𔭎˂���ʒu
    [SerializeField] private BulletType _type = BulletType.Player; // �e�̎�ށi�f�t�H���g�̓v���C���[�p�j
    public GameObject _shootEffectPrefab;

    void Update()
    {
        // ���N���b�N�Ȃǂ̓��͂��������Ƃ��ɒe�𔭎�
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        //���˃G�t�F�N�g�𐶐�
        if (_shootEffectPrefab != null)
        {
            Instantiate(_shootEffectPrefab, _shootpoint.position,_shootpoint.rotation);
        }
         
        // �e�𐶐����ď����ʒu�ƕ�����ݒ�
        GameObject bullet = Instantiate(_bulletpre, _shootpoint.position, _shootpoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetBulletType(_type); // �e�̃^�C�v��ݒ�i�v���C���[�p���G�p���j
        }
    }
}

