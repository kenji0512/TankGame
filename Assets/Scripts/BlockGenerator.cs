using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public GameObject blockPrefab; // 生成するブロックのプレハブ
    public int rows = 5; // 行の数
    public int cols = 5; // 列の数
    public float spacing = 2.0f; // ブロック間の間隔

    void Start()
    {
        GenerateBlocks();
    }

    void GenerateBlocks()
    {
        float startX = -(cols - 1) * spacing / 2;
        float startZ = -(rows - 1) * spacing / 2;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 position = new Vector3(startX + col * spacing, 0, startZ + row * spacing);
                Instantiate(blockPrefab, position, Quaternion.identity);
            }
        }
    }
}
