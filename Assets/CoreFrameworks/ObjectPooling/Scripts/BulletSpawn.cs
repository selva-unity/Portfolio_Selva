using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public Transform spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectPoolManager.Instance.CreatePool(bulletPrefab, 20);
        ObjectPoolManager.Instance.CreatePool(grenadePrefab, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject bullet = ObjectPoolManager.Spawn(bulletPrefab);
            bullet.transform.position = spawnPoint.position;
            bullet.transform.rotation = spawnPoint.rotation;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject grenade = ObjectPoolManager.Spawn(grenadePrefab);
            grenade.transform.position = spawnPoint.position;
            grenade.transform.rotation = spawnPoint.rotation;
        }
    }
}
