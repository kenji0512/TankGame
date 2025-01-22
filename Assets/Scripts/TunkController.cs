using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class TunkController : Character
{
    public static event Action<TunkController> OnPlayerDied;
    private PlayerInput _playerInput;
    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _shootActionName = "Shoot";
    //private InputAction _moveAction;
    //private InputAction _shootAction;
    //private InputAction _skillAction;
    //private InputAction _rotateAction;
    public float _moveSpeed = 5f;     // タンクの移動速度
    public float _turnSpeed = 100f;   // タンクの回転速度
    private Transform _firePoint;//Transformはpriveteにする
    [SerializeField] private BulletShoot _bulletShoot;
    public PlayerType _playerType;    // プレイヤーのタイプ
    private State _currentState = State.Idle; // プレイヤーの現在の状態
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
        //_playerInput.actions[_moveActionName].performed += HandleMovement;
        _playerInput.actions[_shootActionName].performed += HandleShooting;

        //_moveAction = _playerInput.actions["Move"];
        //_shootAction = _playerInput.actions["Shoot"];
        //_rotateAction = _playerInput.actions["Rotate"];

        // アクションを有効化
        //_moveAction.Enable();
        //_shootAction.Enable();
        //_rotateAction.Enable();

        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
        Debug.Log("Player2 action map is active.");

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
        //float moveInput = 0f;
        //float turnInput = 0f;

        //// プレイヤーごとに操作方法を設定
        //switch (_playerType)
        //{
        //    case PlayerType.Player1:
        //        moveInput = Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f);
        //        turnInput = Input.GetKey(KeyCode.A) ? -1f : (Input.GetKey(KeyCode.D) ? 1f : 0f);
        //        break;

        //    case PlayerType.Player2:
        //        moveInput = Input.GetKey(KeyCode.UpArrow) ? 1f : (Input.GetKey(KeyCode.DownArrow) ? -1f : 0f);
        //        turnInput = Input.GetKey(KeyCode.LeftArrow) ? -1f : (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);
        //        break;
        //}

        //MoveTank(moveInput, turnInput);
        //_currentState = moveInput != 0 || turnInput != 0 ? _currentState | State.Moving : _currentState & ~State.Moving;
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
        //bool isShooting = false;
        //bool isRocketShooting = false;

        // プレイヤーの入力に応じて弾を発射
        if (_playerInput.actions[_shootActionName].triggered)
        {
            // 通常弾の発射
            _bulletShoot.Shoot(_playerType, transform.forward);
            TriggerShootAnimation("shoot");
        }
        else if ((_playerType == PlayerType.Player1 && context.control.device == Keyboard.current) ||
        (_playerType == PlayerType.Player2 && context.control.device == Keyboard.current))
        {
            // ロケット弾の発射
            _bulletShoot.RocketShoot(_playerType, transform.forward);
            TriggerShootAnimation("rocketShoot");
        }

        //// プレイヤーの入力に応じて弾を発射
        //switch (_playerType)
        //{
        //    case PlayerType.Player1:
        //        if (Input.GetKeyDown(KeyCode.LeftShift))
        //        {
        //            _bulletShoot.Shoot(_playerType, shootDirection);
        //            isShooting = true;
        //        }
        //        else if (Input.GetKeyDown(KeyCode.F))
        //        {
        //            isRocketShooting = true;
        //            _bulletShoot.RocketShoot(_playerType, shootDirection);
        //        }
        //        break;

        //    case PlayerType.Player2:
        //        if (Input.GetKeyDown(KeyCode.RightShift))
        //        {
        //            _bulletShoot.Shoot(_playerType, shootDirection);
        //            isShooting = true;
        //        }
        //        else if (Input.GetKeyDown(KeyCode.Backslash))
        //        {
        //            isRocketShooting = true;
        //            _bulletShoot.RocketShoot(_playerType, shootDirection);
        //        }
        //        break;
        //}

        //if (isRocketShooting)
        //{
        //    _animator.SetTrigger("rocketShoot"); // ロケット用のトリガーを設定
        //    _currentState = _currentState | State.Shooting;
        //}
        //else if (isShooting)
        //{
        //    _animator.SetTrigger("shoot"); // 通常の弾用トリガー
        //    _currentState = _currentState | State.Shooting;
        //}
        //else
        //{
        //    _currentState = _currentState & ~State.Shooting;
        //}
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
