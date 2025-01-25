using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static List<Transform> players = new List<Transform>();

    private void Start()
    {
        // �V�[�����̑S�Ă�Player�^�O�����I�u�W�F�N�g�����X�g�ɒǉ�
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
