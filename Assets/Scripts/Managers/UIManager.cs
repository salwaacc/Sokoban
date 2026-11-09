using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endingPanel;

    [Header("Start")]
    [SerializeField] private bool useStartPanel = true;

    [Header("HUD")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text altarText;

    [Header("Ending")]
    [SerializeField] private TMP_Text finalTimeText;
    [SerializeField] private GameObject nextButton;

    [Header("Level")]
    [SerializeField] private int levelNumber = 1;
    [SerializeField] private float parTime = 90f;

    [Header("Stars")]
    [SerializeField] private Image starsFill;
    // Nilai fill untuk 0, 1, 2, 3 bintang
    [SerializeField] private float[] fillValues = { 0f, 0.33f, 0.66f, 1f };

    private const int MainMenuSceneIndex = 0;

    // Static supaya bertahan setelah reload scene (dipakai Restart)
    private static bool skipStartPanel;

    private bool isPlaying;
    private float elapsed;

    private int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;
    private bool HasNextLevel => CurrentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings;

    private void Start()
    {
        pausePanel.SetActive(false);
        endingPanel.SetActive(false);
        elapsed = 0f;

        bool showStart = useStartPanel && startPanel != null && !skipStartPanel;
        skipStartPanel = false; // hanya berlaku sekali

        if (showStart)
            ShowStartPanel();
        else
            StartGame();
    }

    private void Update()
    {
        if (!isPlaying) return;

        elapsed += Time.deltaTime;
        timerText.text = FormatTime(elapsed);

        if (Input.GetKeyDown(KeyCode.Escape) && !SettingsUI.IsOpen)
            Pause();
    }

    // ---------- HUD ----------

    public void SetAltarCount(int current, int total)
    {
        altarText.text = $"{current}/{total}";
    }

    // ---------- Game state ----------

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
    public void StartGame()
    {
        if (startPanel != null) startPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        SetPlaying(true);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        SetPlaying(false);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        SetPlaying(true);
    }

    public void ShowEnding()
    {
        gameplayPanel.SetActive(false);
        endingPanel.SetActive(true);
        SetPlaying(false);

        int stars = LevelProgress.CalculateStars(elapsed, parTime);
        LevelProgress.Save(levelNumber, elapsed, stars);

        finalTimeText.text = FormatTime(elapsed);
        starsFill.fillAmount = fillValues[Mathf.Clamp(stars, 0, fillValues.Length - 1)];
        nextButton.SetActive(HasNextLevel);
    }

    // ---------- Scene navigation ----------

    public void Restart()
    {
        skipStartPanel = true; // lewati start panel, langsung main
        LoadScene(CurrentSceneIndex);
    }

    public void MainMenu()
    {
        skipStartPanel = false;
        LoadScene(MainMenuSceneIndex);
    }

    public void NextLevel()
    {
        LoadScene(HasNextLevel ? CurrentSceneIndex + 1 : MainMenuSceneIndex);
    }

    // ---------- Helpers ----------

    private void ShowStartPanel()
    {
        startPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        SetPlaying(false);
    }

    private void SetPlaying(bool playing)
    {
        isPlaying = playing;
        Time.timeScale = playing ? 1f : 0f;
    }

    private static void LoadScene(int index)
    {
        Time.timeScale = 1f; // wajib dikembalikan sebelum pindah scene
        SceneManager.LoadScene(index);
    }

    private static string FormatTime(float seconds)
    {
        int minutes = (int)(seconds / 60f);
        int secs = (int)(seconds % 60f);
        return $"{minutes:00}:{secs:00}";
    }
}