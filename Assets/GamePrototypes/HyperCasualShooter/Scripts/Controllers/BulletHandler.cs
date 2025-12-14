using CoreFramework.ObjectPooling;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public float lifeTime = 3f;
    private float timer = 0f;
    public float speed = 20f;
    void OnEnable()
    {
        timer = 0f;
    }
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            // Return the bullet to the pool instead of destroying it
            ObjectPoolManager.Recycle(gameObject);
        }
    }
}
