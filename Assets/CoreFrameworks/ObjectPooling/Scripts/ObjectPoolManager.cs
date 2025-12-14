using System.Collections.Generic;
using UnityEngine;

namespace CoreFramework.ObjectPooling
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public GameObject objectToPool;
        public int amountToPool = 10;

        public static ObjectPoolManager Instance;

        private readonly Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

        private Transform poolRoot;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (poolRoot == null)
            {
                poolRoot = new GameObject("ObjectPools").transform;
                poolRoot.SetParent(transform, false);
            }
        }

        private void Start()
        {
            if (objectToPool != null && amountToPool > 0)
            {
                CreatePool(objectToPool, Mathf.Max(1, amountToPool));
            }
        }

        public void CreatePool(GameObject prefab, int initialSize)
        {
            if (prefab == null)
            {
                Debug.Log("ObjectPoolManager: CreatePool called with null prefab");
                return;
            }

            if (!pools.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<GameObject>(initialSize);
                pools[prefab] = queue;
            }

            // Pre-instantiate objects to reach initialSize
            for (int i = queue.Count; i < initialSize; i++)
            {
                var obj = InstantiateNew(prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
        }

        public GameObject GetObject(GameObject prefab)
        {
            if (prefab == null)
            {
                Debug.Log("ObjectPoolManager: GetObject called with null prefab");
                return null;
            }

            if (!pools.TryGetValue(prefab, out var queue))
            {
                // create a pool lazily
                CreatePool(prefab, 1);
                queue = pools[prefab];
            }

            GameObject obj = null;
            while (queue.Count > 0)
            {
                obj = queue.Dequeue();
                if (obj != null)
                {
                    break;
                }
            }

            if (obj == null)
            {
                obj = InstantiateNew(prefab);
            }

            obj.SetActive(true);
            return obj;
        }

        public void ReturnObject(GameObject instance)
        {
            if (instance == null) return;

            var member = instance.GetComponent<PoolMember>();
            if (member == null || member.prefab == null)
            {
                instance.SetActive(false);
                instance.transform.SetParent(poolRoot, false);
                return;
            }

            instance.SetActive(false);
            instance.transform.SetParent(poolRoot, false);

            if (!pools.TryGetValue(member.prefab, out var queue))
            {
                queue = new Queue<GameObject>();
                pools[member.prefab] = queue;
            }

            queue.Enqueue(instance);
        }

        public static GameObject Spawn(GameObject prefab) => Instance.GetObject(prefab);
        public static void Recycle(GameObject instance) => Instance.ReturnObject(instance);

        private GameObject InstantiateNew(GameObject prefab)
        {
            var go = Instantiate(prefab, poolRoot);

            if (!go.TryGetComponent<PoolMember>(out var member))
                member = go.AddComponent<PoolMember>();

            member.prefab = prefab;
            return go;
        }

        private class PoolMember : MonoBehaviour
        {
            public GameObject prefab;
        }
    }
}
