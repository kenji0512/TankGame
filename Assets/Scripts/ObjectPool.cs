using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}
public class ObjectPool : MonoBehaviour
{
    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    public GameObject Release(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        Queue<GameObject> objectPool = poolDictionary[tag];
        GameObject obj = null;

        while (objectPool.Count > 0)
        {
            obj = objectPool.Dequeue();
            if (obj != null) break; // �L���Ȃ�g��
        }
        if (obj == null)
        {
            Pool poolConfig = pools.Find(p => p.tag == tag);
            if (poolConfig == null) return null;
            obj = Instantiate(poolConfig.prefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        // --- �g���C�������Z�b�g���镔����ǉ� ---
        TrailRenderer trail = obj.GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.Clear(); // �c��������
        }
        obj.SetActive(true);

        Bullet bullet = obj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.myPool = this;
            bullet.poolTag = tag;
        }

        return obj;
    }//�e�����o���Ĕ��˂��邾��
    public void Catch(string tag, GameObject obj)
    {
        if (obj == null) return;

        if (poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Bullet had no pool, but tried to return: {gameObject.name}");
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning($"No pool found for tag: {tag}");
            Destroy(obj);
        }
    }//�e���g���I�������v�[���ɖ߂�
}
