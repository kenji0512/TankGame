using UnityEngine;

public class SphereBooster : MonoBehaviour
{
    [SerializeField] private float _forceMagnitude = 10.0f; // �͂̑傫��
    [SerializeField] private Vector3 _initialDirection = new Vector3(1.0f, 1.0f, 0f); // �����ˏo����
    [SerializeField] private float _gravity = -9.81f; // �d�͂̋���

    private Vector3 _velocity; // �����x
    private Vector3 _gravityEffect; // �d�͂ɂ��e��

    void Start()
    {
        // �����x��ݒ�i�͂̑傫���ƕ�������v�Z�j
        _velocity = _initialDirection.normalized * _forceMagnitude;

        // �d�͂̉e����ݒ� (Y�������ɂ̂݉e��)
        _gravityEffect = new Vector3(0, _gravity, 0);
    }

    void Update()
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
        Vector3 arrowEnd = transform.position + _initialDirection.normalized * _forceMagnitude;
        Gizmos.DrawLine(transform.position, arrowEnd); // ���˕���
        Gizmos.DrawSphere(arrowEnd, 0.1f); // ���̐�[

        // �˒��͈͂������~��`��i�K�v�ɉ����Ĕ͈͂�ݒ�\�j
        Gizmos.DrawWireSphere(transform.position, 5.0f);
    }
}
