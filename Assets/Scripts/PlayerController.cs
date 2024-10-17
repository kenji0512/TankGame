using UnityEngine;

public class PlayerController : Character
{
    public float moveSpeed = 5f;     // �^���N�̈ړ����x
    public float turnSpeed = 100f;   // �^���N�̉�]���x

    private void Update()
    {
        // �O�i�E��ނ̓��͂��擾 (Vertical Axis)
        float moveInput = Input.GetAxis("Vertical");
        // ���E��]�̓��͂��擾 (Horizontal Axis)
        float turnInput = Input.GetAxis("Horizontal");

        // �O�i�E��ނ̈ړ�����
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        transform.position += moveDirection;

        // ���E��]�̏���
        float turnAmount = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, turnAmount, 0f);
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
