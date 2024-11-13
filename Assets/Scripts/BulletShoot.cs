using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    public GameObject _bulletpre; // �e�̃v���n�u
    public GameObject _roketBulletpre; // �e�̃v���n�u
    public Transform _shootpoint; // �e�𔭎˂���ʒu
    public Transform _shootpointR; // �e�𔭎˂���ʒu
    [SerializeField] public Vector3 _initialDirection = new Vector3(1.0f, 1.0f, 0f); // �����ˏo����
    public GameObject _shootEffectPrefab;
    public float shootEffectLifetime = 2f; // ���˃G�t�F�N�g�̎����i�b�j
    [SerializeField] private float _delayTime = 0.5f;
    public PlayerType shooterType;

    public void Shoot(PlayerType shooterType, Vector3 direction)
    {
        //���˃G�t�F�N�g�𐶐�
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpoint.position, _shootpoint.rotation);
            Destroy(shootEffect, shootEffectLifetime); // �G�t�F�N�g����莞�Ԍ�ɏ���
        }

        // �e�𐶐����ď����ʒu�ƕ�����ݒ�
        if (_bulletpre != null && _shootpoint != null)
        { 
            GameObject bullet = Instantiate(_bulletpre, _shootpoint.position, _shootpoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.shooterType = shooterType;
                bulletScript.SetDirection(direction); // ���˕�����ݒ�
            }
        }
        else
        {
            Debug.LogError("Bullet Prefab or Shoot Point is not assigned.");
        }
    }
    public void RoketShoot(PlayerType shooter, Vector3 direction)
    {
        //���˃G�t�F�N�g�𐶐�
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpoint.position, _shootpoint.rotation);
            Destroy(shootEffect, shootEffectLifetime); // �G�t�F�N�g����莞�Ԍ�ɏ���
        }

        // ���˂�x��������
        StartCoroutine(DelayedShoot(shooter, direction));
    }
    private IEnumerator DelayedShoot(PlayerType shooter, Vector3 direction)
    {
        // �w�肳�ꂽ�x�����Ԃ����ҋ@
        yield return new WaitForSeconds(_delayTime);

        // �e�𐶐����ď����ʒu�ƕ�����ݒ�
        if (_roketBulletpre != null && _shootpointR != null)
        {
            GameObject roketBullet = Instantiate(_roketBulletpre, _shootpointR.position, _shootpointR.rotation);
            roketBullet.GetComponent<SphereBooster>().Initialize(_initialDirection, shooter); // ���˕�����n��
        }
        else
        {
            Debug.LogError("Rocket Bullet Prefab or Shoot Point is not assigned.");
        }
    }
}
