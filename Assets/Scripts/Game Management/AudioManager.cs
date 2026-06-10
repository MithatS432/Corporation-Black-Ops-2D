using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource audioSource;
    [SerializeField] private AudioClip level1;

    [SerializeField] private AudioClip level2;

    [SerializeField] private AudioClip level3;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        audioSource =
            GetComponent<AudioSource>();

        audioSource.loop = true;
    }

    public void PlayMusic(int levelIndex)
    {
        AudioClip selectedClip;

        if (levelIndex <= 3)
        {
            selectedClip = level1;
        }
        else if (levelIndex <= 6)
        {
            selectedClip = level2;
        }
        else
        {
            selectedClip = level3;
        }

        if (audioSource.clip == selectedClip)
            return;

        audioSource.Stop();

        audioSource.clip =
            selectedClip;

        audioSource.Play();
    }
}