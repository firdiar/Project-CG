using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundDatabase
{
    public string soundName;
    public AudioClip clip;
}

public class SoundBase : MonoBehaviour
{
    public static SoundBase MAIN;
    [SerializeField] List<SoundDatabase> soundClips;

    public void Awake()
    {
        MAIN = this;
    }

    public AudioClip GetSoundClip(string name)
    {
        foreach (SoundDatabase sd in MAIN.soundClips)
        {
            if (name == sd.soundName)
            {
                return sd.clip;
            }
        }
        return null;
    }
}
