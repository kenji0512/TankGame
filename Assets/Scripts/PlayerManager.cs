using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public GameObject tankBluePrefab;
    public GameObject tankRedPrefab;

    private int playerCount = 0;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // �v���C���[���̐���
        if (playerCount >= 2)
        {
            Debug.LogError("Too many players for split-screen configuration.");
            return;
        }

        playerCount++;

        // �X�v���b�g�X�N���[���̐ݒ�
        if (playerCount == 1)
        {
            playerInput.camera.rect = new Rect(0, 0, 0.5f, 1f); // ����
            Debug.Log("Player 1 joined: " + playerInput.gameObject.name);
        }
        else if (playerCount == 2)
        {
            playerInput.camera.rect = new Rect(0.5f, 0, 0.5f, 1f); // �E��
            Debug.Log("Player 2 joined: " + playerInput.gameObject.name);
        }

        // �v���C���[Prefab�̐���
        //GameObject prefab = playerCount == 1 ? tankBluePrefab : tankRedPrefab;
        //Instantiate(prefab, new Vector3(playerCount * 5, 0, 0), Quaternion.identity);
    }

}
