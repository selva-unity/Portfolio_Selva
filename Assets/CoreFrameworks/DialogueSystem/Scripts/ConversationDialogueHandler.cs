using TMPro;
using UnityEngine;

public class ConversationDialogueHandler : MonoBehaviour
{
    public TextMeshProUGUI speakerTxt;
    public TextMeshProUGUI dialogueTxt;
    
    public void SetConversationDialogue(string speaker, string dialogue)
    {
        speakerTxt.SetText(" ( " + speaker + " )  ");
        dialogueTxt.SetText(dialogue);
    }
}
