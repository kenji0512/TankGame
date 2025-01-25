using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static List<Transform> players = new List<Transform>();

    private void Start()
    {
        // シーン内の全てのPlayerタグを持つオブジェクトをリストに追加
        players.AddRange(GameObject.FindGameObjectsWithTag("Player")
            .Select(player => player.transform).ToList());
    }

    public static void UpdatePlayerList()
    {
        players.Clear();
        players.AddRange(GameObject.FindGameObjectsWithTag("Player")
            .Select(player => player.transform).ToList());
    }
}
