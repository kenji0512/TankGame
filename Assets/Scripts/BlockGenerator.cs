using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public GameObject blockPrefab; // ��������u���b�N�̃v���n�u
    public int rows = 5; // �s�̐�
    public int cols = 5; // ��̐�
    public float spacing = 2.0f; // �u���b�N�Ԃ̊Ԋu

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
