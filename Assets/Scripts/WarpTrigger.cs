using UnityEngine;

public class WarpTrigger : MonoBehaviour
{
    public Transform[] _warpDestinations; // 複数のワープ先を配列で設定

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがワープエリアに入ったとき
        if (!other.CompareTag("Player")) return;
        {
            TunkController controller = other.GetComponentInParent<TunkController>();
            if (controller != null && controller.CanWarp)
            {
                // ランダムにワープ先を選択
                int randomIndex = Random.Range(0, _warpDestinations.Length);
                Vector3 warpPos = _warpDestinations[randomIndex].position;
                controller.transform.rotation = _warpDestinations[randomIndex].rotation;

                //地面の高さをレイキャストで取得
                if (Physics.Raycast(warpPos + Vector3.up * 30f, Vector3.down, out RaycastHit hit, 50f))
                {
                    warpPos.y = hit.point.y + 0.5f;
                }
                else
                {
                    warpPos.y = 1f;
                    Debug.LogWarning("地面が見つかりませんでした！ ワープ位置: " + warpPos);
                }
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.MovePosition(warpPos);
                }
                //else other.transform.position = warpPos;
                //other.transform.position = warpPos;

                controller.ResetMovement();
                controller.PreventWarpForSeconds(1.8f);
            }
        }
    }
}
