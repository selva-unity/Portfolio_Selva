using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ConversationSO", menuName = "ScriptableObject/ConversationSO", order = 1)]
public class ConversationSO : ScriptableObject
{
    public ConvoScene[] convoScenes;
}

[System.Serializable]
public class ConvoScene
{
    public string name;
    public SceneNarration narration;
    public Dialogue[] dialogues;

}

[System.Serializable]
public class Dialogue
{
    public float responseTime = 1f;
    public DialogueSpeaker dialogueSpeaker;
    public string dialogue;
    public AudioClip dialogueAudio;
    public DialogueType dialogueType;
    public NarrationEffect narrationEffect;
    public Choices[] choices;
}

[System.Serializable]
public class Choices
{
    public DialogueSpeaker dialogueSpeaker;
    public string dialogue;
    public AudioClip dialogueAudio;
    //public UnityEvent onSelectChoice;
}

public enum SceneNarration
{
    Inroduction,
    ResponseConversation

}
public enum DialogueType
{
    NarrationType,
    ConversationType,
    ResponseType

}

public enum DialogueSpeaker
{
    Narrator
}

public enum NarrationEffect
{
    Default,
    TypeWriter
}

