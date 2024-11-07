using UnityEngine;

public class SphereBooster : Bullet
{
    [SerializeField] private float _forceMagnitude = 10.0f; // �͂̑傫��
    [SerializeField] private float _gravity = -9.81f; // �d�͂̋���

    private Vector3 _velocity; // �����x
    private Vector3 _gravityEffect; // �d�͂ɂ��e��

    //void Start()
    //{
    //    Transform shootPoint = FindObjectOfType<BulletShoot>()._shootpointR;
    //    base.Start();
    //    // �����x��ݒ�i�͂̑傫���ƕ�������v�Z�j
    //    _velocity = shootPoint.forward.normalized * _forceMagnitude;

    //    // �d�͂̉e����ݒ� (Y�������ɂ̂݉e��)
    //    _gravityEffect = new Vector3(0, _gravity, 0);
    //}
    // ���ˈʒu��n���ď����x��ݒ�
    public void Initialize(Vector3 initialDirection, PlayerType shooter)
    {
        base.Start();
        // ���˕����ishootPoint�̌����Ă�������j���擾���āA�����x��ݒ�
        _velocity = initialDirection.normalized * _forceMagnitude;

        // �d�͂̉e����ݒ� (Y�������ɂ̂݉e��)
        _gravityEffect = new Vector3(0, _gravity, 0);
    }

    protected override void Update()
    {
        // �d�͂̉e����������
        _velocity += _gravityEffect * Time.deltaTime;

        // �t���[�����ƂɈʒu���X�V
        transform.position += _velocity * Time.deltaTime;
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
