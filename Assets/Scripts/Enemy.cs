using UnityEngine;

public class Enemy : Character
{
    protected override void Start()
    {
        base.Start();
        // �G�ŗL�̏����������������ɒǉ�
    }

    public override void TakeDamage(int damageAmount = 10)
    {
        base.TakeDamage(damageAmount);
        // �G���_���[�W���󂯂����̒ǉ������������ɒǉ��i��F�{���ԂɂȂ�j
    }

    protected override void Die()
    {
        base.Die();
        // �G�����񂾎��̒ǉ������i��F�|�C���g��ǉ��j
        Debug.Log("An enemy has been defeated!");
    }
}