using UnityEngine;

public class PlayerController : Character
{
    public float _moveSpeed = 5f;     // �^���N�̈ړ����x
    public float _turnSpeed = 100f;   // �^���N�̉�]���x
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
            moveInput = Input.GetAxis("Vertical");   // W/S�L�[�őO��ړ�
            turnInput = Input.GetAxis("Horizontal"); // A/D�L�[�ō��E��]
        }
        else if (_playerType == PlayerType.Player2)
        {
            moveInput = Input.GetAxis("Vertical2");  // ��/�����L�[�őO��ړ�
            turnInput = Input.GetAxis("Horizontal2"); // ��/�E���L�[�ō��E��]
        }

        // �ړ��E��]�����𓝍�
        MoveTank(moveInput, turnInput);
    }
    private void MoveTank(float moveInput, float turnInput)
    {
        // �O�i�E��ނ̈ړ�����
        Vector3 moveDirection = transform.forward * moveInput * _moveSpeed * Time.deltaTime;
        transform.position += moveDirection;

        // ���E��]�̏���
        float turnAmount = turnInput * _turnSpeed * Time.deltaTime;
        transform.Rotate(0f, turnAmount, 0f);
    }
    void HandleShooting()
    {
        if (_playerType == PlayerType.Player1 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Player 1 �U��
            Shoot();
        }
        else if (_playerType == PlayerType.Player2 && Input.GetKeyDown(KeyCode.Return))
        {
            // Player 2 �U��
            Shoot();
        }
    }
    private void Shoot()
    {
        // �e�𐶐����ď����ʒu�ƕ�����ݒ�
        GameObject bullet = Instantiate(_bulletprefab, _firePoint.position, _firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetBulletType(Bullet.BulletType.Player); // �v���C���[�p�̒e
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
