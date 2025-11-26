using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Multiplier"))
        {
            Debug.Log("Player hit a Multiplier!");
            // Handle multiplier logic here
        }
    }
}
