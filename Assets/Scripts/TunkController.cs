using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TunkController : Character
{
    public static event Action<TunkController> OnPlayerDied;
    [SerializeField] private BulletShoot _bulletShoot;  // BulletShoot�R���|�[�l���g
    [SerializeField] private Transform _turret;

    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _shootActionName = "Shoot";
    public float _moveSpeed = 5f; // �^���N�̈ړ����x
    public float _turnSpeed = 100f;   // �^���N�̉�]���x
    public float _turretTurnSpeed = 180f; // �C���̉�]���x

    public PlayerType playerType;    // �v���C���[�̃^�C�v

    private PlayerInput _playerInput;
    private Transform _firePoint;//Transform��privete�ɂ���
    private State _currentState = State.Idle; // �v���C���[�̌��݂̏��
    private bool isInvulnerable = false; //�A�C�e���擾���� ���Gtrue �ɂȂ�
    private Animator _animator;
    private Rigidbody _rb;

    protected override void Start()
    {
        base.Start(); // Start�ł͂Ȃ�Awake���Ă�
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
        // Player 1�p�܂���Player 2�p�̃A�N�V�����}�b�v��ݒ�
        if (playerType == PlayerType.Player1)
        {
            _playerInput.SwitchCurrentActionMap("Player");
        }
        else if (playerType == PlayerType.Player2)
        {
            _playerInput.SwitchCurrentActionMap("Player");
        }
        // PlayerInput�̃C�x���g�o�^
        _playerInput.actions[_shootActionName].performed += HandleShooting;
        _playerInput.actions["RocketShoot"].performed += HandleShooting;
        _playerInput.actions["HomingShoot"].performed += HandleShooting;

        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();

        if (!_playerInput.actions.Contains(_playerInput.actions[_shootActionName]))
        {
            Debug.LogError($"Action {_shootActionName} is not found in PlayerInput.");
        }

        GameManager.Instance.RegisterPlayer(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleTurretRotation();
    }
    public void HandleMovement()
    {
        // �Q�[���p�b�h�̈ړ����͂��擾
        Vector2 moveInput = _playerInput.actions[_shootActionName].ReadValue<Vector2>();
        float moveDirection = moveInput.y;  // �O��ړ�
        float turnDirection = moveInput.x;  // ���E��]

        MoveTank(moveDirection, turnDirection);

        // ��Ԃ��X�V
        _currentState = moveDirection != 0
            ? _currentState | State.Moving
            : _currentState & ~State.Moving;
    }

    private void MoveTank(float moveInput, float turnInput)
    {
        Vector3 moveDirection = transform.forward * moveInput * _moveSpeed * Time.deltaTime;
        transform.position += moveDirection;
        //Vector3 newPosition = _rb.position + moveDirection;
        //_rb.MovePosition(newPosition);

        //float turnAmount = turnInput * _turnSpeed * Time.deltaTime;
        float turnAmount = turnInput * 100f * Time.deltaTime;
        transform.Rotate(0f, turnAmount, 0f);
        //Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        //_rb.MoveRotation(_rb.rotation * turnRotation);
    }//�ړ�����
    private void HandleTurretRotation()
    {
        if (_turret == null) return;
        // �E�X�e�B�b�N�̓��͂ŖC���̉�]
        Vector2 moveInput = _playerInput.actions["TurretMove"].ReadValue<Vector2>();
        float turretTurnInput = moveInput.x;  // ���E�i����j

        float turretTurnAmount = turretTurnInput * _turretTurnSpeed * Time.deltaTime;
        //if (moveInput.sqrMagnitude > 0.1f)
        //{
        //    // ���͕�������p�x���v�Z
        //    float angle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
        //    Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
        //    _turret.rotation = Quaternion.RotateTowards(_turret.rotation, targetRotation, _turretTurnSpeed * Time.deltaTime);
        //}
        _turret.Rotate(0f, turretTurnAmount, 0f);
    }

    void HandleShooting(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.currentState != GameState.Playing)
        {
            Debug.Log("�Q�[��������Ȃ��̂Ŕ��˂ł��܂���I�I");
            return;
        }

        Quaternion rotation = default;
        if (_bulletShoot == null) return; // �����𒆒f
        IShootStrategy strategy = null;
        string actionName = context.action.name;

        // �v���C���[�̓��͂ɉ����Ēʏ�e�𔭎�
        if (actionName == _shootActionName)
        {
            strategy = new NormalShotStrategy();
            TriggerShootAnimation("shoot");
        }
        // ���P�b�g�e�̔��ˁiRocketShoot�A�N�V�����ɉ����ď����j
        else if (actionName == "RocketShoot")
        {
            strategy = new RocketShotStrategy();
            TriggerShootAnimation("rocketShoot");
        }
        //�z�[�~���O�e������
        else if (actionName == "HomingShoot")
        {
            strategy = new HomingShotStrategy();
            TriggerShootAnimation("shoot");
        }

        if (strategy != null)
        {
            _bulletShoot.SetStrategy(strategy);
            _bulletShoot.ShootByStrategy(playerType, transform.forward, transform.rotation);
        }
    }

    private void TriggerShootAnimation(string triggerName)
    {
        _animator.SetTrigger(triggerName);
        _currentState |= State.Shooting;
    }

    public override void TakeDamage(float damage)
    {
        Debug.Log($"{gameObject.name} before damage: {currentHealth}");
        currentHealth -= damage;
        if (hpBar == null)
        {
            Debug.LogError("HP�o�[���A�^�b�`����Ă��܂���I");
        }
        hpBar.UpdateHP(currentHealth, maxHealth);

        if (IsInvulnerable)
        {
            Debug.Log("���G��Ԃ̂��߃_���[�W���󂯂Ȃ��I");
            return;
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;  // HP�����̒l�ɂȂ�Ȃ��悤�ɂ���
            Die();
        }
        _currentState = State.Dead;
    }

    public bool IsInvulnerable
    {
        get { return isInvulnerable; }
        set
        {
            isInvulnerable = value;
            Debug.Log(isInvulnerable ? "���G���[�h ON!" : "���G���[�h OFF!");
        }
    }
    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        GameManager.Instance.RemovePlayer(this);
        OnPlayerDied?.Invoke(this);
        string winner = gameObject.name == "TankBlue(Player)" ? "TankRed(Enemy)" : "TankBlue(Player)";
        FindAnyObjectByType<GameManager>().CheckWinner(winner);
        base.Die();
    }
}
