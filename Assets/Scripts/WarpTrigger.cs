using UnityEngine;

public class WarpTrigger : MonoBehaviour
{
    public Transform[] warpDestinations; // 複数のワープ先を配列で設定

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤーがワープエリアに入ったとき
        {
            // ランダムにワープ先を選択
            int randomIndex = Random.Range(0, warpDestinations.Length);
            other.transform.position = warpDestinations[randomIndex].position;
        }
    }
}
