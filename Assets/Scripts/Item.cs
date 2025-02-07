using NetcodePlus.Demo;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    public int _value;
    public float _duration;
    public float _respawnDuration = 10f;

    [Header("Ref")]
    public GameObject mesh;
    public ParticleSystem pickupEffect; // �擾���̃G�t�F�N�g�i��Œǉ��p�j


    private bool _taken = false;
    private float _timer = 0f;

    private void Update()
    {
        // Powerup�����ꂽ��̏���
        if (_taken)
        {
            _timer += Time.deltaTime;
            if (_timer > _respawnDuration)
                Respawn();
        }
    }

    public void Take(TunkController tunkController)
    {
        if (!_taken)
        {
            _taken = true;
            mesh.SetActive(false); // ���b�V�����\���ɂ��āA�A�C�e�������ꂽ���Ƃ�����
            ApplyBonus(tunkController);
        }
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
                    tunkController.IncreaseSpeed(_value);
                    StartCoroutine(ApplySpeedBoost(tunkController, _duration));
                    break;
                case PowerupType.Invulnerable:
                    tunkController.IsInvulnerable = true;
                    StartCoroutine(DisableInvulnerabilityAfterTime(tunkController, _duration));
                    break;
            }
        }
    }
    private IEnumerator ApplySpeedBoost(TunkController tankController, float duration)
    {
        float originalSpeed = tankController._moveSpeed;
        tankController._moveSpeed *= (1 + (_value / 100f)); // �X�s�[�h�A�b�v
        yield return new WaitForSeconds(duration);
        tankController.IncreaseSpeed(-_value); // ���̑��x�ɖ߂�
    }

    private IEnumerator DisableInvulnerabilityAfterTime(TunkController tankController, float duration)
    {
        yield return new WaitForSeconds(duration);
        tankController.IsInvulnerable = false;
    }
    public void Respawn()
    {
        _taken = false;
        mesh.SetActive(true); // �A�C�e�����ĕ\������
        _timer = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_taken)
            return;

        TunkController tank = other.GetComponentInParent<TunkController>();
        if (tank != null)
        {
            Take(tank); // �A�C�e�����������X�L���𔭓�
        }
    }
}
