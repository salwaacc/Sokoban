using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PropsAltar : MonoBehaviour
    {
        private const string StatueTag = "Statue";
        private const float RuneSfxPitchVariation = 0.05f;

        [Header("Altar Runes")]
        [SerializeField] private List<SpriteRenderer> altarRunes;

        [Header("Pillar Runes")]
        [SerializeField] private List<SpriteRenderer> pillarRunes;

        [Header("Glow Settings")]
        [SerializeField] private float lerpSpeed = 3f;

        [Header("Altar Final")]
        [SerializeField] private bool onlyStatue = false;

        public bool IsOccupied { get; private set; }

        public event Action OnAltarChanged;

        private StoneController currentStone;
        private readonly HashSet<Collider2D> stoneColliders = new HashSet<Collider2D>();
        private float targetAlpha;

        private void Awake()
        {
            SetAllRunesAlpha(0f);
        }

        private void Update()
        {
            FadeRunes(altarRunes);
            FadeRunes(pillarRunes);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!TryGetValidStone(other, out StoneController stone)) return;

            // Hanya satu batu yang boleh menempati altar
            if (currentStone != null && currentStone != stone) return;

            currentStone = stone;
            stoneColliders.Add(other);

            if (IsOccupied) return;

            IsOccupied = true;
            targetAlpha = 1f;

            PlayRuneSound();
            OnAltarChanged?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            StoneController stone = other.GetComponentInParent<StoneController>();
            if (stone == null || stone != currentStone) return;

            stoneColliders.Remove(other);

            // Tunggu sampai semua collider batu keluar
            if (stoneColliders.Count > 0) return;

            currentStone = null;
            IsOccupied = false;
            targetAlpha = 0f;

            OnAltarChanged?.Invoke();
        }

        private bool TryGetValidStone(Collider2D other, out StoneController stone)
        {
            stone = other.GetComponentInParent<StoneController>();
            if (stone == null) return false;

            // Altar final hanya menerima Statue
            if (onlyStatue && !stone.CompareTag(StatueTag)) return false;

            return true;
        }

        private void PlayRuneSound()
        {
            AudioManager audio = AudioManager.Instance;
            if (audio == null) return;

            audio.PlaySfx(audio.runeOn, RuneSfxPitchVariation);
        }

        private void FadeRunes(List<SpriteRenderer> runes)
        {
            float step = lerpSpeed * Time.deltaTime;

            foreach (SpriteRenderer rune in runes)
            {
                if (rune == null) continue;

                SetAlpha(rune, Mathf.Lerp(rune.color.a, targetAlpha, step));
            }
        }

        private void SetAllRunesAlpha(float alpha)
        {
            foreach (SpriteRenderer rune in altarRunes)
                if (rune != null) SetAlpha(rune, alpha);

            foreach (SpriteRenderer rune in pillarRunes)
                if (rune != null) SetAlpha(rune, alpha);
        }

        private static void SetAlpha(SpriteRenderer renderer, float alpha)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }
}