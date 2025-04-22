using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    [SerializeField] private float _initialDirectionY = 1.0f; // �����ˏo����
    [SerializeField] private float _delayTime = 0.5f;

    public Transform _shootpoint; // �e�𔭎˂���ʒu
    public Transform _shootpointR; // �e�𔭎˂���ʒu
    public Transform _shootpointH; // �e�𔭎˂���ʒu
    public GameObject _shootEffectPrefab;
    public float shootEffectLifetime = 2f; // ���˃G�t�F�N�g�̎����i�b�j

    public PlayerType shooterType;

    [SerializeField] TunkController tunkController;
    [SerializeField] private ObjectPool _bulletPool;

    [Header("�e�̃^�O�ݒ�")]
    [SerializeField] private string _normalBulletTag = "NormalBullet";
    [SerializeField] private string _rocketBulletTag = "RocketBullet";
    [SerializeField] private string _homingBulletTagA = "HomingBullet_Blue";
    [SerializeField] private string _homingBulletTagB = "HomingBullet_Red";

    public void Awake()
    {
        if (_shootpoint == null)
        {
            Debug.LogError("_shootpoint is not assigned.");
        }
    }
    public void Shoot(PlayerType shooterType, Vector3 direction, Quaternion rotation)
    {
        GameObject bulletObject = _bulletPool.Release(_normalBulletTag, _shootpoint.position, rotation);//_bulletPool����ł܂Ƃ߂邩��ސ��ɉ���������邩����
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.transform.position = _shootpoint.position;
        bullet.transform.rotation = _shootpoint.rotation;

        // �e�𐶐����ď����ʒu�ƕ�����ݒ�
        if (bulletObject != null && _shootpoint != null)
        {
            //Instantiate(bulletObject, _shootpoint.position, _shootpoint.rotation);
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
            Debug.LogError("NormalBullet Prefab or Shoot Point is not assigned.");
        }
        //���˃G�t�F�N�g�𐶐�
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpoint.position, _shootpoint.rotation);
            Destroy(shootEffect, shootEffectLifetime); // �G�t�F�N�g����莞�Ԍ�ɏ���
        }
    }
    public void RocketShoot(PlayerType shooter, Quaternion rotation)
    {
        //���˃G�t�F�N�g�𐶐�
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpointR.position, _shootpointR.rotation);
            Destroy(shootEffect, shootEffectLifetime); // �G�t�F�N�g����莞�Ԍ�ɏ���
        }

        // ���˂�x��������
        StartCoroutine(DelayedShoot(shooter,rotation));
    }
    public void HomingMissle(PlayerType shooter, Vector3 direction, Quaternion rotation)
    {
        //shooter �ɉ����ăz�[�~���O�e�̃^�O������
        string selectedHomingTag = shooter switch
        {
            PlayerType.Player1 => _homingBulletTagA, // �e
            PlayerType.Player2 => _homingBulletTagB, // �Ԓe
            _ => _homingBulletTagA, // �f�t�H���g�͐�
        };
        //GameObject bulletObject = _bulletPool.Release(_homingBulletTagA, transform.position);
        //GameObject bulletObjectB = _bulletPool.Release(_homingBulletTagB, transform.position);
        GameObject bulletObject = _bulletPool.Release(selectedHomingTag, _shootpointH.position,rotation);

        if (bulletObject == null)
        {
            //bulletObject.transform.rotation = Quaternion.LookRotation(direction);
            Debug.LogError("ObjectPool ���� HomingBullet ���擾�ł��܂���ł����I");
            return; // null �Ȃ瑦 return ���ăA�N�Z�X��h��
        }
        HomingMissile missile = bulletObject.GetComponent<HomingMissile>();
        if (missile != null)
        {
            missile.Initialize(shooter); // �ǔ��e�ɔ��ˎ�̏���n��
        }
        else
        {
            Debug.LogError("HomingBullet �� HomingMissile �R���|�[�l���g���t���Ă��܂���I");
        }
        if (_shootpointH != null && _shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpointH.position, _shootpointH.rotation);
            Destroy(shootEffect, shootEffectLifetime);
        }
        //else
        //{
        //    Debug.LogError("ObjectPool ���� HomingBullet ���擾�ł��܂���ł����I");
        //}

        //bulletObject.transform.rotation = _shootpointH.rotation;
        //if (_shootpointH == null)
        //{
        //    Debug.LogError("Shoot Point is not assigned.");
        //}
        //if (_shootEffectPrefab != null)
        //{
        //    GameObject shootEffect = Instantiate(_shootEffectPrefab, _shootpointH.position, _shootpointH.rotation);
        //    Destroy(shootEffect, shootEffectLifetime); // �G�t�F�N�g����莞�Ԍ�ɏ���
        //}
        //HomingMissile homingscript = bulletObject.GetComponent<HomingMissile>();

        //if (bulletObject != null && _shootpointH != null)
        //{
        //    //GameObject homingBullet = Instantiate(bulletObject, _shootpointH.position, _shootpointH.rotation);
        //    if (homingscript != null)
        //    {
        //        homingscript.Initialize(shooter);
        //    }
        //    else
        //    {
        //        Debug.LogError("HomingMissile script not found on homing bullet prefab.");
        //    }
        //}
        //else
        //{
        //    Debug.LogError("Homing Bullet Prefab or Shoot Point is not assigned.");
        //}
    }
    private IEnumerator DelayedShoot(PlayerType shooter,Quaternion rotation)
    {
        // �w�肳�ꂽ�x�����Ԃ����ҋ@
        yield return new WaitForSeconds(_delayTime);
        if (_shootpointR == null)
        {
            Debug.LogError("Rocket shoot point is not assigned.");
            yield break;
        }
        GameObject bulletObject = _bulletPool.Release(_rocketBulletTag, _shootpointR.position, rotation);

        if (bulletObject == null)
        {
            Debug.LogError("Rocket bullet could not be released from pool.");
            yield break;
        }
        bulletObject.transform.position = _shootpointR.position;
        bulletObject.transform.rotation = _shootpointR.rotation;
        // �e�𐶐����ď����ʒu�ƕ�����ݒ�
        //if (bulletObject != null && _shootpointR != null)
        //{
        //    GameObject roketBullet = Instantiate(bulletObject, _shootpointR.position, _shootpointR.rotation);
        //    SphereBooster boosterScript = roketBullet.GetComponent<SphereBooster>();

        //    //roketBullet.GetComponent<SphereBooster>().Initialize(direction, shooter); // ���˕�����n��
        //    if (boosterScript != null)
        //    {
        //        Vector3 launchDir = new Vector3(transform.forward.x, transform.forward.y + _initialDirectionY, transform.forward.z);
        //        //boosterScript.Initialize(new Vector3(transform.forward.x, transform.forward.y + _initialDirectionY, transform.forward.z)); // direction ��n��
        //        boosterScript.shooterType = shooter;
        //        roketBullet.GetComponent<SphereBooster>().shooterType = shooter; // shooterType ��n��
        //    }
        //}
        //else
        //{
        //    Debug.LogError("Rocket Bullet Prefab or Shoot Point is not assigned.");
        //}
        SphereBooster boosterScript = bulletObject.GetComponent<SphereBooster>();
        if (boosterScript != null)
        {
            Vector3 launchDir = new Vector3(transform.forward.x, _initialDirectionY, transform.forward.z);
            boosterScript.Initialize(launchDir);
            boosterScript.shooterType = shooter;
            //roketBullet.GetComponent<SphereBooster>().shooterType = shooter; // shooterType ��n��
        }
        else
        {
            Debug.LogError("SphereBooster script not found on rocket bullet.");
        }
    }
}
