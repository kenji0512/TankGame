using NetcodePlus.Demo;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    protected float currentHealth;
    public bool onPowerUp = false;  // アイテム取得時に パワーアップtrue になる

    public float _damageAmount { get; set; } = 10;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        //_originalDamage = _damageAmount;
        Debug.Log($"{gameObject.name} Initial Health: {currentHealth}"); // 初期体力のログ表示
    }

    // ダメージを受けたときに呼び出されるメソッド
    public virtual void TakeDamage(float intdamage)
    {

    }
    // 死亡時に呼び出されるメソッド
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // オブジェクトを破壊
    }
}
