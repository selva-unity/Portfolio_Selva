using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
namespace DialogueSystem
{
    public class DialogueChoice : MonoBehaviour
    {

        public TextMeshProUGUI speakerTxt;
        public TextMeshProUGUI dialogueTxt;
        public TextMeshProUGUI[] choicesTxt;
        public GameObject choicesParent;

        public int selectedChoice;

        Tween typeWriter;

        private StringBuilder dialogueSB;

        public IEnumerator SetDialogueChoice(DialogueData dialogueData, AudioSource audioSource)
        {
            dialogueSB = new StringBuilder();
            //dialogueTxt.SetText(dialogueData.dialogue);
            audioSource.clip = dialogueData.dialogueAudio;
            audioSource.Play();

            //   typeWriter = DOTween.To(() => dialogueSB.ToString(), x =>
            //     {
            //         dialogueSB.Clear();
            //         dialogueSB.Append(x);
            //     }, dialogueData.dialogue, audioSource.clip.length).OnUpdate(() =>
            //     {
            //         dialogueTxt.SetText(dialogueSB.ToString());
            //         //dialogueTxt3d.text = dialogueSB.ToString();
            //     });
            // string dialogue = " ( "+ dialogueData.dialogueSpeaker.ToString()  +" )  " + dialogueData.dialogue;
            if (dialogueData.dialogueSpeaker != DialogueSpeaker.Narrator)
            {
                speakerTxt.SetText(" ( " + dialogueData.dialogueSpeaker.ToString() + " )  ");
            }
            dialogueTxt.SetText(dialogueData.dialogue);

            yield return new WaitForSeconds(audioSource.clip.length);
            choicesParent.SetActive(true);

            EventSystem.current.SetSelectedGameObject(choicesTxt[0].GetComponentInParent<Button>().gameObject);

            for (int i = 0; i < dialogueData.dialogueChoiceDatas.Length; i++)
            {
                choicesTxt[i].SetText(dialogueData.dialogueChoiceDatas[i].dialogue);
            }
        }

        public void OnSelectChoice(int index) => selectedChoice = index;

    }

    [System.Serializable]
    public class DialogueData
    {
        public DialogueSpeaker dialogueSpeaker;
        public string dialogue;
        public AudioClip dialogueAudio;
        public DialogueChoiceData[] dialogueChoiceDatas;
    }

    [System.Serializable]
    public class DialogueChoiceData
    {
        public string dialogue;
        public AudioClip dialogueAudio;

    }
}
