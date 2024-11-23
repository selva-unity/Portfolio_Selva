using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public int probability;
    public float timeDelay;
    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    private void Start() 
    {
        timeDelay += audioSource.clip.length;
        InvokeRepeating(nameof(ProbabilityCheck), timeDelay, timeDelay);
    }
    
    private void ProbabilityCheck()
    {
        int ranNum = Random.Range(1, 100);
       // Debug.Log("RanNum " + ranNum);

        if(ranNum <= probability)
        {
            audioSource.Play();
        }
    }
}
