using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public static Sound manager;
    public AudioClip[] audioClips;
    private AudioSource source;

    private void Awake()
    {
        var obj = FindObjectsOfType<Sound>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            manager = gameObject.GetComponent<Sound>();
            source = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlaySound(int index)
    {
        source.clip = audioClips[index];
        source.Play();
    }
}
