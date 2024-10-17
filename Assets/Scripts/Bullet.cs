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
    [SerializeField] private float speed = 20f;   // ’e‚ÌˆÚ“®‘¬“x
    [SerializeField] private float lifetime = 5f; // ’e‚Ìõ–½i•bj

    private void Start()
    {
        // ˆê’èŠÔŒã‚É’e‚ğ©“®‚Å”j‰ó
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // ’e‚ğ‘O•û‚ÉˆÚ“®‚³‚¹‚é
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out var targetCharacter))
        {
            // ƒvƒŒƒCƒ„[—p‚Ì’e‚ª“G‚É“–‚½‚Á‚½ê‡
            if (_type == BulletType.Player && targetCharacter is Enemy)
            {
                targetCharacter.TakeDamage();
                Destroy(gameObject); // ’e‚ğ”j‰ó
            }
            // “G—p‚Ì’e‚ªƒvƒŒƒCƒ„[‚É“–‚½‚Á‚½ê‡
            else if (_type == BulletType.Enemy && targetCharacter is Player)
            {
                targetCharacter.TakeDamage();
                Destroy(gameObject); // ’e‚ğ”j‰ó
            }
        }
    }
}
