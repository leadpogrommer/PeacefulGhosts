using OWML.ModHelper;

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

    public class PeacefulGhosts : ModBehaviour
    {
        private void Start()
        {
            ModHelper.Logger.Log($"Start of {nameof(PeacefulGhosts)}");
            ModHelper.HarmonyHelper.AddPrefix<GrabAction>("CalculateUtility", typeof(Patch), "Prefix");
            ModHelper.HarmonyHelper.AddPrefix<ChaseAction>("CalculateUtility", typeof(Patch), "Prefix");
            ModHelper.HarmonyHelper.AddPrefix<HuntAction>("CalculateUtility", typeof(Patch), "Prefix");
            ModHelper.HarmonyHelper.AddPrefix<StalkAction>("CalculateUtility", typeof(Patch), "Prefix");
        }
    }
}