using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Invalid,
        Player,
        Enemy,
    }

    [SerializeField] private BulletType _type;
    [SerializeField] private float speed = 20f;   // �e�̈ړ����x
    [SerializeField] private float lifetime = 5f; // �e�̎����i�b�j
    [SerializeField] public GameObject _hitEffectPrefab;//�Փ˃G�t�F�N�g�̃v���n�u
    [SerializeField] public int damageAmount = 10;//�_���[�W��

    private void Start()
    {
        // ��莞�Ԍ�ɒe�������Ŕj��
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // �e��O���Ɉړ�������
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BreakableWall>(out var breakableWall))
        {
            HandleWallCollision(breakableWall);
        }
        else if (other.TryGetComponent<Character>(out var targetCharacter))
        {
            HandleCharacterCollision(targetCharacter);
        }
    }
    private void HandleWallCollision(BreakableWall breakableWall)
    {
        breakableWall.Damage(); // �ǂɃ_���[�W��^����
        CreateHitEffect(); // �Փ˃G�t�F�N�g�𐶐�
        Destroy(gameObject); // �e��j��
    }
    private void HandleCharacterCollision(Character targetCharacter)
    {
        if (_type == BulletType.Player && targetCharacter is Enemy)
        {
            targetCharacter.TakeDamage();
            CreateHitEffect();
            Destroy(gameObject); // �e��j��
        }
        else if (_type == BulletType.Enemy && targetCharacter is Player)
        {
            targetCharacter.TakeDamage();
            CreateHitEffect();
            Destroy(gameObject); // �e��j��
        }
    }
    private void CreateHitEffect()
    {
        if (_hitEffectPrefab != null)
        {
            //�Փ˃G�t�F�N�g�𐶐�
            Instantiate(_hitEffectPrefab, transform.position,Quaternion.identity);
        }
    }
    // �V�������\�b�h��ǉ����Ēe�̎�ނ�ݒ�ł���悤�ɂ���
    public void SetBulletType(BulletType type)
    {
        _type = type;
    }
    public void SetDamageAmount(int damage)
    {
        damageAmount = damage;
    }
}
