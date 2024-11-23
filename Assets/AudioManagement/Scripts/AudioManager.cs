using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject objectToPool;
    public int amountToPool = 10;

    private List<GameObject> pooledObjects;

    [SerializeField] private AudioSO audioSO;

    public static AudioManager Instance { get; private set;}
    private void Awake() 
    {
        if(Instance == null){
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    private GameObject GetPooledObject()
    {
        foreach (var obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null; // No available objects in the pool
    }

    public void PlayAudio(AudioClips audioClip, bool isLoop, float volume = 1f)
    {
        var audioSource = GetPooledObject().GetComponent<AudioSource>();

        var audioData = audioSO.audioData.Where(x => x.audioClips == audioClip).FirstOrDefault();

        audioSource.clip = audioData.clip;
        audioSource.Play();
        audioSource.loop = isLoop;
        audioSource.volume = volume;
    }
    public void StopAudio(AudioClips audioClip)
    {
        // var audioData = pooledObjects.Where(x => x.GetComponent<AudioSource>().clip == ).FirstOrDefault();
        var audioData = audioSO.audioData.Where(x => x.audioClips == audioClip).FirstOrDefault();

        var audioObject = pooledObjects.Where(x => x.GetComponent<AudioSource>().clip == audioData.clip).FirstOrDefault();


    }

    public void StopAllAudio()
    {
        foreach (var obj in pooledObjects) obj.SetActive(false);
    }

    public void PlaySFX(AudioClips audioClip, float volume = 1f)
    {
        var audioSource = GetPooledObject().GetComponent<AudioSource>();

        var audioData = audioSO.audioData.Where(x => x.audioClips == audioClip).FirstOrDefault();
        audioSource.clip = audioData.clip;

        StartCoroutine(PlaySFX(audioSource, volume));
    }

    private IEnumerator PlaySFX(AudioSource audio, float volume = 1f)
    {
        audio.Play();
        audio.loop = false;
        audio.volume = volume;
        yield return new WaitForSeconds(audio.clip.length);
        Debug.Log("Audio length");
        audio.gameObject.SetActive(false);
    }

}
