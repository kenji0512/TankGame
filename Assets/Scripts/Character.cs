using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // ダメージを受けたときに呼び出されるメソッド
    public virtual void TakeDamage(int damageAmount = 10)
    {
        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} took {damageAmount} damage. Remaining Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 死亡時に呼び出されるメソッド
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // オブジェクトを破壊
    }

    // 現在の体力を取得するメソッド
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
