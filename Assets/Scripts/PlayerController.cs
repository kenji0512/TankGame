using UnityEngine;

public class PlayerController : Character
{
    public float moveSpeed = 5f;     // �^���N�̈ړ����x
    public float turnSpeed = 100f;   // �^���N�̉�]���x

    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // �O�i�E��ނ̓��͂��擾 (Vertical Axis)
        float moveInput = Input.GetAxis("Vertical");
        // ���E��]�̓��͂��擾 (Horizontal Axis)
        float turnInput = Input.GetAxis("Horizontal");

        // �O�i�E��ނ̈ړ�����
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        // ���E��]�̏���
        Quaternion turnRotation = Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
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
