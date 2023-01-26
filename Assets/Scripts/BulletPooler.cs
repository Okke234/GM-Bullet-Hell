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

        foreach (var pool in pools)
        {
            var bulletPool = new Queue<GameObject>();

            for (var i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                bulletPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, bulletPool);
        }


        // Do this better later down the line.
        tagToSpriteDictionary.Add("test", DataManager.Instance.bulletSprites[0]);
    }

    public GameObject SpawnFromPool(string bulletTag, Vector2 position, Quaternion rotation, Vector2 direction)
    {
        if (!poolDictionary.ContainsKey(bulletTag))
        {
            return null;
        }

        var objectToSpawn = poolDictionary[bulletTag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.GetComponent<Bullet>().direction = direction;

        poolDictionary[bulletTag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
