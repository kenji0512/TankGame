using UnityEngine;

public class WarpTrigger : MonoBehaviour
{
    public Transform[] warpDestinations; // �����̃��[�v���z��Őݒ�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �v���C���[�����[�v�G���A�ɓ������Ƃ�
        {
            // �����_���Ƀ��[�v���I��
            int randomIndex = Random.Range(0, warpDestinations.Length);
            other.transform.position = warpDestinations[randomIndex].position;
        }
    }
}
