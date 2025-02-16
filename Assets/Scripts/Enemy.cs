using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public Transform _player; // �v���C���[�̈ʒu��ݒ�
    private NavMeshAgent _agent;
    public GameObject _bulletprehub; //�e�̃v���n�u
    public Transform _firePoint; //�e�̔��ˌ�
    private float _attackRange = 5.0f;// �U���͈�
    private float _lastAttackTime = 2f;// �U���N�[���_�E���i�b�j
    private int _attackCooldown;//�U���N�[���_�E���i�b�j

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (_player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            if (distanceToPlayer <= _attackRange && Time.time > _lastAttackTime + _attackCooldown)
            {
                AttackPlayer();
                _lastAttackTime = Time.time;
            }
            else
            {
                _agent.SetDestination(_player.position);
            }
        }
    }
    void AttackPlayer()
    {
        Debug.Log("Player�ɍU���I");
    }
    private void ShootBullet()
    {
        if ( _bulletprehub != null && _firePoint != null)
        {
            //�e�𐶐�
            GameObject bullet = Instantiate(_bulletprehub,_firePoint.position,_firePoint.rotation);

            //�e�̕������v���C���[�Ɍ�����
            Vector3 directionToPlayer = (_player.position - _firePoint.position).normalized;
            bullet.transform.position = directionToPlayer;

            //�e�̎�ނ�ݒ�
            Bullet bulletScript = bullet.GetComponent<Bullet>();
        }
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        // �G���_���[�W���󂯂����̒ǉ������������ɒǉ��i��F�{���ԂɂȂ�j
    }

    protected override void Die()
    {
        base.Die();
        // �G�����񂾎��̒ǉ�����
        Debug.Log("An enemy has been defeated!");
    }
}