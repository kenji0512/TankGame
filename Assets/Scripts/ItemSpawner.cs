using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; // �A�C�e���̃v���n�u
    public Vector2 xRange = new Vector2(-10, 10);
    public Vector2 zRange = new Vector2(-10, 10);
    public float spawnDelay = 5f; // �ŏ��̏o���܂ł̎���
    public float spawnInterval = 3f; // �o���Ԋu

    void Start()
    {
        StartCoroutine(StartSpawning());
    }
    IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(spawnDelay);//�ŏ��̒x��
        InvokeRepeating(nameof(SpawnItem), 0f, spawnInterval);//���Ԋu�ŃX�|�[��
    }
    void SpawnItem()
    {
        if (items.Length == 0) return;

        Vector3 spawnPosition = new Vector3(
            Random.Range(xRange.x, xRange.y),
            1.8f, // y���W�͓K�X�ύX
            Random.Range(zRange.x, zRange.y)
        );

        int itemIndex = Random.Range(0, items.Length);
        Instantiate(items[itemIndex], spawnPosition, Quaternion.identity);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // �ΐF�̘g��`��
        Vector3 center = new Vector3((xRange.x + xRange.y) / 2, 1f, (zRange.x + zRange.y) / 2);
        Vector3 size = new Vector3(xRange.y - xRange.x, 1f, zRange.y - zRange.x);

        Gizmos.DrawWireCube(center, size);
    }
}
