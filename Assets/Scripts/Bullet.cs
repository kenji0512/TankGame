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
        if (other.TryGetComponent<Character>(out var targetCharacter))
        {
            // �v���C���[�p�̒e���G�ɓ��������ꍇ
            if (_type == BulletType.Player && targetCharacter is Enemy)
            {
                targetCharacter.TakeDamage();
                Destroy(gameObject); // �e��j��
            }
            // �G�p�̒e���v���C���[�ɓ��������ꍇ
            else if (_type == BulletType.Enemy && targetCharacter is Player)
            {
                targetCharacter.TakeDamage();
                Destroy(gameObject); // �e��j��
            }
        }
    }
}
