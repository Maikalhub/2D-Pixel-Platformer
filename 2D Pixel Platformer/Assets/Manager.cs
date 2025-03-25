using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;
    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;

    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        if (soundEffectLibrary == null || audioSource == null) return;

        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    private void Start()
    {
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetVolume);
            SetVolume(sfxSlider.value); // Устанавливаем громкость при старте
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    private void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
}
