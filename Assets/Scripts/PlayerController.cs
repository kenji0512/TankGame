using UnityEngine;

public class PlayerController : Character
{
    public float _moveSpeed = 5f;     // タンクの移動速度
    public float _turnSpeed = 100f;   // タンクの回転速度
    public GameObject _bulletprefab;
    public Transform _firePoint;
    public PlayerType _playerType;

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
            moveInput = Input.GetAxis("Vertical");   // W/Sキーで前後移動
            turnInput = Input.GetAxis("Horizontal"); // A/Dキーで左右回転
        }
        else if (_playerType == PlayerType.Player2)
        {
            moveInput = Input.GetAxis("Vertical2");  // 上/下矢印キーで前後移動
            turnInput = Input.GetAxis("Horizontal2"); // 左/右矢印キーで左右回転
        }

        // 移動・回転処理を統合
        MoveTank(moveInput, turnInput);
    }
    private void MoveTank(float moveInput, float turnInput)
    {
        // 前進・後退の移動処理
        Vector3 moveDirection = transform.forward * moveInput * _moveSpeed * Time.deltaTime;
        transform.position += moveDirection;

        // 左右回転の処理
        float turnAmount = turnInput * _turnSpeed * Time.deltaTime;
        transform.Rotate(0f, turnAmount, 0f);
    }
    void HandleShooting()
    {
        if (_playerType == PlayerType.Player1 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Player 1 攻撃
            Shoot();
        }
        else if (_playerType == PlayerType.Player2 && Input.GetKeyDown(KeyCode.Return))
        {
            // Player 2 攻撃
            Shoot();
        }
    }
    private void Shoot()
    {
        // 弾を生成して初期位置と方向を設定
        GameObject bullet = Instantiate(_bulletprefab, _firePoint.position, _firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetBulletType(Bullet.BulletType.Player); // プレイヤー用の弾
            Debug.Log(_playerType + " is shooting!");
        }
    }
    public override void TakeDamage(int damageAmount = 10)
    {
        base.TakeDamage(damageAmount);
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Game Over! The player has died.");
    }
}
