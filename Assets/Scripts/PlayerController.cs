using System.Collections;
using UnityEngine;

public class PlayerController : Character
{
    public float _moveSpeed = 5f;     // タンクの移動速度
    public float _turnSpeed = 100f;   // タンクの回転速度
    public GameObject _bulletprefab;
    public Transform _firePoint;
    [SerializeField] private BulletShoot _bulletShoot;
    public PlayerType _playerType;
    Animator _animator;
    private Rigidbody _rb;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponentInChildren<Animator>(); // 子オブジェクトから Animator を取得
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }
    void HandleMovement()
    {
        float moveInput = 1f;
        float turnInput = 1f;
        if (_playerType == PlayerType.Player1)
        {
            if (Input.GetKey(KeyCode.W))
                moveInput = 1f;
            else if (Input.GetKey(KeyCode.S))
                moveInput = -1f;
            else
                moveInput = 0;
            // W/Sキーで前後移動

            if (Input.GetKey(KeyCode.A))
                turnInput = -1;
            else if (Input.GetKey(KeyCode.D))
                turnInput = 1;
            else
                turnInput = 0;
            // A/Dキーで左右回転
        }

        else if (_playerType == PlayerType.Player2)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                moveInput = 1f;
            else if (Input.GetKey(KeyCode.DownArrow))
                moveInput = -1f;
            else
                moveInput = 0;
            // 上/下矢印キーで前後移動

            if (Input.GetKey(KeyCode.LeftArrow))
                turnInput = -1;
            else if (Input.GetKey(KeyCode.RightArrow))
                turnInput = 1;
            else
                turnInput = 0;
            // 左/右矢印キーで左右回転
        }

        // 移動・回転処理を統合
        MoveTank(moveInput, turnInput);
    }
    private void MoveTank(float moveInput, float turnInput)
    {
        // 前進・後退の移動処理
        Vector3 moveDirection = transform.forward * moveInput * _moveSpeed * Time.deltaTime;
        Vector3 newPosition = _rb.position + moveDirection;
        _rb.MovePosition(newPosition);

        // 左右回転の処理
        float turnAmount = turnInput * _turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        _rb.MoveRotation(_rb.rotation * turnRotation);
    }
    void HandleShooting()
    {
        bool isShooting = false;
        bool isroketShooting = false;

        if (_playerType == PlayerType.Player1 && Input.GetButtonDown("LeftShift"))
        {
            // Player 1 攻撃
            _bulletShoot.Shoot(_playerType); // BulletShoot スクリプトの Shoot メソッドを呼び出す
            isShooting = true;
            Debug.Log(_playerType + " is shooting!");
        }
        else if (_playerType == PlayerType.Player2 && Input.GetButtonDown("RightShift"))
        {
            // Player 2 攻撃
            _bulletShoot.Shoot(_playerType); // BulletShoot スクリプトの Shoot メソッドを呼び出す
            isShooting = true;
            Debug.Log(_playerType + " is shooting!");
        }else if (_playerType == PlayerType.Player1 && Input.GetKeyDown(KeyCode.F))
        {
            isroketShooting = true;
            _bulletShoot.RoketShoot(_playerType);
        }
        else if (_playerType == PlayerType.Player2 && Input.GetKeyDown(KeyCode.Backslash))
        {
            isroketShooting = true;
            _bulletShoot.RoketShoot(_playerType);
        }

        if (isShooting)
        {
            _animator.SetTrigger("shoot");
        }
        if (isroketShooting)
        {
            _animator.SetTrigger("roketShoot");
        }
    }

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
    }
    private IEnumerator ShootCoroutine()
    {
        while (true) // 永久ループなので適切な条件を設定
        {
            _bulletShoot.Shoot(_playerType);
            yield return new WaitForSeconds(0.5f); // 0.5秒間隔で発射
        }
    }
    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        GameManager.Instance.RemovePlayer(this); // GameManagerからプレイヤーを削除
        base.Die(); // 親クラスのDieメソッドを呼び出して、オブジェクトを破壊
    }
}
