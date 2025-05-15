using UnityEngine;

public class WarpTrigger : MonoBehaviour
{
    public Transform[] warpDestinations; // �����̃��[�v���z��Őݒ�

    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[�����[�v�G���A�ɓ������Ƃ�
        if (!other.CompareTag("Player")) return;
        {
            TunkController controller = other.GetComponent<TunkController>();
            if (controller != null && controller.CanWarp)
            {
                // �����_���Ƀ��[�v���I��
                int randomIndex = Random.Range(0, warpDestinations.Length);
                Vector3 warpPos = warpDestinations[randomIndex].position;

                //�n�ʂ̍��������C�L���X�g�Ŏ擾
                if (Physics.Raycast(warpPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 10f))
                {
                    // �n�ʂ̈ʒu�ɕ␳
                    warpPos.y = hit.point.y;
                    Debug.Log("�n�ʌ��o: " + hit.point);
                }
                else
                {
                    warpPos.y = 0;
                    Debug.LogWarning("�n�ʂ�������܂���ł����I ���[�v�ʒu: " + warpPos);
                }
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.position = warpPos;
                    rb.angularVelocity = Vector3.zero;
                }
                else other.transform.position = warpPos;
                other.transform.position = warpPos;

                controller.PreventWarpForSeconds(1.8f);
            }
        }
    }
}
