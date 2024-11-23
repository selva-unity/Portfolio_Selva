using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSO", menuName = "ScriptableObject/AudioSO", order = 1)]
public class AudioSO : ScriptableObject
{
   public AudioData[] audioData;
}

[System.Serializable]
public class AudioData
{
    public string name;
    public AudioClips audioClips;
    public AudioClip clip;
}

public enum AudioClips{
    None,
    BGMusic,
    AmbientMusic,
    Landing

}
