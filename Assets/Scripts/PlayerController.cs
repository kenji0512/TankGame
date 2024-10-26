using UnityEngine;

public class PlayerController : Character
{
    public float moveSpeed = 5f;     // タンクの移動速度
    public float turnSpeed = 100f;   // タンクの回転速度
    public GameObject _bulletprefab;
    public Transform _firePoint;

    private void Update()
    {
        // 1Pの操作 (WASD)
        HandlePlayerOne();

        // 2Pの操作 (矢印キー)
        HandlePlayerTwo();
    }
    void HandlePlayerOne()
    {
        float moveInput = 0; // W/S
        float turnInput = 0; // A/D

        // WASDによる移動
        if (Input.GetKey(KeyCode.W))
        {
            moveInput = 1; // 前進
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveInput = -1; // 後退
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnInput = -1; // 左回転
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnInput = 1; // 右回転
        }
        // 弾の発射
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Shoot();
        }
    }
    void HandlePlayerTwo()
    {
        float moveInput = 0;
        float turnInput = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveInput = 1; // 上
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveInput = -1; // 下
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            turnInput = 1; // 右
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            turnInput = -1; // 左
        }

        MoveTank(moveInput, turnInput);

        // 弾の発射
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Shoot();
        }
    }
    private void MoveTank(float moveInput, float turnInput)
    {
        // 前進・後退の移動処理
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        transform.position += moveDirection;

        // 左右回転の処理
        float turnAmount = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, turnAmount, 0f);
    }
    private void Shoot()
    {
        // 弾を生成して初期位置と方向を設定
        GameObject bullet = Instantiate(_bulletprefab, _firePoint.position, _firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetBulletType(Bullet.BulletType.Player); // プレイヤー用の弾
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
