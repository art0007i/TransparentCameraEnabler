using HarmonyLib;
using ResoniteModLoader;
using System;
using System.Linq;
using System.Reflection;
using UnityFrooxEngineRunner;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using FrooxEngine;
using System.Reflection.Emit;
using Elements.Core;

namespace TransparentCameraEnabler;

public class TransparentCameraEnabler : ResoniteMod
{
    public override string Name => "TransparentCameraEnabler";
    public override string Author => "art0007i";
    public override string Version => "1.0.0";
    public override string Link => "https://github.com/art0007i/TransparentCameraEnabler/";
    public override void OnEngineInit()
    {
        Harmony harmony = new Harmony("me.art0007i.TransparentCameraEnabler");
        var toPatch = AccessTools.Method(AccessTools.Method(typeof(TextureDisplayBlitter), "Blit").GetCustomAttribute<IteratorStateMachineAttribute>().StateMachineType, "MoveNext");
        harmony.Patch(toPatch, transpiler: new(typeof(TransparentCameraEnabler).GetMethod(nameof(BlitTranspiler))));
    }
    public static IEnumerable<CodeInstruction> BlitTranspiler(IEnumerable<CodeInstruction> codes)
    {
        foreach (var code in codes)
        {
            if (code.operand is string s && s == "Unlit/Texture") code.operand = "Unlit/Transparent";
            yield return code;
        }
    }
}
