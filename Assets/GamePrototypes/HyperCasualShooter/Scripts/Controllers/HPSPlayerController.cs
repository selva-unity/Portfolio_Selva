using UnityEngine;

public class HPSPlayerController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Multiplier"))
        {
            Debug.Log("Multiplier");
        }
    }
}
