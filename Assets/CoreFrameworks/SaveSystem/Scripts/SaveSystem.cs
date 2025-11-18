using UnityEngine;
using System;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotsParent;
    [SerializeField] private PlayerHandler playerHandler; // drag in inspector

    private int saveSlotCount = 0;
    public Vector3 lastPlayerPosition;

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
        LoadSlots();   // <--- called at startup
    }

    private void OnPlayerPositionChanged(Vector3 pos)
    {
        lastPlayerPosition = pos;
        Debug.Log("Player position updated to: " + lastPlayerPosition);
    }

    public void OnClickNewSave()
    {
        saveSlotCount++;

        // build save key like "slot_1", "slot_2" ...
        string key = $"slot_{saveSlotCount}";

        // 1) create save data
        PlayerData data = new PlayerData();

        data.position = lastPlayerPosition;

        Debug.Log("Creating new save data at position: " + lastPlayerPosition);
        Debug.Log(data.position);

        // 2) save to disk
        SaveManager.Save(data, key);
        Debug.Log($"Saved new slot {key} at position {data.position}");

        // 3) create UI slot
        GameObject slotGO = Instantiate(slotPrefab, slotsParent);
        var handler = slotGO.GetComponent<SlotHandler>();

        string dateString = DateTime.Now.ToString("g");
        handler.Init(this, key);
        handler.SetSlotInfo($"Save {key}", dateString);
    }

    private void LoadSlots()
    {
        // get folder where SaveManager stores files
        string folder = SaveManager.Instance.RootPath;
        if (!Directory.Exists(folder))
            return;

        // pattern used by SaveManager: "PlayerData_<key>.json"
        string[] files = Directory.GetFiles(folder, "PlayerData_*.json");

        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file); // e.g. "PlayerData_slot_1"
            const string prefix = "PlayerData_";

            if (!fileName.StartsWith(prefix))
                continue;

            string key = fileName.Substring(prefix.Length); // e.g. "slot_1"
            saveSlotCount++;

            GameObject slotGO = Instantiate(slotPrefab, slotsParent);
            var handler = slotGO.GetComponent<SlotHandler>();

            // use file write time as "last played" date
            string dateString = File.GetLastWriteTime(file).ToString("g");
            Debug.Log("Found save file: " + fileName + " Key: " + key + " Date: " + dateString);

            handler.Init(this, key);
            handler.SetSlotInfo($"Save {key}", dateString);
        }
    }

    // called by SlotHandler when you click a slot
    public void LoadSlot(string key)
    {
        if (SaveManager.TryLoad<PlayerData>(key, out var data))
        {
            Debug.Log($"Loaded save data for key {key}: Player position {data.position}");
            // move player to saved position
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


