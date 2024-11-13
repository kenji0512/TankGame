using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;   // �e�̈ړ����x
    [SerializeField] private float lifetime = 5f; // �e�̎����i�b�j
    [SerializeField] public GameObject _hitEffectPrefab;//�Փ˃G�t�F�N�g�̃v���n�u
    [SerializeField] public int damageAmount = 10;//�_���[�W��
    public PlayerType shooterType;// ���ˎ҂̃v���C���[�^�C�v

    protected virtual void Start()
    {
        // ��莞�Ԍ�ɒe�������Ŕj��
        Destroy(gameObject, lifetime);
    }
    private Vector3 _direction;  // �e�̈ړ�����

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
    protected virtual void Update()
    {
        // �e��O���Ɉړ�������
        transform.position += _direction * speed * Time.deltaTime;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            //var targetCharacter = other.GetComponent<Character>();
            PlayerController hitPlayer = other.GetComponent<PlayerController>();
            Debug.Log($"hitPlayer.type : {hitPlayer._playerType}\nshooterType : {shooterType}");

            if (hitPlayer._playerType != shooterType)
            {
                HandleCharacterCollision(hitPlayer);
                Debug.Log($"Bullet hit {hitPlayer.gameObject.name} and dealt damage! Remaining Health: {hitPlayer.GetCurrentHealth()}");
                // �Փ˂������肪���ˎ҂łȂ��ꍇ�A�������s��
                Destroy(gameObject); // �e������
            }
        }
        else if (other.CompareTag("BreakableWall")) // �ǂƂ̏Փˏ�����ǉ�
        {
            var breakableWall = other.GetComponent<BreakableWall>();
            if (breakableWall != null)
            {
                HandleWallCollision(breakableWall);
            }
            Debug.Log($"Bullet hit {breakableWall}");
        }
    }
    protected virtual void HandleWallCollision(BreakableWall breakableWall)
    {
        breakableWall.Damage(); // �ǂɃ_���[�W��^����
        CreateHitEffect(); // �Փ˃G�t�F�N�g�𐶐�
        Destroy(gameObject); // �e��j��
    }
    protected virtual void HandleCharacterCollision(PlayerController hitPlayer)
    {
        // �v���C���[�ɓ��������ꍇ�A�_���[�W��^����
        hitPlayer.TakeDamage(damageAmount);
        CreateHitEffect();
        Destroy(gameObject); // �e��j��
        Debug.Log("Destroy" + gameObject.name);
    }
    protected void CreateHitEffect()
    {
        if (_hitEffectPrefab != null)
        {
            // �Փ˃G�t�F�N�g�𐶐�
            GameObject hitEffect = Instantiate(_hitEffectPrefab, transform.position, Quaternion.identity);

            // �G�t�F�N�g��3�b��Ɏ����ŏ���
            Destroy(hitEffect, 2f);
        }
    }

}
