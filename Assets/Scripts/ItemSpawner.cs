using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; // アイテムのプレハブ
    public Vector2 xRange = new Vector2(-10, 10);
    public Vector2 zRange = new Vector2(-10, 10);
    public float spawnDelay = 5f; // 最初の出現までの時間
    public float spawnInterval = 3f; // 出現間隔

    void Start()
    {
        StartCoroutine(StartSpawning());
    }
    IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(spawnDelay);//最初の遅延
        InvokeRepeating(nameof(SpawnItem), 0f, spawnInterval);//一定間隔でスポーン
    }
    void SpawnItem()
    {
        if (items.Length == 0) return;

        Vector3 spawnPosition = new Vector3(
            Random.Range(xRange.x, xRange.y),
            1.8f, // y座標は適宜変更
            Random.Range(zRange.x, zRange.y)
        );

        int itemIndex = Random.Range(0, items.Length);
        Instantiate(items[itemIndex], spawnPosition, Quaternion.identity);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // 緑色の枠を描画
        Vector3 center = new Vector3((xRange.x + xRange.y) / 2, 1f, (zRange.x + zRange.y) / 2);
        Vector3 size = new Vector3(xRange.y - xRange.x, 1f, zRange.y - zRange.x);

        Gizmos.DrawWireCube(center, size);
    }
}
