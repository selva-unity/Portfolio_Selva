using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
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
            GameObject bullet = ObjectPoolManager.Instance.GetObject(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject grenade = ObjectPoolManager.Instance.GetObject(grenadePrefab);
            grenade.transform.position = transform.position;
            grenade.transform.rotation = transform.rotation;
            grenade.SetActive(true);
        }
    }
}
