using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Invalid,
        Player,
        Enemy,
    }

    [SerializeField] private BulletType _type;
    [SerializeField] private float speed = 20f;   // 弾の移動速度
    [SerializeField] private float lifetime = 5f; // 弾の寿命（秒）
    [SerializeField] public GameObject _hitEffectPrefab;//衝突エフェクトのプレハブ

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
        if (other.TryGetComponent<BreakableWall>(out var breakableWall))
        {
            breakableWall.Damage(); //壁にダメージを与える
            CreateHitEffect(); //衝突エフェクトを生成
            Destroy(gameObject);//弾を墓石
            return;//ここで処理を終了
        }
        if (other.TryGetComponent<Character>(out var targetCharacter))
        {
            // プレイヤー用の弾が敵に当たった場合
            if (_type == BulletType.Player && targetCharacter is Enemy)
            {
                targetCharacter.TakeDamage();
                CreateHitEffect();
                Destroy(gameObject); // 弾を破壊
            }
            // 敵用の弾がプレイヤーに当たった場合
            else if (_type == BulletType.Enemy && targetCharacter is Player)
            {
                CreateHitEffect();
                targetCharacter.TakeDamage();
                Destroy(gameObject); // 弾を破壊
            }
        }

    }
    private void CreateHitEffect()
    {
        if (_hitEffectPrefab != null)
        {
            //衝突エフェクトを生成
            Instantiate(_hitEffectPrefab, transform.position,Quaternion.identity);
        }
    }
    // 新しいメソッドを追加して弾の種類を設定できるようにする
    public void SetBulletType(BulletType type)
    {
        _type = type;
    }
}
