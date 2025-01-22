using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public GameObject tankBluePrefab;
    public GameObject tankRedPrefab;

    private int playerCount = 0;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // プレイヤー数の制限
        if (playerCount >= 2)
        {
            Debug.LogError("Too many players for split-screen configuration.");
            return;
        }

        playerCount++;

        // スプリットスクリーンの設定
        if (playerCount == 1)
        {
            playerInput.camera.rect = new Rect(0, 0, 0.5f, 1f); // 左側
            Debug.Log("Player 1 joined: " + playerInput.gameObject.name);
        }
        else if (playerCount == 2)
        {
            playerInput.camera.rect = new Rect(0.5f, 0, 0.5f, 1f); // 右側
            Debug.Log("Player 2 joined: " + playerInput.gameObject.name);
        }

        // プレイヤーPrefabの生成
        //GameObject prefab = playerCount == 1 ? tankBluePrefab : tankRedPrefab;
        //Instantiate(prefab, new Vector3(playerCount * 5, 0, 0), Quaternion.identity);
    }

}
