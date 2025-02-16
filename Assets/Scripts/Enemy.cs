using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public Transform _player; // プレイヤーの位置を設定
    private NavMeshAgent _agent;
    public GameObject _bulletprehub; //弾のプレハブ
    public Transform _firePoint; //弾の発射口
    private float _attackRange = 5.0f;// 攻撃範囲
    private float _lastAttackTime = 2f;// 攻撃クールダウン（秒）
    private int _attackCooldown;//攻撃クールダウン（秒）

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
        Debug.Log("Playerに攻撃！");
    }
    private void ShootBullet()
    {
        if ( _bulletprehub != null && _firePoint != null)
        {
            //弾を生成
            GameObject bullet = Instantiate(_bulletprehub,_firePoint.position,_firePoint.rotation);

            //弾の方向をプレイヤーに向ける
            Vector3 directionToPlayer = (_player.position - _firePoint.position).normalized;
            bullet.transform.position = directionToPlayer;

            //弾の種類を設定
            Bullet bulletScript = bullet.GetComponent<Bullet>();
        }
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        // 敵がダメージを受けた時の追加処理をここに追加（例：怒り状態になる）
    }

    protected override void Die()
    {
        base.Die();
        // 敵が死んだ時の追加処理
        Debug.Log("An enemy has been defeated!");
    }
}