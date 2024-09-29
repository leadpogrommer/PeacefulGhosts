using System.Reflection;
using OWML.ModHelper;
using HarmonyLib;
using OWML.Common;

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
            // If Peace is not enabled, continue with the normal prefix
            if (!PeacefulGhosts.Instance.peaceEnabled) return true;
            
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
            if (!PeacefulGhosts.Instance.peaceEnabled) return;
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
            //If Peace is Enabled, return false to override the default prefix
            //If it's disabled, return true to allow the default prefix to continue
            return !PeacefulGhosts.Instance.peaceEnabled;
        }
    }

    public class PeacefulGhosts : ModBehaviour
    {
        
        public static PeacefulGhosts Instance { get; private set; }
        public bool peaceEnabled { get; private set; }
        
        public void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Instance = this;
        }

        private void Start()
        {
            peaceEnabled = ModHelper.Config.GetSettingsValue<bool>("Peaceful Mode");
            ModHelper.Console.WriteLine($"Start of {nameof(PeacefulGhosts)}");
        }

        public override void Configure(IModConfig config)
        {
            peaceEnabled = config.GetSettingsValue<bool>("Peaceful Mode");
            ModHelper.Console.WriteLine(peaceEnabled ? "Peace Established" : "Peace Ended");
        }
    }
}