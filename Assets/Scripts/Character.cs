using NetcodePlus.Demo;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    protected float currentHealth;
    public float _damageAmount { get; set; } = 10;
    Item item;    // Item クラスのインスタンスを参照

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        Debug.Log($"{gameObject.name} Initial Health: {currentHealth}"); // 初期体力のログ表示
    }

    // ダメージを受けたときに呼び出されるメソッド
    public virtual void TakeDamage()
    {
        Debug.Log($"{gameObject.name} before damage: {currentHealth}"); // ダメージ前のHP表示
        Debug.Log($"{gameObject.name} applying damage: {_damageAmount}"); // ダメージ量のログ
        // パワーアップが有効な場合、ダメージに追加で 10 を加える
        if (item != null && item.OnPowerUp)
        {
            currentHealth -= (_damageAmount + item._value);
            Debug.Log($"{gameObject.name} OnPowerUp is active! Extra damage applied.");
        }
        else
        {
            currentHealth -= _damageAmount;
        }
        Debug.Log($"{gameObject.name} took {_damageAmount} damage. Remaining Health: {currentHealth}");

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
}
