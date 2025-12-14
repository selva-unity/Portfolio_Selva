using TMPro;
using UnityEngine;

public class NarrationDialogueHandler : MonoBehaviour
{
    public TextMeshProUGUI narrationTxt;
    
    public void SetNarrationDialogue(string dialogue)
    {
        narrationTxt.SetText(dialogue);
    }
}
