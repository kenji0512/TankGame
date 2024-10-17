using UnityEngine;

public class Enemy : Character
{
    protected override void Start()
    {
        base.Start();
        // 敵固有の初期化処理をここに追加
    }

    public override void TakeDamage(int damageAmount = 10)
    {
        base.TakeDamage(damageAmount);
        // 敵がダメージを受けた時の追加処理をここに追加（例：怒り状態になる）
    }

    protected override void Die()
    {
        base.Die();
        // 敵が死んだ時の追加処理（例：ポイントを追加）
        Debug.Log("An enemy has been defeated!");
    }
}