using NetcodePlus.Demo;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    protected float currentHealth;
    public bool onPowerUp = false;  // �A�C�e���擾���� �p���[�A�b�vtrue �ɂȂ�

    public float _damageAmount { get; set; } = 10;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        //_originalDamage = _damageAmount;
        Debug.Log($"{gameObject.name} Initial Health: {currentHealth}"); // �����̗͂̃��O�\��
    }

    // �_���[�W���󂯂��Ƃ��ɌĂяo����郁�\�b�h
    public virtual void TakeDamage(float intdamage)
    {

    }
    // ���S���ɌĂяo����郁�\�b�h
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // �I�u�W�F�N�g��j��
    }
}
