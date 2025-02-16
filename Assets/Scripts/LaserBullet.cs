using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    [SerializeField] float _laserRange = 30f;   // ���[�U�[�̎˒�����
    [SerializeField] int _damage = 3;           // ���[�U�[�̃_���[�W
    [SerializeField] LineRenderer _lineRenderer; // ���[�U�[�̎��o�G�t�F�N�g�p
    [SerializeField] float _laserDuration = 0.2f; // ���[�U�[�̕\������

    void Start()
    {
        FireLaser();
        Destroy(gameObject, _laserDuration); // �w�莞�Ԍ�ɍ폜
    }

    void FireLaser()
    {
        Vector3 start = transform.position;
        Vector3 direction = transform.forward;
        Vector3 end = start + direction * _laserRange;

        // Raycast�œG�𔻒�
        if (Physics.Raycast(start, direction, out RaycastHit hit, _laserRange))
        {
            // �q�b�g�����I�u�W�F�N�g�� ���� �Ȃ�_���[�W��^����
            TunkController hitplayer = hit.collider.GetComponent<TunkController>();
            if (hitplayer != null)
            {
                hitplayer.TakeDamage();
                end = hit.point; // ���[�U�[�̏I�_��G�ɕύX
            }
        }

        // LineRenderer �Ń��[�U�[�G�t�F�N�g��`��
        if (_lineRenderer != null)
        {
            _lineRenderer.SetPosition(0, start); // �J�n�n�_
            _lineRenderer.SetPosition(1, end);   // �I���n�_
        }
    }
}
