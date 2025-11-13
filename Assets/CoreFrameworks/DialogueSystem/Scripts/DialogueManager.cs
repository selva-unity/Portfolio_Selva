using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using DG.Tweening;
using System.Linq;
using System;
namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;
        [SerializeField] private TextMeshProUGUI speakerTxt;
        [SerializeField] private TextMeshProUGUI dialogueTxt;
        //[SerializeField] private TextMeshPro dialogueTxt3d;
        public ConversationSO convoSO;
        public ConversationSO narrationSO;

        private StringBuilder dialogueSB;
        [SerializeField] protected AudioSource audioSource;

        [SerializeField] private GameObject dialogueChoicePrefab;
        [SerializeField] private GameObject conversationDialoguePrefab;
        [SerializeField] private GameObject narrationDialoguePrefab;
        [SerializeField] private Transform dialogueParent;

        Tween typeWriter;

        public Coroutine dialogueCoroutine;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            DontDestroyOnLoad(this);

            dialogueSB = new StringBuilder();
        }

        private void Start()
        {
            //StartCoroutine(ShowDialogue(0));
        }

        public void ShowDialogue(ConversationSO sO, SceneNarration sceneNarration, DialogueType dialogueType, bool isVoiceOnly, Action callback)
        {
            dialogueCoroutine = StartCoroutine(ShowDialogueCoroutine(sO, sceneNarration, dialogueType, isVoiceOnly, callback));
        }

        private IEnumerator ShowDialogueCoroutine(ConversationSO sO, SceneNarration sceneNarration,  DialogueType dialogueType, bool isVoiceOnly, Action callback)
        {
            var narration = sO.convoScenes.Where(x => x.narration == sceneNarration).FirstOrDefault();

            for (int i = 0; i < narration.dialogues.Length; i++)
            {
                yield return new WaitForSeconds(narration.dialogues[i].responseTime);


                if (narration.dialogues[i].dialogueType == DialogueType.ResponseType)
                {
                    dialogueSB.Clear();
                    //dialogueTxt.SetText("");

                    DialogueData dialogueData = new DialogueData();

                    dialogueData.dialogue = narration.dialogues[i].dialogue;
                    dialogueData.dialogueAudio = narration.dialogues[i].dialogueAudio;
                    dialogueData.dialogueSpeaker = narration.dialogues[i].dialogueSpeaker;

                    dialogueData.dialogueChoiceDatas = new DialogueChoiceData[narration.dialogues[i].choices.Length];

                    dialogueData.dialogueChoiceDatas = new DialogueChoiceData[]
                    {
                    new() { dialogue = narration.dialogues[i].choices[0].dialogue, dialogueAudio = narration.dialogues[i].choices[0].dialogueAudio },
                    new() { dialogue = narration.dialogues[i].choices[1].dialogue, dialogueAudio = narration.dialogues[i].choices[1].dialogueAudio },
                    new() { dialogue = narration.dialogues[i].choices[2].dialogue, dialogueAudio = narration.dialogues[i].choices[2].dialogueAudio }
                    };

                    var dialogueChoice = Instantiate(dialogueChoicePrefab, dialogueParent);
                    StartCoroutine(dialogueChoice.GetComponent<DialogueChoice>().SetDialogueChoice(dialogueData, audioSource));

                    yield return new WaitUntil(() => Input.anyKey);

                    audioSource.clip = narration.dialogues[i].choices[dialogueChoice.GetComponent<DialogueChoice>().selectedChoice].dialogueAudio;
                    audioSource.Play();

                    if (!isVoiceOnly)
                    {
                        if (narration.dialogues[i].narrationEffect == NarrationEffect.TypeWriter)
                        {
                            typeWriter = DOTween.To(() => dialogueSB.ToString(), x =>
                            {
                                dialogueSB.Clear();
                                dialogueSB.Append(x);
                            }, narration.dialogues[i].choices[dialogueChoice.GetComponent<DialogueChoice>().selectedChoice].dialogue, audioSource.clip.length).OnUpdate(() =>
                            {
                                //dialogueTxt.SetText(dialogueSB.ToString());
                            });
                        }
                        else
                        {
                            string dialogue = " ( " + narration.dialogues[i].choices[dialogueChoice.GetComponent<DialogueChoice>().selectedChoice].dialogueSpeaker.ToString() + " )  " + narration.dialogues[i].choices[dialogueChoice.GetComponent<DialogueChoice>().selectedChoice].dialogue;
                            //dialogueTxt.SetText(dialogue);
                            //dialogueTxt.SetText();
                        }
                    }



                    Destroy(dialogueChoice);

                    yield return new WaitForSeconds(audioSource.clip.length);



                }
                else if (narration.dialogues[i].dialogueType == DialogueType.ConversationType)
                {
                    dialogueSB.Clear();
                    //dialogueTxt.SetText("");
                    

                    audioSource.clip = narration.dialogues[i].dialogueAudio;
                    audioSource.Play();

                    ConversationDialogueHandler conversationDialogueComponent = null;




                    if (!isVoiceOnly)
                    {
                        var conversationDialogue = Instantiate(conversationDialoguePrefab, dialogueParent);
                        conversationDialogueComponent = conversationDialogue.GetComponent<ConversationDialogueHandler>();
                        conversationDialogueComponent.SetConversationDialogue(string.Empty, string.Empty);

                        if (narration.dialogues[i].narrationEffect == NarrationEffect.TypeWriter)
                        {
                            typeWriter = DOTween.To(() => dialogueSB.ToString(), x =>
                            {
                                dialogueSB.Clear();
                                dialogueSB.Append(x);
                            }, narration.dialogues[i].dialogue, audioSource.clip.length).OnUpdate(() =>
                            {
                                //dialogueTxt.SetText(dialogueSB.ToString());
                                conversationDialogueComponent.SetConversationDialogue(narration.dialogues[i].dialogueSpeaker.ToString(), dialogueSB.ToString());
                            });
                        }
                        else
                        {
                            conversationDialogueComponent.SetConversationDialogue(narration.dialogues[i].dialogueSpeaker.ToString(), narration.dialogues[i].dialogue);
                            // if (narration.dialogues[i].dialogueSpeaker != DialogueSpeaker.Narrator)
                            // {
                            //     speakerTxt.SetText(" ( " + narration.dialogues[i].dialogueSpeaker.ToString() + " )  ");
                            // }
                            // //string dialogue = " ( "+ narration.dialogues[i].dialogueSpeaker.ToString()  +" )  " + narration.dialogues[i].dialogue;

                            // dialogueTxt.SetText(narration.dialogues[i].dialogue);

                        }

                    }



                    yield return new WaitForSeconds(audioSource.clip.length);
                    conversationDialogueComponent.SetConversationDialogue(string.Empty, string.Empty);  

                }
                else if(narration.dialogues[i].dialogueType == DialogueType.NarrationType)
                {
                    dialogueSB.Clear();
                    //dialogueTxt.SetText("");

                    audioSource.clip = narration.dialogues[i].dialogueAudio;
                    audioSource.Play();

                    NarrationDialogueHandler narrationDialogueComponent = null;

                    if (!isVoiceOnly)
                    {
                        var narrationDialogue = Instantiate(narrationDialoguePrefab, dialogueParent);
                        narrationDialogueComponent = narrationDialogue.GetComponent<NarrationDialogueHandler>();
                        narrationDialogueComponent.SetNarrationDialogue(string.Empty);
                        
                        if (narration.dialogues[i].narrationEffect == NarrationEffect.TypeWriter)
                        {
                            typeWriter = DOTween.To(() => dialogueSB.ToString(), x =>
                            {
                                dialogueSB.Clear();
                                dialogueSB.Append(x);
                            }, narration.dialogues[i].dialogue, audioSource.clip.length).OnUpdate(() =>
                            {
                                //dialogueTxt.SetText(dialogueSB.ToString());
                                narrationDialogueComponent.SetNarrationDialogue(dialogueSB.ToString());
                            });
                        }
                        else
                        {
                            narrationDialogueComponent.SetNarrationDialogue(narration.dialogues[i].dialogue);
                            // if (narration.dialogues[i].dialogueSpeaker != DialogueSpeaker.Narrator)
                            // {
                            //     speakerTxt.SetText(" ( " + narration.dialogues[i].dialogueSpeaker.ToString() + " )  ");
                            // }
                            // //string dialogue = " ( "+ narration.dialogues[i].dialogueSpeaker.ToString()  +" )  " + narration.dialogues[i].dialogue;

                            // dialogueTxt.SetText(narration.dialogues[i].dialogue);

                        }

                    }

                    yield return new WaitForSeconds(audioSource.clip.length);
                    narrationDialogueComponent.SetNarrationDialogue(string.Empty);
                    

                }


            }

            callback.Invoke();
            //dialogueTxt.SetText("");
            //speakerTxt.SetText("");

        }
    }
}

/*
import numpy as np
text_prompt = """
    Your mission begins here. Locate the Matsya 6000 on the launch platform.
"""
audio_array = generate_audio(text_prompt, history_prompt="v2/en_speaker_5")
audio_array = np.concatenate((np.array([0.5] * 1000), audio_array))
Audio(audio_array, rate=SAMPLE_RATE)
*/
