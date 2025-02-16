using NetcodePlus.Demo;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    protected float currentHealth;
    public float _damageAmount { get; set; } = 10;
    Item item;    // Item �N���X�̃C���X�^���X���Q��

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        Debug.Log($"{gameObject.name} Initial Health: {currentHealth}"); // �����̗͂̃��O�\��
    }

    // �_���[�W���󂯂��Ƃ��ɌĂяo����郁�\�b�h
    public virtual void TakeDamage()
    {
        Debug.Log($"{gameObject.name} before damage: {currentHealth}"); // �_���[�W�O��HP�\��
        Debug.Log($"{gameObject.name} applying damage: {_damageAmount}"); // �_���[�W�ʂ̃��O
        // �p���[�A�b�v���L���ȏꍇ�A�_���[�W�ɒǉ��� 10 ��������
        if (item != null && item.OnPowerUp)
        {
            currentHealth -= (_damageAmount + item._value);
            Debug.Log($"{gameObject.name} OnPowerUp is active! Extra damage applied.");
        }
        else
        {
            currentHealth -= _damageAmount;
        }
        Debug.Log($"{gameObject.name} took {_damageAmount} damage. Remaining Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    // ���S���ɌĂяo����郁�\�b�h
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // �I�u�W�F�N�g��j��
    }
}
