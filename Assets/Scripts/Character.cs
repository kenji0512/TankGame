using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;
    private bool isDamage = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"{gameObject.name} Initial Health: {currentHealth}"); // �����̗͂̃��O�\��

    }

    // �_���[�W���󂯂��Ƃ��ɌĂяo����郁�\�b�h
    public virtual void TakeDamage(int damageAmount = 1)
    {
        Debug.Log($"{gameObject.name} before damage: {currentHealth}"); // �_���[�W�O��HP�\��

        if (isDamage)
        {
            isDamage = true;
            currentHealth -= damageAmount;
            Debug.Log($"{gameObject.name} took {damageAmount} damage. Remaining Health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

    }

    public void ResetDamageflag()
    {
        isDamage = false;
    }
    // ���S���ɌĂяo����郁�\�b�h
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // �I�u�W�F�N�g��j��
    }

    // ���݂̗̑͂��擾���郁�\�b�h
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
