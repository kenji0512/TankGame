using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TunkController : Character
{
    // === Events ===
    public static event Action<TunkController> OnPlayerDied;

    // === Serialized Fields ===
    [Header("Input Settings")]
    [SerializeField] private string _turretRotateActionName = "TurretRotate";
    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _shootActionName = "Shoot";

    [Header("References")]
    [SerializeField] private BulletShoot _bulletShoot;
    [SerializeField] private Transform _turretTransform;

    [Header("Stats")]
    public float _defaultMoveSpeed = 5f;
    public float _moveSpeed;
    [SerializeField] private float _turnSpeed = 100f;
    [SerializeField] private float _turretTurnSpeed = 100f;

    // === Public Properties ===
    public Transform TurretTransform => _turretTransform;
    public bool CanWarp { get; private set; } = true;
    public PlayerType playerType;

    // === Private Fields ===
    private PlayerInput _playerInput;
    private Rigidbody _rb;
    private Animator _animator;

    private State _currentState = State.Idle;
    private bool isInvulnerable = false;
    private int _skipMoveFrames = 0;
    public bool onSpeedUp = false;
    private Vector2 _smoothedMoveInput = Vector2.zero;

    [SerializeField] private float _inputSmoothSpeed = 10f; // 補間スピード

    // === Unity Events ===
    protected override void Start()
    {
        base.Start();

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;

        SetupInputActions();
        GameManager.Instance.RegisterPlayer(this);
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleTurretRotation();
    }

    // === Input Setup ===
    private void SetupInputActions()
    {
        _playerInput.SwitchCurrentActionMap("Player");

        _playerInput.actions[_shootActionName].performed += HandleShooting;
        _playerInput.actions["RocketShoot"].performed += HandleShooting;
        _playerInput.actions["HomingShoot"].performed += HandleShooting;

        if (!_playerInput.actions.Contains(_playerInput.actions[_shootActionName]))
        {
            Debug.LogError($"Action {_shootActionName} is not found in PlayerInput.");
        }
    }

    // === Movement ===
    private void HandleMovement()
    {
        if (_skipMoveFrames > 0)
        {
            _skipMoveFrames--;
            _rb.linearVelocity = Vector3.zero;
            return;
        }
        Vector2 rawInput = _playerInput.actions[_moveActionName].ReadValue<Vector2>();
        Vector2 moveInput = rawInput;
        // Player1 のときだけスムージング
        if (playerType == PlayerType.Player1)
        {
            _smoothedMoveInput = Vector2.Lerp(
                _smoothedMoveInput,
                rawInput,
                Time.fixedDeltaTime * _inputSmoothSpeed
            );
            moveInput = _smoothedMoveInput;
        }
        Debug.Log(moveInput);

        float moveDirection = moveInput.y;
        float turnDirection = moveInput.x;

        Vector3 moveVector = transform.forward * moveDirection * _moveSpeed * Time.deltaTime;
        _rb.MovePosition(_rb.position + moveVector);

        Quaternion turnAmount = Quaternion.Euler(0f, turnDirection * _turnSpeed * Time.fixedDeltaTime, 0f);
        _rb.MoveRotation(_rb.rotation * turnAmount);

        _currentState = moveDirection != 0 ? _currentState | State.Moving : _currentState & ~State.Moving;
    }

    // === Turret Control ===
    private void HandleTurretRotation()
    {
        if (_turretTransform == null) return;

        float rotateInput = _playerInput.actions[_turretRotateActionName].ReadValue<float>();
        if (Mathf.Abs(rotateInput) > 0.01f)
        {
            float rotationAmount = rotateInput * _turretTurnSpeed * Time.deltaTime;
            Vector3 currentRotation = _turretTransform.localEulerAngles;
            currentRotation.y += rotationAmount;
            _turretTransform.localEulerAngles = currentRotation;
        }
    }

    // === Shooting ===
    private void HandleShooting(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.currentState != GameState.Playing)
        {
            Debug.Log("ゲーム中ではないため発射できません。");
            return;
        }

        if (_bulletShoot == null) return;

        IShootStrategy strategy = context.action.name switch
        {
            var name when name == _shootActionName => new NormalShotStrategy(),
            "RocketShoot" => new RocketShotStrategy(),
            "HomingShoot" => new HomingShotStrategy(),
            _ => null
        };

        if (strategy == null) return;

        string trigger = context.action.name switch
        {
            var name when name == _shootActionName => "shoot",
            "RocketShoot" => "rocketShoot",
            "HomingShoot" => "shoot",
            _ => null
        };

        _animator.SetTrigger(trigger);
        _currentState |= State.Shooting;

        _bulletShoot.SetStrategy(strategy);
        _bulletShoot.ShootByStrategy(playerType, _turretTransform.forward, _turretTransform.rotation);
    }

    // === Damage & Death ===
    public override void TakeDamage(float damage)
    {
        if (IsInvulnerable)
        {
            Debug.Log("無敵状態のためダメージ無効");
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        hpBar?.UpdateHP(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        GameManager.Instance.RemovePlayer(this);
        OnPlayerDied?.Invoke(this);

        string winner = gameObject.name == "TankBlue(Player)" ? "TankRed(Enemy)" : "TankBlue(Player)";
        GameManager.Instance.CheckWinner(winner);

        base.Die();
    }

    // === Utility ===
    public bool IsInvulnerable
    {
        get => isInvulnerable;
        set
        {
            isInvulnerable = value;
            Debug.Log(isInvulnerable ? "無敵モード ON!" : "無敵モード OFF!");
        }
    }
    public async void PreventWarpForSeconds(float seconds)
    {
        CanWarp = false;
        await Cysharp.Threading.Tasks.UniTask.Delay(System.TimeSpan.FromSeconds(seconds));
        CanWarp = true;
    }
    public void ResetMovement()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _currentState = State.Idle;

        _skipMoveFrames = 2;
    }
}

