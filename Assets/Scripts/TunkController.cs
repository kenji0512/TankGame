using Cysharp.Threading.Tasks;
using DG.Tweening;
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
    [SerializeField] private DamageFlash _damageFlash;

    [Header("Stats")]
    public float _moveSpeed;
    public float _defaultMoveSpeed = 5f;

    [SerializeField] private float _turnSpeed = 100f;
    [SerializeField] private float _turretTurnSpeed = 100f;
    [SerializeField] private float _inputSmoothSpeed = 10f;

    // === Public Properties ===
    public Transform TurretTransform => _turretTransform;
    public bool CanWarp { get; private set; } = true;
    public PlayerType playerType;
    public bool onSpeedUp = false;

    // === Private Fields ===
    private PlayerInput _playerInput;
    private Rigidbody _rb;
    private Animator _animator;

    private State _currentState = State.Idle;
    private bool _isInvulnerable = false;
    private int _skipMoveFrames = 0;
    private Vector2 _smoothedMoveInput = Vector2.zero;

    // === Unity Events ===
    protected override void Start()
    {
        base.Start();

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;

        _moveSpeed = _defaultMoveSpeed;

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
        //入力を０か１で認識するのではなく途中の数値を受け取ることによってカクつきを軽減
        if (playerType == PlayerType.Player1)
        {
            _smoothedMoveInput = Vector2.Lerp(
                _smoothedMoveInput,
                rawInput,
                Time.fixedDeltaTime * _inputSmoothSpeed
            );
            moveInput = _smoothedMoveInput;
        }


        if (playerType == PlayerType.Player1) _smoothedMoveInput = moveInput;

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
        if (Mathf.Abs(rotateInput) < 0.01f) return;

        float rotationAmount = rotateInput * _turretTurnSpeed * Time.deltaTime;
        Vector3 currentRotation = _turretTransform.localEulerAngles;
        currentRotation.y += rotationAmount;
        _turretTransform.localEulerAngles = currentRotation;
    }

    // === Shooting ===
    private void HandleShooting(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;
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
    public override async void TakeDamage(float damage)
    {
        if (IsInvulnerable) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        hpBar?.UpdateHP(currentHealth, maxHealth);
        transform.DOShakePosition(0.2f, 0.3f, 20, 90, false, true);

        if (_damageFlash != null)
        {
            await _damageFlash.FlashAsync(this.GetCancellationTokenOnDestroy());
        }

        if (currentHealth <= 0) Die();
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
        get => _isInvulnerable;
        set
        {
            _isInvulnerable = value;
            Debug.Log(_isInvulnerable ? "無敵モード ON!" : "無敵モード OFF!");
        }
    }

    public async void PreventWarpForSeconds(float seconds)
    {
        CanWarp = false;
        await UniTask.Delay(TimeSpan.FromSeconds(seconds));
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
