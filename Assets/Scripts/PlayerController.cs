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
    Animator _animator;
    private Rigidbody _rb;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponentInChildren<Animator>(); // �q�I�u�W�F�N�g���� Animator ���擾
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
        Vector3 newPosition = _rb.position + moveDirection;
        _rb.MovePosition(newPosition);

        // ���E��]�̏���
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
            // Player 1 �U��
            _bulletShoot.Shoot(_playerType); // BulletShoot �X�N���v�g�� Shoot ���\�b�h���Ăяo��
            isShooting = true;
            Debug.Log(_playerType + " is shooting!");
        }
        else if (_playerType == PlayerType.Player2 && Input.GetButtonDown("RightShift"))
        {
            // Player 2 �U��
            _bulletShoot.Shoot(_playerType); // BulletShoot �X�N���v�g�� Shoot ���\�b�h���Ăяo��
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
        while (true) // �i�v���[�v�Ȃ̂œK�؂ȏ�����ݒ�
        {
            _bulletShoot.Shoot(_playerType);
            yield return new WaitForSeconds(0.5f); // 0.5�b�Ԋu�Ŕ���
        }
    }
    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        GameManager.Instance.RemovePlayer(this); // GameManager����v���C���[���폜
        base.Die(); // �e�N���X��Die���\�b�h���Ăяo���āA�I�u�W�F�N�g��j��
    }
}
