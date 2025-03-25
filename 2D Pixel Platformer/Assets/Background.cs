using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager instance;
    private static AudioSource audioSource;

    [SerializeField] private AudioClip backgroundMusic; // Основная фоновая музыка
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();

            if (backgroundMusic != null)
            {
                audioSource.clip = backgroundMusic;
                audioSource.loop = true; // Зацикливаем музыку
                audioSource.Play();
            }

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(SetVolume);
            SetVolume(musicSlider.value); // Устанавливаем громкость при старте
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
        SetVolume(musicSlider.value);
    }
}
