using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public Dictionary<string, Sprite> tagToSpriteDictionary = new Dictionary<string, Sprite>();

    #region Singleton

    private static BulletPooler _instance;
    public static BulletPooler Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    private void Start()
    {


        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> bulletPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                bulletPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, bulletPool);
        }


        // Do this better later down the line.
        tagToSpriteDictionary.Add("test", DataManager.Instance.bulletSprites[0]);
    }

    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation, Vector2 direction)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.GetComponent<Bullet>().direction = direction;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
