using System.Collections;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    [SerializeField] private float _initialDirectionY = 1.0f; // �����ˏo����
    [SerializeField] private float _delayTime = 0.5f;

    public Bullet _bulletpre; // �e�̃v���n�u
    public GameObject _roketBulletpre; // �e�̃v���n�u
    public GameObject _homingBulletpre; // �e�̃v���n�u
    public Transform _shootpoint; // �e�𔭎˂���ʒu
    public Transform _shootpointR; // �e�𔭎˂���ʒu
    public Transform _shootpointH; // �e�𔭎˂���ʒu
    public GameObject _shootEffectPrefab;
    public float shootEffectLifetime = 2f; // ���˃G�t�F�N�g�̎����i�b�j
    public PlayerType shooterType;
    [SerializeField] TunkController tunkController;

    public GameObject HomingBulletPrefab => _homingBulletpre; // �v���p�e�B�Ƃ��Č��J

    public void Awake()
    {
        if (_shootpoint == null)
        {
            Debug.LogError("_shootpoint is not assigned.");
        }
    }
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
            Bullet bullet = Instantiate(_bulletpre, _shootpoint.position, _shootpoint.rotation);
            if (tunkController.onPowerUp)
            {
                bullet.BulletdamageAmount += 10;
                Debug.Log($"-10damage");
            }
            if (bullet != null)
            {
                bullet.shooterType = shooterType;
                bullet.SetDirection(direction); // ���˕�����ݒ�
            }
        }
        else
        {
            Debug.LogError("Bullet Prefab or Shoot Point is not assigned.");
        }
    }
    public void RocketShoot(PlayerType shooter)
    {
        //���˃G�t�F�N�g�𐶐�
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpoint.position, _shootpoint.rotation);
            Destroy(shootEffect, shootEffectLifetime); // �G�t�F�N�g����莞�Ԍ�ɏ���
        }

        // ���˂�x��������
        StartCoroutine(DelayedShoot(shooter));
    }
    public void HomingMissle(PlayerType shooter)
    {
        if (_homingBulletpre == null)
        {
            Debug.LogError("BulletShoot is not assigned.");
        }

        if (_shootpointH == null)
        {
            Debug.LogError("Shoot Point is not assigned.");
        }

        // ���̎Q�ƃI�u�W�F�N�g�����l�Ɋm�F
        if (shooter == null)
        {
            Debug.LogError("Homing target is null.");
        }
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpoint.position, _shootpoint.rotation);
            Destroy(shootEffect, shootEffectLifetime); // �G�t�F�N�g����莞�Ԍ�ɏ���
        }
        if (_homingBulletpre != null && _shootpointH != null)
        {
            GameObject homingBullet = Instantiate(_homingBulletpre, _shootpointH.position, _shootpointH.rotation);
            HomingMissile homingscript = homingBullet.GetComponent<HomingMissile>();
            if (homingscript != null)
            {
                homingscript.Initialize(shooter);
            }
            else
            {
                Debug.LogError("HomingMissile script not found on homing bullet prefab.");
            }
        }
        else
        {
            Debug.LogError("Homing Bullet Prefab or Shoot Point is not assigned.");
        }
    }
    private IEnumerator DelayedShoot(PlayerType shooter)
    {
        // �w�肳�ꂽ�x�����Ԃ����ҋ@
        yield return new WaitForSeconds(_delayTime);

        // �e�𐶐����ď����ʒu�ƕ�����ݒ�
        if (_roketBulletpre != null && _shootpointR != null)
        {
            GameObject roketBullet = Instantiate(_roketBulletpre, _shootpointR.position, _shootpointR.rotation);
            SphereBooster boosterScript = roketBullet.GetComponent<SphereBooster>();

            //roketBullet.GetComponent<SphereBooster>().Initialize(direction, shooter); // ���˕�����n��
            if (boosterScript != null)
            {
                boosterScript.Initialize(new Vector3(transform.forward.x, transform.forward.y + _initialDirectionY, transform.forward.z)); // direction ��n��
                boosterScript.shooterType = shooter;
                roketBullet.GetComponent<SphereBooster>().shooterType = shooter; // shooterType ��n��
            }
        }
        else
        {
            Debug.LogError("Rocket Bullet Prefab or Shoot Point is not assigned.");
        }
    }
}
