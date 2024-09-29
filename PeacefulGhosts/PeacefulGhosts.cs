using System.Reflection;
using OWML.ModHelper;
using HarmonyLib;

namespace PeacefulGhosts
{
    [HarmonyPatch]
    public class Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GrabAction), nameof(GrabAction.CalculateUtility))]
        [HarmonyPatch(typeof(ChaseAction), nameof(ChaseAction.CalculateUtility))]
        [HarmonyPatch(typeof(HuntAction), nameof(HuntAction.CalculateUtility))]
        [HarmonyPatch(typeof(StalkAction), nameof(StalkAction.CalculateUtility))]
        private static bool Prefix(ref float __result)
        {
            __result = -1337f;
            return false;
        }
    }

    [HarmonyPatch]
    public class SensorsPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GhostSensors), nameof(GhostSensors.FixedUpdate_Sensors))]
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

    [HarmonyPatch]
    public class PartyDirectorPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GhostPartyDirector), nameof(GhostPartyDirector.OnEnterDoorTrigger))]
        [HarmonyPatch(typeof(GhostPartyDirector), nameof(GhostPartyDirector.OnEnterAmbushTrigger))]
        private static bool Prefix()
        {
            return false;
        }
    }

    public class PeacefulGhosts : ModBehaviour
    {
        public void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        private void Start()
        {
            ModHelper.Console.WriteLine($"Start of {nameof(PeacefulGhosts)}");
        }
    }
}