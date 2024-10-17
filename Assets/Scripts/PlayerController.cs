using UnityEngine;

public class PlayerController : Character
{
    public float moveSpeed = 5f;     // タンクの移動速度
    public float turnSpeed = 100f;   // タンクの回転速度

    private void Update()
    {
        // 前進・後退の入力を取得 (Vertical Axis)
        float moveInput = Input.GetAxis("Vertical");
        // 左右回転の入力を取得 (Horizontal Axis)
        float turnInput = Input.GetAxis("Horizontal");

        // 前進・後退の移動処理
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        transform.position += moveDirection;

        // 左右回転の処理
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
