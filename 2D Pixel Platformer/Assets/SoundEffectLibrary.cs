using UnityEngine;
using System.Collections.Generic;

public class SoundEffectLibrary: MonoBehaviour
{
    [System.Serializable]
    public class SoundEffectGroup
    {
        public string name;
        public List<AudioClip> audioClips;
    }

    [SerializeField] private SoundEffectGroup[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();

        foreach (SoundEffectGroup soundEffectGroup in soundEffectGroups)
        {
            if (!soundDictionary.ContainsKey(soundEffectGroup.name))
            {
                soundDictionary[soundEffectGroup.name] = soundEffectGroup.audioClips;
            }
            else
            {
                Debug.LogWarning($"Duplicate sound group detected: {soundEffectGroup.name}");
            }
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (soundDictionary.TryGetValue(name, out List<AudioClip> audioClips) && audioClips.Count > 0)
        {
            return audioClips[Random.Range(0, audioClips.Count)];
        }

        Debug.LogWarning($"Sound group '{name}' not found or empty.");
        return null;
    }
}
