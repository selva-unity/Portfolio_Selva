using CoreFramework.ObjectPooling;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectPoolManager.Instance.CreatePool(playerPrefab, 10);
       
    }

   
}
