using HarmonyLib;
using UnityEngine;
using Verse;

namespace NoFirefightingInRain
{
    public class NoFirefightingInRainMod : Mod
    {
        public static NoFirefightingInRainSettings Settings;

        public NoFirefightingInRainMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<NoFirefightingInRainSettings>();
            new Harmony("ivotints.nofirefightinginrain").PatchAll();
        }

        public override string SettingsCategory()
        {
            return "No Firefighting During Rain";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.CheckboxLabeled(
                "Enable mod",
                ref Settings.enabled,
                "When off, colonists fight fires as in vanilla.");

            listing.Gap();

            Rect labelRect = listing.GetRect(Text.LineHeight);
            Widgets.Label(labelRect, $"Rain threshold: {Settings.rainThreshold:F2}");
            TooltipHandler.TipRegion(
                labelRect,
                "Colonists stop fighting UNROOFED fires when the current rain rate is above this value.\n\n" +
                "0.00 = any rain at all\n1.00 = never (effectively disables the mod)\n" +
                $"Default: {NoFirefightingInRainSettings.DefaultRainThreshold:F2}");

            float value = listing.Slider(Settings.rainThreshold, 0f, 1f);
            Settings.rainThreshold = Mathf.Round(value * 100f) / 100f;

            listing.Gap();

            if (listing.ButtonText("Reset to default"))
            {
                Settings.enabled = true;
                Settings.rainThreshold = NoFirefightingInRainSettings.DefaultRainThreshold;
            }

            listing.End();
            base.DoSettingsWindowContents(inRect);
        }
    }
}