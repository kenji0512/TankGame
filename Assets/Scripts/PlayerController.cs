using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    public static event Action<PlayerController> OnPlayerDied;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _shootAction;
    private InputAction _skillAction;
    private InputAction _rotateAction;
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
        Debug.Log($"Player Type: {_playerType}");

        // Player 1用またはPlayer 2用のアクションマップを設定
        if (_playerType == PlayerType.Player1)
        {
            Debug.Log("Switching to Player1 action map");

            _playerInput.SwitchCurrentActionMap("Player1");
        }
        else if (_playerType == PlayerType.Player2)
        {

            Debug.Log("Switching to Player2 action map");

            _playerInput.SwitchCurrentActionMap("Player2");
        }
        Debug.Log("Switching to " + (_playerType == PlayerType.Player1 ? "Player1" : "Player2") + " action map");

        _moveAction = _playerInput.actions["Move"];
        _shootAction = _playerInput.actions["Shoot"];
        _rotateAction = _playerInput.actions["Rotate"];

        // アクションを有効化
        _moveAction.Enable();
        _shootAction.Enable();
        _rotateAction.Enable();

        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
        Debug.Log("Player2 action map is active.");
        Debug.Log("Move action value: " + _moveAction.ReadValue<Vector2>());

    }

    private void Update()
    {
        // 最適化: Updateマネージャーを使用する
        if (_currentState != State.Dead)
        {
            HandleMovement();
            HandleShooting();
        }
    }

    void HandleMovement()
    {
        // ゲームパッドの移動入力を取得
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        float moveDirection = moveInput.y;  // 前後移動
        float turnDirection = moveInput.x;  // 左右回転

        MoveTank(moveDirection, turnDirection);

        // 状態を更新
        _currentState = moveDirection != 0 || turnDirection != 0 ? _currentState | State.Moving : _currentState & ~State.Moving;

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

    void HandleShooting()
    {
        Vector3 shootDirection = transform.forward; // 発射方向
        bool isShooting = false;
        bool isRocketShooting = false;

        // プレイヤーの入力に応じて弾を発射
        if (_shootAction.triggered) // ボタンが押された場合
        {
            _bulletShoot.Shoot(_playerType, shootDirection);
            isShooting = true;
        }
        else if (_playerType == PlayerType.Player1 && Keyboard.current.fKey.isPressed)
        {
            isRocketShooting = true;
            _bulletShoot.RocketShoot(_playerType, shootDirection);
        }
        else if (_playerType == PlayerType.Player2 && Keyboard.current.backslashKey.isPressed)
        {
            isRocketShooting = true;
            _bulletShoot.RocketShoot(_playerType, shootDirection);
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

        if (isRocketShooting)
        {
            _animator.SetTrigger("rocketShoot"); // ロケット用のトリガーを設定
            _currentState = _currentState | State.Shooting;
        }
        else if (isShooting)
        {
            _animator.SetTrigger("shoot"); // 通常の弾用トリガー
            _currentState = _currentState | State.Shooting;
        }
        else
        {
            _currentState = _currentState & ~State.Shooting;
        }
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
