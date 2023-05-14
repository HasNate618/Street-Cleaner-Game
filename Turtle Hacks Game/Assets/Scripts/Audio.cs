using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    private static AudioSource soundEffectSource;
    [SerializeField] AudioClip[] soundEffectsArray;
    private static Dictionary<string, AudioClip> soundEffects;

    public static Audio instance;

    private void Start()
    {
        instance = this;
        soundEffectSource = GetComponent<AudioSource>();

        if (soundEffects == null)
        {
            soundEffects = new Dictionary<string, AudioClip>();
            foreach (AudioClip sound in soundEffectsArray)
            {
                soundEffects.Add(sound.name, sound);
            }
        }
    }

    public static void PlaySound(string name, AudioSource source)
    {
        if (soundEffects.ContainsKey(name))
        {
            AudioClip sound = soundEffects[name];
            source ??= soundEffectSource;
            source.PlayOneShot(sound);
        }
        else
        {
            Debug.LogWarning("Sound Effect \"" + name + "\" not found");
        }
    }
}
