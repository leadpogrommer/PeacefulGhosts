using OWML.ModHelper;
using HarmonyLib;

namespace PeacefulGhosts
{
    public class Patch
    {
        private static bool Prefix(ref float __result)
        {
            __result = -1337f;
            return false;
        }
    }

    public class SensorsPatch
    {
        private static void Postfix(GhostSensors __instance)
        {
            var data = Traverse.Create(__instance).Field("_data").GetValue() as GhostData;
            var sensor = data.sensor;
            sensor.isIlluminatedByPlayer = false;
            sensor.isPlayerIlluminatedByUs = false;
            sensor.isPlayerIlluminated = false;
            sensor.isPlayerVisible = false;
            sensor.isPlayerHeldLanternVisible = false;
            sensor.isPlayerDroppedLanternVisible = false;
            sensor.isPlayerOccluded = false;
        }
    }

    public class PartyDirectorPatch
    {
        private static bool Prefix()
        {
            return false;
        }
    }

    public class PeacefulGhosts : ModBehaviour
    {
        private void Start()
        {
            ModHelper.Logger.Log($"Start of {nameof(PeacefulGhosts)}");
            ModHelper.HarmonyHelper.AddPrefix<GrabAction>("CalculateUtility", typeof(Patch), "Prefix");
            ModHelper.HarmonyHelper.AddPrefix<ChaseAction>("CalculateUtility", typeof(Patch), "Prefix");
            ModHelper.HarmonyHelper.AddPrefix<HuntAction>("CalculateUtility", typeof(Patch), "Prefix");
            ModHelper.HarmonyHelper.AddPrefix<StalkAction>("CalculateUtility", typeof(Patch), "Prefix");
            
            ModHelper.Logger.Log("Patching GhostSensors");
            ModHelper.HarmonyHelper.AddPostfix<GhostSensors>("FixedUpdate_Sensors", typeof(SensorsPatch), "Postfix");
            
            ModHelper.HarmonyHelper.AddPrefix<GhostPartyDirector>("OnEnterDoorTrigger", typeof(PartyDirectorPatch), "Prefix");
            ModHelper.HarmonyHelper.AddPrefix<GhostPartyDirector>("OnEnterAmbushTrigger", typeof(PartyDirectorPatch), "Prefix");
            
        }
    }
}