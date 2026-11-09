using UnityEngine;

public static class LevelProgress
{
    private const string UnlockedKey = "Unlocked";
    private const float TwoStarMultiplier = 1.5f;

    private static string BestKey(int level) => $"Best_{level}";
    private static string StarsKey(int level) => $"Stars_{level}";

    public static int CalculateStars(float time, float parTime)
    {
        if (time <= parTime) return 3;
        if (time <= parTime * TwoStarMultiplier) return 2;
        return 1;
    }

    public static void Save(int level, float time, int stars)
    {
        // Simpan waktu terbaik
        if (time < PlayerPrefs.GetFloat(BestKey(level), float.MaxValue))
            PlayerPrefs.SetFloat(BestKey(level), time);

        // Simpan bintang tertinggi
        if (stars > GetStars(level))
            PlayerPrefs.SetInt(StarsKey(level), stars);

        // Buka level berikutnya
        if (level + 1 > PlayerPrefs.GetInt(UnlockedKey, 1))
            PlayerPrefs.SetInt(UnlockedKey, level + 1);

        PlayerPrefs.Save();
    }

    /// Waktu terbaik, atau -1 jika belum pernah selesai
    public static float GetBest(int level) =>
        PlayerPrefs.GetFloat(BestKey(level), -1f);

    public static int GetStars(int level) =>
        PlayerPrefs.GetInt(StarsKey(level), 0);

    public static bool IsUnlocked(int level) =>
        level <= PlayerPrefs.GetInt(UnlockedKey, 1);
}