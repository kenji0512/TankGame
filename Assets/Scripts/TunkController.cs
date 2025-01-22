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
    public float _moveSpeed = 5f;     // �^���N�̈ړ����x
    public float _turnSpeed = 100f;   // �^���N�̉�]���x
    private Transform _firePoint;//Transform��privete�ɂ���
    [SerializeField] private BulletShoot _bulletShoot;
    public PlayerType _playerType;    // �v���C���[�̃^�C�v
    private State _currentState = State.Idle; // �v���C���[�̌��݂̏��
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
        //_playerInput.actions[_moveActionName].performed += HandleMovement;
        _playerInput.actions[_shootActionName].performed += HandleShooting;

        //_moveAction = _playerInput.actions["Move"];
        //_shootAction = _playerInput.actions["Shoot"];
        //_rotateAction = _playerInput.actions["Rotate"];

        // �A�N�V������L����
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
        // �Q�[���p�b�h�̈ړ����͂��擾
        Vector2 moveInput = _playerInput.actions[_moveActionName].ReadValue<Vector2>();
        float moveDirection = moveInput.y;  // �O��ړ�
        float turnDirection = moveInput.x;  // ���E��]

        MoveTank(moveDirection, turnDirection);

        // ��Ԃ��X�V
        _currentState = moveDirection != 0 || turnDirection != 0
            ? _currentState | State.Moving
            : _currentState & ~State.Moving;
        //float moveInput = 0f;
        //float turnInput = 0f;

        //// �v���C���[���Ƃɑ�����@��ݒ�
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
    }//�ړ�����

    void HandleShooting(InputAction.CallbackContext context)
    {
        Vector3 shootDirection = transform.forward; // ���˕���
        //bool isShooting = false;
        //bool isRocketShooting = false;

        // �v���C���[�̓��͂ɉ����Ēe�𔭎�
        if (_playerInput.actions[_shootActionName].triggered)
        {
            // �ʏ�e�̔���
            _bulletShoot.Shoot(_playerType, transform.forward);
            TriggerShootAnimation("shoot");
        }
        else if ((_playerType == PlayerType.Player1 && context.control.device == Keyboard.current) ||
        (_playerType == PlayerType.Player2 && context.control.device == Keyboard.current))
        {
            // ���P�b�g�e�̔���
            _bulletShoot.RocketShoot(_playerType, transform.forward);
            TriggerShootAnimation("rocketShoot");
        }

        //// �v���C���[�̓��͂ɉ����Ēe�𔭎�
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
        //    _animator.SetTrigger("rocketShoot"); // ���P�b�g�p�̃g���K�[��ݒ�
        //    _currentState = _currentState | State.Shooting;
        //}
        //else if (isShooting)
        //{
        //    _animator.SetTrigger("shoot"); // �ʏ�̒e�p�g���K�[
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
