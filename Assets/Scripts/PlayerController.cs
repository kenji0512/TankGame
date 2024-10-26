using UnityEngine;

public class PlayerController : Character
{
    public float moveSpeed = 5f;     // �^���N�̈ړ����x
    public float turnSpeed = 100f;   // �^���N�̉�]���x
    public GameObject _bulletprefab;
    public Transform _firePoint;

    private void Update()
    {
        // 1P�̑��� (WASD)
        HandlePlayerOne();

        // 2P�̑��� (���L�[)
        HandlePlayerTwo();
    }
    void HandlePlayerOne()
    {
        float moveInput = 0; // W/S
        float turnInput = 0; // A/D

        // WASD�ɂ��ړ�
        if (Input.GetKey(KeyCode.W))
        {
            moveInput = 1; // �O�i
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveInput = -1; // ���
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnInput = -1; // ����]
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnInput = 1; // �E��]
        }
        // �e�̔���
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
            moveInput = 1; // ��
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveInput = -1; // ��
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            turnInput = 1; // �E
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            turnInput = -1; // ��
        }

        MoveTank(moveInput, turnInput);

        // �e�̔���
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Shoot();
        }
    }
    private void MoveTank(float moveInput, float turnInput)
    {
        // �O�i�E��ނ̈ړ�����
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        transform.position += moveDirection;

        // ���E��]�̏���
        float turnAmount = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, turnAmount, 0f);
    }
    private void Shoot()
    {
        // �e�𐶐����ď����ʒu�ƕ�����ݒ�
        GameObject bullet = Instantiate(_bulletprefab, _firePoint.position, _firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetBulletType(Bullet.BulletType.Player); // �v���C���[�p�̒e
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
