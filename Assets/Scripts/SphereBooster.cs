using UnityEngine;

public class SphereBooster : Bullet
{
    [SerializeField] private float _forceMagnitude = 10.0f; // �͂̑傫��
    [SerializeField] private float _gravity = -9.81f; // �d�͂̋���
    private float traveledDistance = 0f; // �݌v�ړ�����
    private const float maxDistance = 30f; // �ő�˒�

    private Vector3 _velocity; // �����x
    private Vector3 _gravityEffect; // �d�͂ɂ��e��
                                    //public PlayerType shooterType;

    // ���ˈʒu��n���ď����x��ݒ�
    public void Initialize(Vector3 initialDirection)
    {
        // ���˕����ishootPoint�̌����Ă�������j���擾���āA�����x��ݒ�
        _velocity = initialDirection.normalized * _forceMagnitude;

        // �d�͂̉e����ݒ� (Y�������ɂ̂݉e��)
        _gravityEffect = new Vector3(0, _gravity, 0);

    }

    private void Update()
    {
        _velocity += _gravityEffect * Time.deltaTime;
        Vector3 delta = _velocity * Time.deltaTime;
        transform.position += delta;

        traveledDistance += delta.magnitude;
        if (traveledDistance >= maxDistance)
        {
            Destroy(gameObject); // �˒��𒴂�����e��j��
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmo�̐F��ݒ�
        Gizmos.color = Color.red;
        // �ˏo�����̖���`��
        Vector3 arrowEnd = transform.position + _velocity.normalized * _forceMagnitude;
        Gizmos.DrawLine(transform.position, arrowEnd); // ���˕���
        Gizmos.DrawSphere(arrowEnd, 0.1f); // ���̐�[

        // �˒��͈͂������~��`��i�K�v�ɉ����Ĕ͈͂�ݒ�\�j
        Gizmos.DrawWireSphere(transform.position, 5.0f);
    }
}
