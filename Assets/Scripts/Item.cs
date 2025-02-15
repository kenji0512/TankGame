using NetcodePlus.Demo;
using System.Collections;
using UnityEngine;

public enum PowerupType
{
    None = 0,
    Heal = 10,
    Speed = 20, // value in percentage
    Invulnerable = 30
}

/// <summary>
/// Powerups that can be taken by tanks
/// </summary>
public class Item : MonoBehaviour
{
    public PowerupType type;
    public int _value = 10;
    public float _duration = 3f;
    public float _respawnDuration = 10f;

    [Header("Ref")]
    public SpriteRenderer sprite;
    public SphereCollider sphereCollider;
    public ParticleSystem pickupEffect; // �擾���̃G�t�F�N�g�i��Œǉ��p�j

    private bool _taken = false;
    //private float _timer = 0f;
    private float originalMoveSpeed; // ���̈ړ����x��ێ����邽�߂̕ϐ�

    private void Update()
    {
        //// Powerup�����ꂽ��̏���
        //if (_taken && _timer > _respawnDuration)
        //    StartCoroutine(DeactivateAndRespawnCoroutine());

        //_timer += Time.deltaTime;
        if (!_taken) return;

        // �A�C�e�������ꂽ��ɍďo������R���[�`�����J�n
        StartCoroutine(DeactivateAndRespawnCoroutine());
        _taken = false;  // 1�x�������s�����悤�Ƀt���O�����Z�b�g
    }

    public void Take(TunkController tunkController)
    {
        if (!_taken)
        {
            _taken = true;
            Debug.Log($"Powerup Type: {type}"); // ������type���m�F
            ApplyBonus(tunkController); // ��Ɍ��ʂ�K�p
            StartCoroutine(DeactivateAndRespawnCoroutine());
        }

        // �X�s�[�h�A�b�v�̌��ʂ��K�p����鎞�Ԃ�ݒ�
        //if (type == PowerupType.Speed)
        //{
        //    _duration = 3f;  // �K�v�ɉ����Đݒ�
        //}
    }

    private void ApplyBonus(TunkController tunkController)
    {
        if (tunkController != null)
        {
            switch (type)
            {
                case PowerupType.Heal:
                    tunkController.AddHealth(_value);
                    break;
                case PowerupType.Speed:
                    //originalMoveSpeed = tunkController._moveSpeed; // ���̃X�s�[�h��ۑ�
                    //tunkController.IncreaseSpeed(_value);
                    //StartCoroutine(ApplySpeedBoost(tunkController, _duration));
                    ApplySpeedBoost(tunkController);
                    break;
                case PowerupType.Invulnerable:
                    tunkController.IsInvulnerable = true;
                    StartCoroutine(DisableInvulnerabilityAfterTime(tunkController, _duration));
                    break;
            }
        }
    }
    private void ApplySpeedBoost(TunkController tunkController)
    {
        originalMoveSpeed = tunkController._moveSpeed; // ���̃X�s�[�h��ۑ�
        tunkController._moveSpeed *= 2.5f;
        Debug.Log($"Speed Up Applied: {tunkController._moveSpeed}");
        StartCoroutine(ResetSpeedAfterDuration(tunkController));
    }
    private IEnumerator ResetSpeedAfterDuration(TunkController tunkController)
    {
        yield return new WaitForSeconds(_duration);
        tunkController._moveSpeed = originalMoveSpeed;
        Debug.Log($"Speed Reset: {tunkController._moveSpeed}");
    }

    private IEnumerator DisableInvulnerabilityAfterTime(TunkController tankController, float _duration)
    {
        yield return new WaitForSeconds(_duration);
        tankController.IsInvulnerable = false;
        Debug.Log("Invulnerability ended.");
    }

    private IEnumerator DeactivateAndRespawnCoroutine()
    {
        sprite.enabled = false; // �A�C�e�����\���ɂ���
        sphereCollider.enabled = false; // �R���C�_�[�𖳌��ɂ���

        Debug.Log("Powerup deactivated, waiting to respawn...");

        yield return new WaitForSeconds(_duration); // �x�~���Ԃ�҂�

        sprite.enabled = true; // �A�C�e�����ĕ\������
        sphereCollider.enabled = true; // �R���C�_�[���ēx�L���ɂ���

        _taken = false; // �A�C�e�����������Ԃ����Z�b�g
        //_timer = 0f;
        Debug.Log("Powerup respawned.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_taken) return;

        TunkController tank = other.GetComponentInParent<TunkController>();
        if (tank != null) Take(tank); // �A�C�e�����������X�L���𔭓�
    }
}
