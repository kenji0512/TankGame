using UnityEngine;

public class WarpTrigger : MonoBehaviour
{
    public Transform[] warpDestinations; // 複数のワープ先を配列で設定

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがワープエリアに入ったとき
        if (!other.CompareTag("Player")) return;
        {
            TunkController controller = other.GetComponent<TunkController>();
            if (controller != null && controller.CanWarp)
            {
                // ランダムにワープ先を選択
                int randomIndex = Random.Range(0, warpDestinations.Length);
                Vector3 warpPos = warpDestinations[randomIndex].position;

                //地面の高さをレイキャストで取得
                if (Physics.Raycast(warpPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 10f))
                {
                    // 地面の位置に補正
                    warpPos.y = hit.point.y;
                    Debug.Log("地面検出: " + hit.point);
                }
                else
                {
                    warpPos.y = 0;
                    Debug.LogWarning("地面が見つかりませんでした！ ワープ位置: " + warpPos);
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
