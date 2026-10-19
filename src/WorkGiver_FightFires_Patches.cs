using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace NoFirefightingInRain
{
    /// <summary>Shared rain check used by both patches.</summary>
    public static class RainCheck
    {
        // WorkGiver_FightFires is an internal class, so we can't reference it with typeof().
        // We look it up by name through reflection instead.
        public static readonly Type FightFiresType = AccessTools.TypeByName("RimWorld.WorkGiver_FightFires");

        public static MethodBase FindDeclared(string methodName)
        {
            return FightFiresType == null ? null : AccessTools.DeclaredMethod(FightFiresType, methodName);
        }

        public static bool ShouldIgnoreFire(Thing fire)
        {
            var settings = NoFirefightingInRainMod.Settings;
            if (settings == null || !settings.enabled || fire == null)
                return false;

            Map map = fire.Map;
            if (map == null)
                return false;

            // Roofed fires are not reached by rain, so colonists should still fight them
            if (fire.Position.Roofed(map))
                return false;

            return map.weatherManager.RainRate > settings.rainThreshold;
        }
    }

    // Each patch is skipped (Prepare returns false) if the method isn't declared
    // directly in WorkGiver_FightFires in this game version.

    [HarmonyPatch]
    public static class WorkGiver_FightFires_HasJobOnThing_Patch
    {
        static MethodBase TargetMethod() => RainCheck.FindDeclared("HasJobOnThing");

        static bool Prepare() => TargetMethod() != null;

        // 'forced' = the player explicitly ordered it, so we leave that alone
        static void Postfix(Thing t, bool forced, ref bool __result)
        {
            if (__result && !forced && RainCheck.ShouldIgnoreFire(t))
                __result = false;
        }
    }

    [HarmonyPatch]
    public static class WorkGiver_FightFires_JobOnThing_Patch
    {
        static MethodBase TargetMethod() => RainCheck.FindDeclared("JobOnThing");

        static bool Prepare() => TargetMethod() != null;

        static void Postfix(Thing t, bool forced, ref Verse.AI.Job __result)
        {
            if (__result != null && !forced && RainCheck.ShouldIgnoreFire(t))
                __result = null;
        }
    }
}