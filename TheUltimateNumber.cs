using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalLib;
using LethalLib.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace TheUltimateNumber 
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency(LethalLib.Plugin.ModGUID, BepInDependency.DependencyFlags.HardDependency)]
    public class TheUltimateNumber : BaseUnityPlugin
    {
        public static TheUltimateNumber Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }
        internal static UltimateNumberConfig UltimateConfig { get; private set; } = null!;

        private void Awake()
        {
            UltimateConfig = new UltimateNumberConfig(base.Config);
            Logger = base.Logger;
            Instance = this;
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
            string assetBundlePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TheUltimateNumber.numbabundle");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetBundlePath);

                Item theUltimateNumberItem = bundle.LoadAsset<Item>("Assets/LethalCompany/Mods/TheUltimateNumber/items/TheUltimateNumber.asset");
                NetworkPrefabs.RegisterNetworkPrefab(theUltimateNumberItem.spawnPrefab);
                Utilities.FixMixerGroups(theUltimateNumberItem.spawnPrefab);
                if (TheUltimateNumber.UltimateConfig.isOnAllMoons.Value == true)
                {
                    Items.RegisterScrap(theUltimateNumberItem, TheUltimateNumber.UltimateConfig.numberRarity.Value, Levels.LevelTypes.All);
                }
            else
            {
                Items.RegisterScrap(theUltimateNumberItem, TheUltimateNumber.UltimateConfig.numberRarity.Value, Levels.LevelTypes.None);
            }

                Patch();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            Logger.LogDebug("Patching...");

            Harmony.PatchAll();

            Logger.LogDebug("Finished patching!");
        }

        internal static void Unpatch()
        {
            Logger.LogDebug("Unpatching...");

            Harmony?.UnpatchSelf();

            Logger.LogDebug("Finished unpatching!");
        }
    }
}

