using Cysharp.Threading.Tasks;
using System;
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
    private float _originalMoveSpeed; // ���̈ړ����x��ێ����邽�߂̕ϐ�
    private float _originalDamage;
    private ParticleSystem currentEffect; // ���݂̌��ʂ̃G�t�F�N�g


    public void Take(TunkController tunkController)
    {
        if (!_taken)
        {
            _taken = true;
            ApplyBonus(tunkController); // ��Ɍ��ʂ�K�p
            DeactivateAndRespawnCoroutine().Forget();
        }
    }

    private void ApplyBonus(TunkController tunkController)
    {
        if (tunkController != null)
        {
            if (pickupEffect != null)
            {
                currentEffect = Instantiate(pickupEffect, tunkController.transform.position + Vector3.zero, Quaternion.identity);
                currentEffect.transform.SetParent(tunkController.transform); // ���ʃG�t�F�N�g���v���C���[�ɒǏ]����悤�ɐݒ�

            }

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

    private async UniTaskVoid ApplyPowerBoost(TunkController tunkController)
    {
        Debug.Log($"{tunkController.name} �Ƀp���[�A�b�v�K�p");
        tunkController.onPowerUp = true;
        _originalDamage = tunkController._damageAmount;
        tunkController._damageAmount += _value;

        await UniTask.Delay(TimeSpan.FromSeconds(_duration));

        tunkController._damageAmount = _originalDamage;
        tunkController.onPowerUp = false;
        currentEffect.Stop();
        Destroy(currentEffect);
        Debug.Log($"{currentEffect} destroy");
    }

    private async UniTaskVoid ApplySpeedBoost(TunkController tunkController)
    {
        if (tunkController.onSpeedUp) return;
        tunkController.onSpeedUp = true;
        _originalMoveSpeed = tunkController._moveSpeed; // ���̃X�s�[�h��ۑ�
        tunkController._moveSpeed *= (1f + (_value / 100f));

        await UniTask.Delay(TimeSpan.FromSeconds(_duration));
        tunkController._moveSpeed = tunkController._defaultMoveSpeed;
        tunkController.onSpeedUp = false;
        currentEffect.Stop(); // �G�t�F�N�g���~
        Destroy(currentEffect);
        Debug.Log(currentEffect + $"destroy");
    }
    private async UniTaskVoid ApplyInvulnerability(TunkController tunkController)
    {
        tunkController.IsInvulnerable = true;  // ���G��L���ɂ���

        await UniTask.Delay(TimeSpan.FromSeconds(_duration));

        tunkController.IsInvulnerable = false;  // ���G�𖳌��ɂ���
        currentEffect.Stop(); // �G�t�F�N�g���~
        Destroy(currentEffect);
        Debug.Log(currentEffect + $"destroy");
    }

    private async UniTaskVoid DeactivateAndRespawnCoroutine()
    {
        sprite.enabled = false; // �A�C�e�����\���ɂ���
        sphereCollider.enabled = false; // �R���C�_�[�𖳌��ɂ���

        await UniTask.Delay(TimeSpan.FromSeconds(_itemRespawnDuration));// �x�~���Ԃ�҂�

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
