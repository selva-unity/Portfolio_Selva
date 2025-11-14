using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Return the bullet to the pool instead of destroying it
            ObjectPoolManager.Recycle(gameObject);
        }
    }
}
