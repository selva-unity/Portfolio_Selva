using UnityEngine;
using DialogueSystem;

public class DialogueSystemController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueManager.instance.ShowDialogue(DialogueManager.instance.narrationSO, SceneNarration.Inroduction, DialogueType.NarrationType, false, () => {
            Debug.Log("Dialogue Finished inside");
        });
        Debug.Log("Dialogue Finished outside");
        // DialogueManager.instance.ShowDialogue(DialogueManager.instance.convoSO, SceneNarration.ResponseConversation, DialogueType.ConversationType, false, () => {
        //     Debug.Log("Dialogue Finished");
        // });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
