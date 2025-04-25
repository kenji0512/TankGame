using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    [SerializeField] private float _initialDirectionY = 1.0f; // �����ˏo����
    [SerializeField] private double _delayTime = 0.5f;

    public Transform _shootpoint; // �e�𔭎˂���ʒu
    public Transform _shootpointR; // �e�𔭎˂���ʒu
    public Transform _shootpointH; // �e�𔭎˂���ʒu
    public GameObject _shootEffectPrefab;
    public float shootEffectLifetime = 2f; // ���˃G�t�F�N�g�̎����i�b�j
    public double cooldownTime = 2.0f;

    public PlayerType shooterType;

    [SerializeField] TunkController tunkController;
    [SerializeField] private ObjectPool _bulletPool;

    [Header("�e�̃^�O�ݒ�")]
    [SerializeField] private string _normalBulletTag = "NormalBullet";
    [SerializeField] private string _rocketBulletTag = "RocketBullet";
    [SerializeField] private string _homingBulletTagA = "HomingBullet_Blue";
    [SerializeField] private string _homingBulletTagB = "HomingBullet_Red";

    private bool canShoot = true;

    public void Awake()
    {
        if (_shootpoint == null)
        {
            Debug.LogError("_shootpoint is not assigned.");
        }
    }
    public async void Shoot(PlayerType shooterType, Vector3 direction, Quaternion rotation)
    {
        if (!canShoot) return;

        GameObject bulletObject = _bulletPool.Release(_normalBulletTag, _shootpoint.position, rotation);//_bulletPool����ł܂Ƃ߂邩��ސ��ɉ���������邩����
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.transform.position = _shootpoint.position;
        bullet.transform.rotation = _shootpoint.rotation;

        // �e�𐶐����ď����ʒu�ƕ�����ݒ�
        if (bulletObject != null && _shootpoint != null)
        {
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
            StartCooldown();
        }
        else
        {
            Debug.LogError("NormalBullet Prefab or Shoot Point is not assigned.");
        }
        //���˃G�t�F�N�g�𐶐�
        PlayShootEffect(_shootpoint.position, _shootpoint.rotation);
    }
    public void RocketShoot(PlayerType shooter, Quaternion rotation)
    {
        if (!canShoot) return;
        // ���˂�x��������
        DelayedShootAsync(shooter, rotation).Forget();
        StartCooldown();

        //���˃G�t�F�N�g�𐶐�
        PlayShootEffect(_shootpointR.position, _shootpointR.rotation);
    }
    public void HomingMissle(PlayerType shooter, Vector3 direction, Quaternion rotation)
    {
        if(!canShoot) return;
        StartCooldown();
        //shooter �ɉ����ăz�[�~���O�e�̃^�O������
        string selectedHomingTag = shooter switch
        {
            PlayerType.Player1 => _homingBulletTagA, // �e
            PlayerType.Player2 => _homingBulletTagB, // �Ԓe
            _ => _homingBulletTagA, // �f�t�H���g�͐�
        };
        GameObject bulletObject = _bulletPool.Release(selectedHomingTag, _shootpointH.position, rotation);

        if (bulletObject == null)
        {
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
            //���˃G�t�F�N�g�𐶐�
            PlayShootEffect(_shootpointH.position, _shootpointH.rotation);
        }
    }
    //private IEnumerator DelayedShoot(PlayerType shooter, Quaternion rotation)
    //{
    //    // �w�肳�ꂽ�x�����Ԃ����ҋ@
    //    yield return new WaitForSeconds(_delayTime);
    //    if (_shootpointR == null)
    //    {
    //        Debug.LogError("Rocket shoot point is not assigned.");
    //        yield break;
    //    }
    //    GameObject bulletObject = _bulletPool.Release(_rocketBulletTag, _shootpointR.position, rotation);

    //    if (bulletObject == null)
    //    {
    //        Debug.LogError("Rocket bullet could not be released from pool.");
    //        yield break;
    //    }
    //    bulletObject.transform.position = _shootpointR.position;
    //    bulletObject.transform.rotation = _shootpointR.rotation;

    //    SphereBooster boosterScript = bulletObject.GetComponent<SphereBooster>();
    //    if (boosterScript != null)
    //    {
    //        Vector3 launchDir = new Vector3(transform.forward.x, _initialDirectionY, transform.forward.z);
    //        boosterScript.Initialize(launchDir);
    //        boosterScript.shooterType = shooter;
    //    }
    //    else
    //    {
    //        Debug.LogError("SphereBooster script not found on rocket bullet.");
    //    }
    //}
    private async UniTaskVoid DelayedShootAsync(PlayerType shooter, Quaternion rotation)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(_delayTime));

        if (_shootpointR == null)
        {
            Debug.Log("Roket shoot is not assigned");
            return;
        }
        GameObject bulletObject = _bulletPool.Release(_rocketBulletTag, _shootpointR.position, rotation);
        if (bulletObject == null)
        {
            Debug.LogError("Rocket bullet could not be released from pool.");
            return;
        }
        bulletObject.transform.position = _shootpointR.position;
        bulletObject.transform.rotation = _shootpointR.rotation;

        SphereBooster boosterScript = bulletObject.GetComponent<SphereBooster>();
        if (boosterScript != null)
        {
            Vector3 launchDir = new Vector3(transform.forward.x, _initialDirectionY, transform.forward.z);
            boosterScript.Initialize(launchDir);
            boosterScript.shooterType = shooter;
        }
        else
        {
            Debug.LogError("SphereBooster script not found on rocket bullet.");
        }
    }
    private void PlayShootEffect(Vector3 position, Quaternion rotation)
    {
        if (_shootEffectPrefab != null)
        {
            GameObject shootEffect = Instantiate(_shootEffectPrefab, position, rotation);
            Destroy(shootEffect, shootEffectLifetime);
        }
    }
    private async UniTask StartCooldown()
    {
        canShoot = false;
        await UniTask.Delay(System.TimeSpan.FromSeconds(cooldownTime));
        canShoot = true;
    }
}
