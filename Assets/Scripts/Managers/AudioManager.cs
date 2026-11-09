using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip bgmClip;
    public AudioClip runeOn;
    public AudioClip runeOff;      // opsional
    public AudioClip buttonClick;  // opsional

    private const string BgmKey = "BgmVolume";
    private const string SfxKey = "SfxVolume";
    private const float DefaultBgmVolume = 0.7f;
    private const float DefaultSfxVolume = 1f;

    public float BgmVolume { get; private set; }
    public float SfxVolume { get; private set; }

    private void Awake()
    {
        // Kalau sudah ada AudioManager dari scene sebelumnya, yang baru dihancurkan
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        BgmVolume = PlayerPrefs.GetFloat(BgmKey, DefaultBgmVolume);
        SfxVolume = PlayerPrefs.GetFloat(SfxKey, DefaultSfxVolume);

        StartBgm();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    private void StartBgm()
    {
        if (bgmClip == null) return;

        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.volume = BgmVolume;
        bgmSource.Play();
    }

    public void SetBgmVolume(float volume)
    {
        BgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = BgmVolume;
        PlayerPrefs.SetFloat(BgmKey, BgmVolume);
    }

    public void SetSfxVolume(float volume)
    {
        SfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(SfxKey, SfxVolume);
    }

    public void PlaySfx(AudioClip clip, float pitchVariation = 0f)
    {
        if (clip == null) return;

        sfxSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        sfxSource.PlayOneShot(clip, SfxVolume);
    }

    // Untuk dipanggil dari script (tombol UI sebaiknya lewat UIManager)
    public void PlayClick() => PlaySfx(buttonClick);
}