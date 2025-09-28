using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Renderite.Unity;

namespace TransparentCameraEnabler;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
#nullable disable
    static ManualLogSource Log;
#nullable enable

    public void Awake()
    {
        Log = Logger;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
    }

    [HarmonyPatch(typeof(TextureDisplayBlitter), "Blit", MethodType.Enumerator)]
    public static class BlitTranspiler
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var success = false;
            foreach (var code in codes)
            {
                if (code.operand is string s && s == "Unlit/Texture")
                {
                    code.operand = "Unlit/Transparent";
                    success = true;
                }
                yield return code;
            }
            if (!success)
            {
                Log.LogError("Failed to patch TextureDisplayBlitter.Blit! The mod will not function.");
            }
        }
    }
}
