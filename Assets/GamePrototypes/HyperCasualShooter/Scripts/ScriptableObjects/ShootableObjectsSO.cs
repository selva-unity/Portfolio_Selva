using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootableObjectsSO", menuName = "ScriptableObjects/ShootableObjectsSO", order = 1)]
public class ShootableObjectsSO : ScriptableObject
{
     
}
[Serializable]
public class ShootableObject
{
    public string Name;
    public GameObject Prefab;
    public float Health;
    public int ScoreValue;
}
