using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using NightScene.UI;

namespace YequQteSkip;

[BepInPlugin("xu.yequ.qteskip", "Yequ QTE Skip", "0.2.0")]
public sealed class Plugin : BasePlugin
{
    internal static ConfigEntry<bool> Enabled = null!;
    internal static ManualLogSource Logger = null!;

    public override void Load()
    {
        Logger = Log;
        Enabled = Config.Bind("General", "Enabled", true,
            "做菜时跳过夜雀之歌，按满分结算。改成 false 就恢复原样。");
        new Harmony("xu.yequ.qteskip").PatchAll(typeof(OpenQtePanelInternalAsyncPatch));
        Logger.LogInfo("夜雀之歌跳过已加载");
    }
}

/// <summary>
/// 手动开锅时游戏走：SetCook → SolveQteMultiplier → OpenQTEPanelInternalAsync（弹音游，等分数）
/// → OnQTESucceeded → StartCookCountDown(分数)。探路实测打满时分数是 1。
/// 外层 OpenQTEPanelForCook 被编译揉进了调用方，拦不到；这里拦开面板这一步，
/// 不弹面板，直接交回一个已完成的 1 分，后面的奖励和倒计时照游戏原样走。
/// </summary>
[HarmonyPatch(typeof(WorkSceneSustainedPannel), nameof(WorkSceneSustainedPannel.OpenQTEPanelInternalAsync))]
internal static class OpenQtePanelInternalAsyncPatch
{
    private static bool Prefix(ref UniTask<float> __result)
    {
        if (!Plugin.Enabled.Value) return true;

        try
        {
            __result = new UniTask<float>(1f);
        }
        catch (Exception ex)
        {
            // 出错就退回原版，照常弹音游
            Plugin.Logger.LogWarning($"跳过失败，照常弹音游：{ex.GetBaseException().Message}");
            return true;
        }

        Plugin.Logger.LogInfo("跳过一次夜雀之歌，交 1 分");
        return false;
    }
}
