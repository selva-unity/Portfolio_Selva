using TMPro;
using UnityEngine;

public class SlotHandler : MonoBehaviour
{
    public TextMeshProUGUI slotTxt;
    public TextMeshProUGUI timeTxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSlotInfo(string slotName, string timeStamp)
    {
        slotTxt.text = slotName;
        timeTxt.text = timeStamp;
    }
}
