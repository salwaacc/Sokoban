using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PuzzleManager : MonoBehaviour
    {
        [System.Serializable]
        public class PuzzleRule
        {
            public string name;                          // label saja, biar rapi di Inspector
            public List<PropsAltar> requiredAltars;      // semua harus terisi
            public List<DoorController> doors;
            public List<StairController> obstacle;
        }

        [Header("Altars")]
        [SerializeField] private List<PropsAltar> allAltars;   // termasuk altar final
        [SerializeField] private PropsAltar finalAltar;

        [Header("Rules")]
        [SerializeField] private List<PuzzleRule> rules;

        [Header("UI")]
        [SerializeField] private UIManager uiManager;

        private bool endingShown;

        private void OnEnable()
        {
            foreach (var a in allAltars)
                if (a != null) a.OnAltarChanged += CheckPuzzle;
        }

        private void OnDisable()
        {
            foreach (var a in allAltars)
                if (a != null) a.OnAltarChanged -= CheckPuzzle;
        }

        private void Start()
        {
            CheckPuzzle();
        }

        private void CheckPuzzle()
        {
            // Counter altar
            int count = 0;
            foreach (var a in allAltars)
                if (a != null && a.IsOccupied) count++;
            uiManager.SetAltarCount(count, allAltars.Count);

            // Aturan pintu dan tangga
            foreach (var rule in rules)
            {
                bool met = true;
                foreach (var a in rule.requiredAltars)
                {
                    if (a == null || !a.IsOccupied) { met = false; break; }
                }

                foreach (var d in rule.doors)
                    if (d != null) d.SetOpen(met);

                foreach (var o in rule.obstacle)
                    if (o != null) o.SetActive(met);
            }

            // Ending
            if (finalAltar != null && finalAltar.IsOccupied && !endingShown)
            {
                endingShown = true;
                uiManager.ShowEnding();
            }
        }
    }
}