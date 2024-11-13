using UnityEngine;

public class PlayerController : Character
{
    public float _moveSpeed = 5f;     // タンクの移動速度
    public float _turnSpeed = 100f;   // タンクの回転速度
    //public GameObject _bulletPrefab;
    public Transform _firePoint;
    [SerializeField] private BulletShoot _bulletShoot;
    public PlayerType _playerType;    // プレイヤーのタイプ
    private State _currentState = State.Idle; // プレイヤーの現在の状態
    private Animator _animator;
    private Rigidbody _rb;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    void HandleMovement()
    {
        float moveInput = 0f;
        float turnInput = 0f;

        if (_playerType == PlayerType.Player1)
        {
            moveInput = Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f);
            turnInput = Input.GetKey(KeyCode.A) ? -1f : (Input.GetKey(KeyCode.D) ? 1f : 0f);
        }
        else if (_playerType == PlayerType.Player2)
        {
            moveInput = Input.GetKey(KeyCode.UpArrow) ? 1f : (Input.GetKey(KeyCode.DownArrow) ? -1f : 0f);
            turnInput = Input.GetKey(KeyCode.LeftArrow) ? -1f : (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);
        }

        MoveTank(moveInput, turnInput);
        _currentState = moveInput != 0 || turnInput != 0 ? State.moving : State.Idle;
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

        if (_playerType == PlayerType.Player1 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            _bulletShoot.Shoot(_playerType, shootDirection);
            isShooting = true;
        }
        else if (_playerType == PlayerType.Player2 && Input.GetKeyDown(KeyCode.RightShift))
        {
            _bulletShoot.Shoot(_playerType, shootDirection);
            isShooting = true;
        }
        else if (_playerType == PlayerType.Player1 && Input.GetKeyDown(KeyCode.F))
        {
            isRocketShooting = true;
            _bulletShoot.RoketShoot(_playerType , shootDirection);
        }
        else if (_playerType == PlayerType.Player2 && Input.GetKeyDown(KeyCode.Backslash))
        {
            isRocketShooting = true;
            _bulletShoot.RoketShoot(_playerType , shootDirection);
        }

        if (isShooting)
        {
            _animator.SetTrigger("shoot");
            _currentState = State.Shooting;
        }
        if (isRocketShooting)
        {
            _animator.SetTrigger("roketShoot");
            _currentState = State.Shooting;
        }
    }//発射処理

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
        _currentState = State.Dead;
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        GameManager.Instance.RemovePlayer(this);
        base.Die();
    }
}
