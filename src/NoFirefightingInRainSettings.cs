using UnityEngine;
using Verse;

namespace NoFirefightingInRain
{
    public class NoFirefightingInRainSettings : ModSettings
    {
        public const float DefaultRainThreshold = 0.5f;

        public bool enabled = true;
        public float rainThreshold = DefaultRainThreshold;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref enabled, "enabled", true);
            Scribe_Values.Look(ref rainThreshold, "rainThreshold", DefaultRainThreshold);

            // Guard against corrupted/hand-edited values
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                rainThreshold = Mathf.Clamp(rainThreshold, 0f, 1f);
            }
        }
    }
}