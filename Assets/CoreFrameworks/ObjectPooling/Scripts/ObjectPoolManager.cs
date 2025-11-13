using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central object pool manager. Use this from any script by calling
/// ObjectPoolManager.Instance.CreatePool(prefab, size);
/// ObjectPoolManager.Instance.GetObject(prefab);
/// ObjectPoolManager.Instance.ReturnObject(instance);
///
/// For convenience there are also static wrappers: ObjectPoolManager.Spawn(prefab) / ObjectPoolManager.Recycle(instance)
///
/// Backwards-compatible fields: if `objectToPool` is set in the inspector this manager will auto-create a single pool of size `amountToPool` on Start.
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    // Backwards compatibility: if you assign a prefab here, the manager will create a pool for it on Start.
    public GameObject objectToPool;
    public int amountToPool = 10;

    // Singleton instance
    public static ObjectPoolManager Instance;
   

    // Map each prefab to its pool (queue of GameObjects)
    private readonly Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    // A root transform in the scene where pooled objects are parented
    private Transform poolRoot;

    private void Awake()
    {
        // Enforce singleton (simple pattern: destroy duplicates)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // Optional: keep across scene loads
        DontDestroyOnLoad(gameObject);

        if (poolRoot == null)
        {
            poolRoot = new GameObject("ObjectPools").transform;
            poolRoot.SetParent(transform, false);
        }
    }

    private void Start()
    {
        // Backwards-compatible single pool creation
        if (objectToPool != null && amountToPool > 0)
        {
            CreatePool(objectToPool, Mathf.Max(1, amountToPool));
        }
    }

    /// <summary>
    /// Create a pool for the given prefab with an initial size.
    /// Calling again for the same prefab will increase the pool to at least the requested size.
    /// </summary>
    public void CreatePool(GameObject prefab, int initialSize)
    {
        if (prefab == null)
        {
            Debug.LogWarning("ObjectPoolManager: CreatePool called with null prefab");
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

    /// <summary>
    /// Get an object for the given prefab. If pool doesn't exist, it's created on-the-fly (size 1).
    /// Returned object will be active.
    /// </summary>
    public GameObject GetObject(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("ObjectPoolManager: GetObject called with null prefab");
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

    /// <summary>
    /// Return an instance to its pool. If the instance wasn't created by the pool manager we'll attempt to find a matching pool via PoolMember; otherwise the object is destroyed.
    /// </summary>
    public void ReturnObject(GameObject instance)
    {
        if (instance == null) return;

        var member = instance.GetComponent<PoolMember>();
        if (member == null || member.prefab == null)
        {
            // Not managed: destroy or simply deactivate and parent to poolRoot
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

    /// <summary>
    /// Convenience static wrappers
    /// </summary>
    public static GameObject Spawn(GameObject prefab) => Instance.GetObject(prefab);
    public static void Recycle(GameObject instance) => Instance.ReturnObject(instance);

    // Internal helper that instantiates a new instance and attaches a PoolMember for tracking the source prefab.
    private GameObject InstantiateNew(GameObject prefab)
    {
        var go = Instantiate(prefab, poolRoot);
        // add PoolMember so returned instances know their prefab
        var member = go.GetComponent<PoolMember>();
        if (member == null)
            member = go.AddComponent<PoolMember>();
        member.prefab = prefab;
        return go;
    }

    // Small component attached to pooled instances to remember their originating prefab.
    private class PoolMember : MonoBehaviour
    {
        public GameObject prefab;
    }
}
