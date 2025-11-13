using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayAudio(AudioClips.BGMusic, isLoop: true, volume:0.2f);      //Play Background Music
        AudioManager.Instance.PlayAudio(AudioClips.AmbientMusic, isLoop: true, volume:0.2f); //Play Ambient Music
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
