using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TunkController : Character
{
    public static event Action<TunkController> OnPlayerDied;
    private PlayerInput _playerInput;
    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _shootActionName = "Shoot";
    public float _moveSpeed = 5f;     // �^���N�̈ړ����x
    public float _turnSpeed = 100f;   // �^���N�̉�]���x
    private Transform _firePoint;//Transform��privete�ɂ���
    [SerializeField] private BulletShoot _bulletShoot;
    public PlayerType _playerType;    // �v���C���[�̃^�C�v
    private State _currentState = State.Idle; // �v���C���[�̌��݂̏��
    private HomingMissile _missile;
    private Animator _animator;
    private Rigidbody _rb;

    protected override void Awake()
    {
        base.Awake(); // Start�ł͂Ȃ�Awake���Ă�
        _playerInput = GetComponent<PlayerInput>();
        // UnityEvent�o�R�Ŏ󂯎��悤�ɐݒ�
        _playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;

        // Player 1�p�܂���Player 2�p�̃A�N�V�����}�b�v��ݒ�
        if (_playerType == PlayerType.Player1)
        {
            _playerInput.SwitchCurrentActionMap("Player1");
        }
        else if (_playerType == PlayerType.Player2)
        {
            _playerInput.SwitchCurrentActionMap("Player1");
        }
        // PlayerInput�̃C�x���g�o�^
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
        // �Q�[���p�b�h�̈ړ����͂��擾
        Vector2 moveInput = _playerInput.actions[_moveActionName].ReadValue<Vector2>();
        float moveDirection = moveInput.y;  // �O��ړ�
        float turnDirection = moveInput.x;  // ���E��]

        MoveTank(moveDirection, turnDirection);

        // ��Ԃ��X�V
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
    }//�ړ�����

    void HandleShooting(InputAction.CallbackContext context)
    {
        Vector3 shootDirection = transform.forward; // ���˕���

        // �v���C���[�̓��͂ɉ����Ēe�𔭎�
        if (_playerInput.actions[_shootActionName].triggered)
        {
            // �ʏ�e�̔���
            _bulletShoot.Shoot(_playerType, transform.forward);
            TriggerShootAnimation("shoot");
        }

        // ���P�b�g�e�̔��ˁiRocketShoot�A�N�V�����ɉ����ď����j
        if (context.action.name == "RocketShoot") // ���P�b�g�e�̏ꍇ�̃`�F�b�N
        {
            Debug.Log($"Rocket Shoot by {_playerType}"); // ���P�b�g�e�̔��˂��Ă΂ꂽ���Ƃ��m�F
            _bulletShoot.RocketShoot(_playerType, shootDirection);
            TriggerShootAnimation("rocketShoot");
        }
        //�z�[�~���O�e������
        if (context.action.name == "HomingShoot")
        {

            Transform target = _missile.FindClosestTarget(); // �ł��߂��^�[�Q�b�g���擾
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