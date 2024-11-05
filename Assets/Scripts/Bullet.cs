using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;   // 弾の移動速度
    [SerializeField] private float lifetime = 5f; // 弾の寿命（秒）
    [SerializeField] public GameObject _hitEffectPrefab;//衝突エフェクトのプレハブ
    [SerializeField] public int damageAmount = 10;//ダメージ量

    protected virtual void Start()
    {
        // 一定時間後に弾を自動で破壊
        Destroy(gameObject, lifetime);
    }

    protected virtual void Update()
    {
        // 弾を前方に移動させる
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var targetCharacter = other.GetComponent<Character>();
            if (targetCharacter != null)
            {
                HandleCharacterCollision(targetCharacter);
            }
            Debug.Log("Bullet hit Player and dealt damage!");
        }
        else if (other.CompareTag("BreakableWall")) // 壁との衝突処理を追加
        {
            var breakableWall = other.GetComponent<BreakableWall>();
            if (breakableWall != null)
            {
                HandleWallCollision(breakableWall);
            }
        }
    }
    protected virtual void HandleWallCollision(BreakableWall breakableWall)
    {
        breakableWall.Damage(); // 壁にダメージを与える
        CreateHitEffect(); // 衝突エフェクトを生成
        Destroy(gameObject); // 弾を破壊
    }
    protected virtual void HandleCharacterCollision(Character targetCharacter)
    {
        // プレイヤーに当たった場合、ダメージを与える
        targetCharacter.TakeDamage();
        CreateHitEffect();
        Destroy(gameObject); // 弾を破壊
        Debug.Log("Destroy" + gameObject.name);
    }
    protected void CreateHitEffect()
    {
        if (_hitEffectPrefab != null)
        {
            // 衝突エフェクトを生成
            GameObject hitEffect = Instantiate(_hitEffectPrefab, transform.position, Quaternion.identity);

            // エフェクトを3秒後に自動で消去
            Destroy(hitEffect, 2f);
        }
    }

}
