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
    public GameObject Release(string tag, Vector3 position,Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        GameObject obj;
        var objectPool = poolDictionary[tag];

        if (objectPool.Count == 0)
        {
            Pool poolConfing = pools.Find(p => p.tag == tag);
            if (poolConfing == null) return null;

            obj = Instantiate(poolConfing.prefab);
        }
        else
        {
            obj = objectPool.Dequeue();
        }
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        Bullet bullet = obj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.myPool = this; 
        }
        poolDictionary[tag].Enqueue(obj);
        return obj;
    }
    public void Catch(string tag, GameObject obj)
    {
        if (obj == null) return; // nullチェックを必ず！

        if (poolDictionary.ContainsKey(tag))
        {
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
            return;
        }else
        {
            Debug.LogWarning($"No pool found for tag: {tag}");

        }
    }
}
