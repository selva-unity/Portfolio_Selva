using TMPro;
using UnityEngine;

public class SlotHandler : MonoBehaviour
{
    public TextMeshProUGUI slotTxt;
    public TextMeshProUGUI timeTxt;


    private SaveSystem saveSystem;
    private string saveKey;
   public void Init(SaveSystem system, string key)
    {
        saveSystem = system;
        saveKey = key;
    }

    public void SetSlotInfo(string title, string date)
    {
        if (slotTxt != null) slotTxt.text = title;
        if (timeTxt != null)  timeTxt.text  = date;
    }

    // Hook this up to the Button's OnClick in the inspector
    public void OnClickSlot()
    {
        if (saveSystem != null && !string.IsNullOrEmpty(saveKey))
        {
            saveSystem.LoadSlot(saveKey);
        }
    }
}
