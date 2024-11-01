using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
        if (other.CompareTag("Player"))
        {
            var targetCharacter = other.GetComponent<Character>();
            if (targetCharacter != null)
            {
                HandleCharacterCollision(targetCharacter);
            }
            Debug.Log("Bullet hit Player and dealt damage!");
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
        // �v���C���[�ɓ��������ꍇ�A�_���[�W��^����
        targetCharacter.TakeDamage();
        CreateHitEffect();
        Destroy(gameObject); // �e��j��
        Debug.Log("Destroy" + gameObject.name);
    }
    private void CreateHitEffect()
    {
        if (_hitEffectPrefab != null)
        {
            // �Փ˃G�t�F�N�g�𐶐�
            GameObject hitEffect = Instantiate(_hitEffectPrefab, transform.position, Quaternion.identity);

            // �G�t�F�N�g��3�b��Ɏ����ŏ���
            Destroy(hitEffect, 2f);
        }
    }

}
