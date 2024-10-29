using System.Collections;
using UnityEngine;

public class PlayerController : Character
{
    public float _moveSpeed = 5f;     // �^���N�̈ړ����x
    public float _turnSpeed = 100f;   // �^���N�̉�]���x
    public GameObject _bulletprefab;
    public Transform _firePoint;
    [SerializeField] private BulletShoot _bulletShoot;
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
            if (Input.GetKey(KeyCode.W))
                moveInput = 1f;
            else if (Input.GetKey(KeyCode.S))
                moveInput = -1f;
            else
                moveInput = 0;
            // W/S�L�[�őO��ړ�

            if (Input.GetKey(KeyCode.A))
                turnInput = -1;
            else if (Input.GetKey(KeyCode.D))
                turnInput = 1;
            else
                turnInput = 0;
            // A/D�L�[�ō��E��]
        }

        else if (_playerType == PlayerType.Player2)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                moveInput = 1f;
            else if (Input.GetKey(KeyCode.DownArrow))
                moveInput = -1f;
            else
                moveInput = 0;
            // ��/�����L�[�őO��ړ�

            if (Input.GetKey(KeyCode.LeftArrow))
                turnInput = -1;
            else if (Input.GetKey(KeyCode.RightArrow))
                turnInput = 1;
            else
                turnInput = 0;
            // ��/�E���L�[�ō��E��]
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
        if (_playerType == PlayerType.Player1 && Input.GetButtonDown("LeftShift"))
        {
            // Player 1 �U��
            _bulletShoot.Shoot(); // BulletShoot �X�N���v�g�� Shoot ���\�b�h���Ăяo��
            Debug.Log(_playerType + " is shooting!");
        }
        else if (_playerType == PlayerType.Player2 && Input.GetButtonDown("RightShift"))
        {
            // Player 2 �U��
            _bulletShoot.Shoot(); // BulletShoot �X�N���v�g�� Shoot ���\�b�h���Ăяo��
            Debug.Log(_playerType + " is shooting!");
        }
    }

    public override void TakeDamage(int damageAmount = 10)
    {
        base.TakeDamage(damageAmount);
    }
    private IEnumerator ShootCoroutine()
    {
        while (true) // �i�v���[�v�Ȃ̂œK�؂ȏ�����ݒ�
        {
            _bulletShoot.Shoot();
            yield return new WaitForSeconds(0.5f); // 0.5�b�Ԋu�Ŕ���
        }
    }
    protected override void Die()
    {
        base.Die();
        Debug.Log("Game Over! The player has died.");
    }
}
