using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;   // 弾の移動速度
    [SerializeField] private float lifetime = 5f; // 弾の寿命（秒）
    [SerializeField] public GameObject _hitEffectPrefab;//衝突エフェクトのプレハブ
    [SerializeField] public int damageAmount = 10;//ダメージ量

    private void Start()
    {
        // 一定時間後に弾を自動で破壊
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // 弾を前方に移動させる
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
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
    }
    private void HandleWallCollision(BreakableWall breakableWall)
    {
        breakableWall.Damage(); // 壁にダメージを与える
        CreateHitEffect(); // 衝突エフェクトを生成
        Destroy(gameObject); // 弾を破壊
    }
    private void HandleCharacterCollision(Character targetCharacter)
    {
        // プレイヤーに当たった場合、ダメージを与える
        targetCharacter.TakeDamage();
        CreateHitEffect();
        Destroy(gameObject); // 弾を破壊
        Debug.Log("Destroy" + gameObject.name);
    }
    private void CreateHitEffect()
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
