using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace TheUltimateNumber.Patches;

[HarmonyPatch]
public static class ScrapSyncCheck
{
    public static event EventHandler ScrapSyncedEvent;
    public static Object ScrapSyncedSender;

    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SyncScrapValuesClientRpc))]
    [HarmonyPostfix]
    public static void SyncScrapValuesClientRpc_Postfix()
    {
        Console.WriteLine("Scrap spawned! Applying postfix!");
        OnScrapSynced();
    }

    public static void OnScrapSynced()
    {
        ScrapSyncedEvent?.Invoke(ScrapSyncedSender, EventArgs.Empty);
        Console.WriteLine("Invoked event!");
    }
}
