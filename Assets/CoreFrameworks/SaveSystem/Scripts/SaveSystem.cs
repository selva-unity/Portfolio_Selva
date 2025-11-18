using UnityEngine;
using System;

public class SaveSystem : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotsParent;
    private int saveSlotCount = 0;

    private void Start() {
        LoadSlots();
    }

    public void OnClickNewSave()
    {
        saveSlotCount++;
        GameObject newSlot = Instantiate(slotPrefab, slotsParent);
        // Additional initialization for the new save slot can be added here
        newSlot.GetComponent<SlotHandler>().SetSlotInfo($"Save Slot {saveSlotCount}", DateTime.Now.ToString("g"));
    }

    private void LoadSlots()
    {
        // // For demonstration, we will create 3 dummy slots
        // for (int i = 1; i <= 3; i++)
        // {
        //     GameObject slot = Instantiate(slotPrefab, slotsParent);
        //     slot.GetComponent<SlotHandler>().SetSlotInfo($"Save Slot {i}", DateTime.Now.AddDays(-i).ToString("g"));
        // }
    }
}

[Serializable]
public class PlayerData
{
    public Vector3 position;
}


