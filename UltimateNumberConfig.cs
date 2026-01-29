using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace TheUltimateNumber
{
    class UltimateNumberConfig
    {
        public readonly ConfigEntry<double>? hundredsChance;
        public readonly ConfigEntry<double>? thousandsChance;
        public readonly ConfigEntry<double>? tenThousandsChance;
        public readonly ConfigEntry<double>? hundredThousandsChance;
        public readonly ConfigEntry<double>? millionsChance;
        public readonly ConfigEntry<double>? tenMillionsChance;
        public readonly ConfigEntry<double>? hundredMillionsChance;
        public readonly ConfigEntry<double>? billionsChance;
        public readonly ConfigEntry<double>? intLimitChance;
        public readonly ConfigEntry<float>? numberAudioSourceVolume;
        public readonly ConfigEntry<float>? numberAudioSourceMinDistance;
        public readonly ConfigEntry<float>? numberAudioSourceMaxDistance;
        public readonly ConfigEntry<float>? explosionPhysicsForce;
        public UltimateNumberConfig(ConfigFile ultimateConfig)
        {
            ultimateConfig.SaveOnConfigSet = false;
            hundredsChance = ultimateConfig.Bind(
                "Number Value Chances",
                "tensChance",
                50.0d,
                "(0-100) Percent chance of The Ultimate Number's value being upgraded from 1-9 -> 10-99. Default: 50.0"
                );
            thousandsChance = ultimateConfig.Bind(
                "Number Value Chances",
                "hundredsChance",
                20.0d,
                "(0-100) Percent chance of The Ultimate Number's value being upgraded from 10-99 -> 100-999. Default: 20.0"
                );
            tenThousandsChance = ultimateConfig.Bind(
                "Number Value Chances",
                "thousandsChance",
                10.0d,
                "(0-100) Percent chance of The Ultimate Number's value being upgraded from 100-999 -> 1,000-9,999. Default: 10.0"
                );
            hundredThousandsChance = ultimateConfig.Bind(
                "Number Value Chances",
                "tenThousandsChance",
                10.0d,
                "(0-100) Percent chance of The Ultimate Number's value being upgraded from 1,000-9,999 -> 10,000-99,999. Default: 10.0"
                );
            millionsChance = ultimateConfig.Bind(
                "Number Value Chances",
                "hundredThousandsChance",
                10.0d,
                "(0-100) Percent chance of The Ultimate Number's value being upgraded from 10,000-99,999 -> 100,000-999,999. Default: 10.0"
                );
            tenMillionsChance = ultimateConfig.Bind(
                "Number Value Chances",
                "millionsChance",
                10.0d,
                "(0-100) Percent chance of The Ultimate Number's value being upgraded from 100,000-999,999 -> 1,000,000-9,999,999. Default: 10.0"
                );
            hundredMillionsChance = ultimateConfig.Bind(
                "Number Value Chances",
                "tenMillionsChance",
                10.0d,
                "(0-100) Percent chance of The Ultimate Number's value being upgraded from 1,000,000-9,999,999 -> 10,000,000-99,999,999. Default: 10.0"
                );
            billionsChance = ultimateConfig.Bind(
                "Number Value Chances",
                "hundredMillionsChance",
                10.0d,
                "(0-100) Percent chance of The Ultimate Number's value being upgraded from 10,000,000-99,999,999 -> 100,000,000-999,999,999. Default: 10.0"
                );
            intLimitChance = ultimateConfig.Bind(
                "Number Value Chances",
                "billionsChance",
                50.0d,
                "(0-100) Percent chance of The Ultimate Number's value being upgraded from 100,000,000-999,999,999 -> 1,000,000,000-2,147,483,647. Default: 50.0"
                );
            numberAudioSourceVolume = ultimateConfig.Bind(
                "Number Audio Source Properties",
                "numberAudioSourceVolume",
                1f,
                "(0-1) Volume of The Ultimate Number's audio source. Default: 1.0"
                );
            numberAudioSourceMinDistance = ultimateConfig.Bind(
                "Number Audio Source Properties",
                "minDistance",
                4f,
               "The maximum distance from The Ultimate Number's audio source where audio will be played at maximum volume. Default: 4"
                );
            numberAudioSourceMaxDistance = ultimateConfig.Bind(
               "Number Audio Source Properties",
               "maxDistance",
               17f,
               "The maximum distance from The Ultimate Number's audio source where audio will be played. Default: 17"
               );
            explosionPhysicsForce = ultimateConfig.Bind(
               "Other Number Properties",
               "explosionPhysicsForce",
               0f,
               "The extra physics force applied to any explosions. Default: 0.0"
               );
            ClearOrphanedEntries(ultimateConfig);
            ultimateConfig.Save();
            ultimateConfig.SaveOnConfigSet = true;
        }
        static void ClearOrphanedEntries(ConfigFile ultimateConfig)
        {
            // yeah this was taken directly from the example on lethal.wiki lol
            PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
            var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(ultimateConfig);
            orphanedEntries.Clear();
        }
    }
}
