using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TunkController : Character
{
    public static event Action<TunkController> OnPlayerDied;
    private PlayerInput _playerInput;
    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _shootActionName = "Shoot";
    public float _moveSpeed = 5f;     // タンクの移動速度
    public float _turnSpeed = 100f;   // タンクの回転速度
    private Transform _firePoint;//Transformはpriveteにする
    [SerializeField] private BulletShoot _bulletShoot;
    public PlayerType _playerType;    // プレイヤーのタイプ
    private State _currentState = State.Idle; // プレイヤーの現在の状態
    private HomingMissile _missile;
    private Animator _animator;
    private Rigidbody _rb;

    protected override void Awake()
    {
        base.Awake(); // StartではなくAwakeを呼ぶ
        _playerInput = GetComponent<PlayerInput>();
        // UnityEvent経由で受け取るように設定
        _playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;

        // Player 1用またはPlayer 2用のアクションマップを設定
        if (_playerType == PlayerType.Player1)
        {
            _playerInput.SwitchCurrentActionMap("Player1");
        }
        else if (_playerType == PlayerType.Player2)
        {
            _playerInput.SwitchCurrentActionMap("Player1");
        }
        // PlayerInputのイベント登録
        _playerInput.actions[_shootActionName].performed += HandleShooting;
        _playerInput.actions["RocketShoot"].performed += HandleShooting;
        _playerInput.actions["HomingShoot"].performed += HandleShooting;

        _missile = GetComponent<HomingMissile>();
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleMovement();
    }
    public void HandleMovement()
    {
        // ゲームパッドの移動入力を取得
        Vector2 moveInput = _playerInput.actions[_moveActionName].ReadValue<Vector2>();
        float moveDirection = moveInput.y;  // 前後移動
        float turnDirection = moveInput.x;  // 左右回転

        MoveTank(moveDirection, turnDirection);

        // 状態を更新
        _currentState = moveDirection != 0 || turnDirection != 0
            ? _currentState | State.Moving
            : _currentState & ~State.Moving;
    }

    private void MoveTank(float moveInput, float turnInput)
    {
        Vector3 moveDirection = transform.forward * moveInput * _moveSpeed * Time.deltaTime;
        Vector3 newPosition = _rb.position + moveDirection;
        _rb.MovePosition(newPosition);

        float turnAmount = turnInput * _turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        _rb.MoveRotation(_rb.rotation * turnRotation);
    }//移動処理

    void HandleShooting(InputAction.CallbackContext context)
    {
        Vector3 shootDirection = transform.forward; // 発射方向

        // プレイヤーの入力に応じて弾を発射
        if (_playerInput.actions[_shootActionName].triggered)
        {
            // 通常弾の発射
            _bulletShoot.Shoot(_playerType, transform.forward);
            TriggerShootAnimation("shoot");
        }

        // ロケット弾の発射（RocketShootアクションに応じて処理）
        if (context.action.name == "RocketShoot") // ロケット弾の場合のチェック
        {
            Debug.Log($"Rocket Shoot by {_playerType}"); // ロケット弾の発射が呼ばれたことを確認
            _bulletShoot.RocketShoot(_playerType, shootDirection);
            TriggerShootAnimation("rocketShoot");
        }
        //ホーミング弾が発射
        if (context.action.name == "HomingShoot")
        {

            Transform target = _missile.FindClosestTarget(); // 最も近いターゲットを取得
            if (target == null)
            {
                Debug.LogError("Target is null in Initialize method.");
            }
            if (target != null)
            {
                Debug.Log($"Homing Shoot by {_playerType} at {target.name}");
                _bulletShoot.HomingMissle(_playerType, target);
                TriggerShootAnimation("shoot");
            }
            else
            {
                Debug.LogWarning("No target found for homing missile.");
            }
        }
    }

    private void TriggerShootAnimation(string triggerName)
    {
        _animator.SetTrigger(triggerName);
        _currentState |= State.Shooting;
    }
    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
        _currentState = State.Dead;
    }
    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        GameManager.Instance.RemovePlayer(this);
        OnPlayerDied?.Invoke(this);
        base.Die();
    }
}
