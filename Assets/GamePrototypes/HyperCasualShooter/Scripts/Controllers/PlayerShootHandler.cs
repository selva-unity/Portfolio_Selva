using CoreFramework.ObjectPooling;
using UnityEngine;

public class PlayerShootHandler : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootInterval = 0.5f;
    private float shootTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectPoolManager.Instance.CreatePool(bulletPrefab, 20);
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
        
    }

    private void Shoot()
    {
        GameObject bullet = ObjectPoolManager.Spawn(bulletPrefab);
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.localRotation;
    }
}
