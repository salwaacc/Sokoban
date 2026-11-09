using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static bool IsOpen { get; private set; }

    [SerializeField] GameObject settingsPanel;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    float previousTimeScale = 1f;

    void Start()
    {
        IsOpen = false;   // reset setelah reload scene

        var am = AudioManager.Instance;
        if (am != null)
        {
            bgmSlider.SetValueWithoutNotify(am.BgmVolume);
            sfxSlider.SetValueWithoutNotify(am.SfxVolume);

            bgmSlider.onValueChanged.AddListener(am.SetBgmVolume);
            sfxSlider.onValueChanged.AddListener(am.SetSfxVolume);
        }

        settingsPanel.SetActive(false);
    }

    public void Open()
    {
        if (IsOpen) return;
        IsOpen = true;

        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        settingsPanel.SetActive(true);
    }

    public void Close()
    {
        if (!IsOpen) return;
        IsOpen = false;

        settingsPanel.SetActive(false);
        Time.timeScale = previousTimeScale;
    }
}