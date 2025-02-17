using System.Collections;
using UnityEngine;

public enum PowerupType
{
    None = 0,
    Power = 10,
    Speed = 20, // value in percentage
    Invulnerable = 30
}

/// <summary>
/// Powerups that can be taken by tanks
/// </summary>
public class Item : MonoBehaviour
{
    public PowerupType type;
    public float _value = 10;
    public float _duration = 3f;
    public float _itemRespawnDuration = 10f;

    [Header("Ref")]
    public SpriteRenderer sprite;
    public SphereCollider sphereCollider;
    public ParticleSystem pickupEffect; // �擾���̃G�t�F�N�g�i��Œǉ��p�j

    private bool _taken = false;
    private float originalMoveSpeed; // ���̈ړ����x��ێ����邽�߂̕ϐ�
    private float originalDamage;

    public void Take(TunkController tunkController)
    {
        if (!_taken)
        {
            _taken = true;
            ApplyBonus(tunkController); // ��Ɍ��ʂ�K�p
            StartCoroutine(DeactivateAndRespawnCoroutine());
        }
    }

    private void ApplyBonus(TunkController tunkController)
    {
        if (tunkController != null)
        {
            switch (type)
            {
                case PowerupType.Power:
                    ApplyPowerBoost(tunkController);
                    break;
                case PowerupType.Speed:
                    ApplySpeedBoost(tunkController);
                    break;
                case PowerupType.Invulnerable:
                    ApplyInvulnerability(tunkController);
                    break;
            }
        }
    }
    private void ApplyPowerBoost(TunkController tunkController)
    {
        Debug.Log($"{tunkController.name} �Ƀp���[�A�b�v�K�p");
        tunkController.onPowerUp = true;
        originalDamage = tunkController._damageAmount;// ���̃p���[��ۑ�
        tunkController._damageAmount += _value;
        StartCoroutine(ResetPowerAfterDuration(tunkController));
    }
    private IEnumerator ResetPowerAfterDuration(TunkController tunkController)
    {
        yield return new WaitForSeconds(_duration);
        tunkController._damageAmount = originalDamage;
        tunkController.onPowerUp = false;
        Debug.Log($"false");
    }
    private void ApplySpeedBoost(TunkController tunkController)
    {
        originalMoveSpeed = tunkController._moveSpeed; // ���̃X�s�[�h��ۑ�
        tunkController._moveSpeed *= (1f + (_value / 100f));
        StartCoroutine(ResetSpeedAfterDuration(tunkController));
    }
    private IEnumerator ResetSpeedAfterDuration(TunkController tunkController)
    {
        yield return new WaitForSeconds(_duration);
        tunkController._moveSpeed = originalMoveSpeed;
    }
    private void ApplyInvulnerability(TunkController tunkController)
    {
        tunkController.IsInvulnerable = true;  // ���G��L���ɂ���
        StartCoroutine(DisableInvulnerabilityAfterTime(tunkController));
    }

    private IEnumerator DisableInvulnerabilityAfterTime(TunkController tankController)
    {
        yield return new WaitForSeconds(_duration);
        tankController.IsInvulnerable = false;  // ���G�𖳌��ɂ���
    }

    private IEnumerator DeactivateAndRespawnCoroutine()
    {
        sprite.enabled = false; // �A�C�e�����\���ɂ���
        sphereCollider.enabled = false; // �R���C�_�[�𖳌��ɂ���

        yield return new WaitForSeconds(_itemRespawnDuration); // �x�~���Ԃ�҂�

        sprite.enabled = true; // �A�C�e�����ĕ\������
        sphereCollider.enabled = true; // �R���C�_�[���ēx�L���ɂ���

        _taken = false; // �A�C�e�����������Ԃ����Z�b�g
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_taken) return;

        TunkController tank = other.GetComponentInParent<TunkController>();
        if (tank != null) Take(tank); // �A�C�e�����������X�L���𔭓�
    }
}
