using UnityEngine;
using System;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotsParent;
    [SerializeField] private PlayerHandler playerHandler; 

    private int saveSlotCount = 0;
    private Vector3 lastPlayerPosition;

    public bool isSlotsLoaded = false;

    private void OnEnable()
    {
        if (playerHandler != null)
            playerHandler.OnPlayerPositionChanged.AddListener(OnPlayerPositionChanged);


    }

    private void OnDisable()
    {
        if (playerHandler != null)
            playerHandler.OnPlayerPositionChanged.RemoveListener(OnPlayerPositionChanged);

    }

    private void Start()
    {
        Debug.Log("SaveSystem started.");
       
        if (playerHandler != null)
            lastPlayerPosition = playerHandler.transform.position;
        if (!isSlotsLoaded)
            LoadSlots();
        
        //DeleteSlotsForTesting(); 
    }
    private void DeleteSlotsForTesting()
    {
        string folder = SaveManager.Instance.RootPath;
        if (Directory.Exists(folder))
        {
            string[] files = Directory.GetFiles(folder, "PlayerData_*.json");
            foreach (string file in files)
            {
                File.Delete(file);
                Debug.Log("Deleted save file: " + file);
            }
        }

    }

    private void OnPlayerPositionChanged(Vector3 pos)
    {
        lastPlayerPosition = pos;
    }

    public void OnClickNewSave()
    {
        saveSlotCount++;

        string key = $"slot_{saveSlotCount}";

        PlayerData data = new PlayerData { position = (playerHandler != null) ? playerHandler.transform.position : lastPlayerPosition };

        Debug.Log("Creating new save data at position: " + data.position);
        Debug.Log(data.position);

        SaveManager.Save(data, key);
        Debug.Log($"Saved new slot {key} at position {data.position}");

        GameObject slotGO = Instantiate(slotPrefab, slotsParent);
        var handler = slotGO.GetComponent<SlotHandler>();

        string dateString = DateTime.Now.ToString("g");
        handler.Init(this, key);
        handler.SetSlotInfo($"Save {key}", dateString);
    }

    private void LoadSlots()
    {
        string folder = SaveManager.Instance.RootPath;
        if (!Directory.Exists(folder))
            return;

        string[] files = Directory.GetFiles(folder, "PlayerData_*.json");

        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            const string prefix = "PlayerData_";

            if (!fileName.StartsWith(prefix))
                continue;

            string key = fileName.Substring(prefix.Length); 
            saveSlotCount++;

            GameObject slotGO = Instantiate(slotPrefab, slotsParent);
            var handler = slotGO.GetComponent<SlotHandler>();

            string dateString = File.GetLastWriteTime(file).ToString("g");
            Debug.Log("Found save file: " + fileName + " Key: " + key + " Date: " + dateString);

            handler.Init(this, key);
            handler.SetSlotInfo($"Save {key}", dateString);
        }
        isSlotsLoaded = true;
        Debug.Log("LoadSlots completed. Total slots loaded: " + saveSlotCount);
    }

    public void LoadSlot(string key)
    {
        if (SaveManager.TryLoad<PlayerData>(key, out var data))
        {
            Debug.Log($"Loaded save data for key {key}: Player position {data.position}");
            playerHandler.TeleportTo(data.position);
        }
        else
        {
            Debug.LogWarning($"No save data for key {key}");
        }
    }
}

[Serializable]
public class PlayerData
{
    public Vector3 position;
}


