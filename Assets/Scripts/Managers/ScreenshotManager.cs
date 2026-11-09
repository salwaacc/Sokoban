using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
    [SerializeField] TMP_Text messageText;
    [SerializeField] float messageDuration = 3f;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void DownloadFile(string fileName, byte[] data, int size);
#endif

    Coroutine messageRoutine;

    void Start()
    {
        if (messageText != null) messageText.gameObject.SetActive(false);
    }

    // Connect to ss button
    public void TakeScreenshot()
    {
        StartCoroutine(Capture());
    }

    IEnumerator Capture()
    {
        // Pesan lama disembunyikan dulu supaya tidak ikut tertangkap
        if (messageText != null) messageText.gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();

        Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
        byte[] png = tex.EncodeToPNG();
        Destroy(tex);

        string fileName = $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";

#if UNITY_WEBGL && !UNITY_EDITOR
        DownloadFile(fileName, png, png.Length);
        ShowMessage("Screenshot diunduh, cek folder Downloads browser");
#else
        try
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Application.productName);
            Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, fileName);
            File.WriteAllBytes(path, png);

            Debug.Log("Screenshot tersimpan di: " + path);
            ShowMessage("Tersimpan di:\n" + path);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            ShowMessage("Gagal menyimpan screenshot");
        }
#endif
    }

    void ShowMessage(string msg)
    {
        if (messageText == null) return;
        if (messageRoutine != null) StopCoroutine(messageRoutine);
        messageRoutine = StartCoroutine(MessageRoutine(msg));
    }

    IEnumerator MessageRoutine(string msg)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        // Realtime, karena game bisa sedang pause (timeScale = 0)
        yield return new WaitForSecondsRealtime(messageDuration);
        messageText.gameObject.SetActive(false);
    }
}