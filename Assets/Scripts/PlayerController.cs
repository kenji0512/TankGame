using UnityEngine;

public class PlayerController : Character
{
    public float moveSpeed = 5f;     // タンクの移動速度
    public float turnSpeed = 100f;   // タンクの回転速度

    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // 前進・後退の入力を取得 (Vertical Axis)
        float moveInput = Input.GetAxis("Vertical");
        // 左右回転の入力を取得 (Horizontal Axis)
        float turnInput = Input.GetAxis("Horizontal");

        // 前進・後退の移動処理
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        // 左右回転の処理
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
