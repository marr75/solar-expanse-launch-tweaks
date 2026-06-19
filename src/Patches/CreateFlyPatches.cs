using Game.UI.Windows.Elements.PlanMissionElements;
using HarmonyLib;

namespace LaunchFix.Patches;

/// <summary>
/// Suppresses the orbit-fuel start promotion while a mission is being written
/// to the schedule. OrbitFuelPatches promotes a surface SC's start to its
/// low-orbit equivalent for cost/display math, but CreateFly must record the
/// real surface origin so the surface-to-orbit launch event fires and fuel is
/// physically sent to orbit. Without this, the mission is stored as starting
/// in orbit and the launch never happens.
/// </summary>
[HarmonyPatch(typeof(PMTabSchedule), "CreateFly")]
internal static class CreateFlyPatches
{
    [HarmonyPrefix]
    private static void Prefix()
    {
        if (!ModConfig.OrbitFuelCredit)
            return;
        OrbitFuelPatches.SuppressPromotion = true;
    }

    [HarmonyFinalizer]
    private static void Finalizer()
    {
        OrbitFuelPatches.SuppressPromotion = false;
    }
}
