using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ItemManager;
using ServerSync;
using UnityEngine;

namespace DaedricSet
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class DaedricSetPlugin : BaseUnityPlugin
    {
        internal const string ModName = "DaedricSet";
        internal const string ModVersion = "1.0.0";
        internal const string Author = "azumatt";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        internal static string ConnectionError = "";

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource DaedricSetLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        public void Awake()
        {
            _serverConfigLocked = config("General", "Force Server Config", true, "Force Server Config");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);
            
            Item DaedricBattleAxe = new("daedricweapons", "DaedricBattleaxe");
            DaedricBattleAxe.Name.English("Daedric Battle Axe"); 
            DaedricBattleAxe.Description.English("A short poky stick from skyrim.");
            DaedricBattleAxe.Crafting.Add(CraftingTable.Forge,3); 
            DaedricBattleAxe.RequiredItems.Add("Iron", 120);
            DaedricBattleAxe.RequiredItems.Add("WolfFang", 20);
            DaedricBattleAxe.RequiredItems.Add("Silver", 40);
            DaedricBattleAxe.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricBattleAxe.RequiredUpgradeItems.Add("Silver", 10); 
            DaedricBattleAxe.CraftAmount = 2;

            Item DaedricDagger = new("daedricweapons", "DaedricDagger");
            DaedricDagger.Name.English("Daedric Dagger"); 
            DaedricDagger.Description.English("A short poky stick from skyrim.");
            DaedricDagger.Crafting.Add(CraftingTable.Forge,3); 
            DaedricDagger.RequiredItems.Add("Iron", 120);
            DaedricDagger.RequiredItems.Add("WolfFang", 20);
            DaedricDagger.RequiredItems.Add("Silver", 40);
            DaedricDagger.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricDagger.RequiredUpgradeItems.Add("Silver", 10); 
            DaedricDagger.CraftAmount = 2;
            
            Item DaedricGlaive = new("daedricweapons", "DaedricGlaive");
            DaedricGlaive.Name.English("Daedric Glaive"); 
            DaedricGlaive.Description.English("A short poky stick from skyrim.");
            DaedricGlaive.Crafting.Add(CraftingTable.Forge,3); 
            DaedricGlaive.RequiredItems.Add("Iron", 120);
            DaedricGlaive.RequiredItems.Add("WolfFang", 20);
            DaedricGlaive.RequiredItems.Add("Silver", 40);
            DaedricGlaive.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricGlaive.RequiredUpgradeItems.Add("Silver", 10);
            
            Item DaedricGreatsword = new("daedricweapons", "DaedricGreatsword");
            DaedricGreatsword.Name.English("Daedric Greatsword"); 
            DaedricGreatsword.Description.English("A short poky stick from skyrim.");
            DaedricGreatsword.Crafting.Add(CraftingTable.Forge,3); 
            DaedricGreatsword.RequiredItems.Add("Iron", 120);
            DaedricGreatsword.RequiredItems.Add("WolfFang", 20);
            DaedricGreatsword.RequiredItems.Add("Silver", 40);
            DaedricGreatsword.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricGreatsword.RequiredUpgradeItems.Add("Silver", 10);
            
            
            Item DaedricHalberd = new("daedricweapons", "DaedricHalberd");
            DaedricHalberd.Name.English("Daedric Halberd"); 
            DaedricHalberd.Description.English("A short poky stick from skyrim.");
            DaedricHalberd.Crafting.Add(CraftingTable.Forge,3); 
            DaedricHalberd.RequiredItems.Add("Iron", 120);
            DaedricHalberd.RequiredItems.Add("WolfFang", 20);
            DaedricHalberd.RequiredItems.Add("Silver", 40);
            DaedricHalberd.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricHalberd.RequiredUpgradeItems.Add("Silver", 10);
            
            Item DaedricHatchet = new("daedricweapons", "DaedricHatchet");
            DaedricHatchet.Name.English("Daedric Hatchet"); 
            DaedricHatchet.Description.English("A short poky stick from skyrim.");
            DaedricHatchet.Crafting.Add(CraftingTable.Forge,3); 
            DaedricHatchet.RequiredItems.Add("Iron", 120);
            DaedricHatchet.RequiredItems.Add("WolfFang", 20);
            DaedricHatchet.RequiredItems.Add("Silver", 40);
            DaedricHatchet.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricHatchet.RequiredUpgradeItems.Add("Silver", 10);
            DaedricHatchet.CraftAmount = 2;
            
            Item DaedricSkeletonClub = new("daedricweapons", "DaedricSkeletonClub");
            DaedricSkeletonClub.Name.English("Daedric Skeleton Club"); 
            DaedricSkeletonClub.Description.English("A short poky stick from skyrim.");
            DaedricSkeletonClub.Crafting.Add(CraftingTable.Forge,3); 
            DaedricSkeletonClub.RequiredItems.Add("Iron", 120);
            DaedricSkeletonClub.RequiredItems.Add("WolfFang", 20);
            DaedricSkeletonClub.RequiredItems.Add("Silver", 40);
            DaedricSkeletonClub.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricSkeletonClub.RequiredUpgradeItems.Add("Silver", 10);
            
            Item DaedricMace = new("daedricweapons", "DaedricMace");
            DaedricMace.Name.English("Daedric Mace"); 
            DaedricMace.Description.English("A short poky stick from skyrim.");
            DaedricMace.Crafting.Add(CraftingTable.Forge,3); 
            DaedricMace.RequiredItems.Add("Iron", 120);
            DaedricMace.RequiredItems.Add("WolfFang", 20);
            DaedricMace.RequiredItems.Add("Silver", 40);
            DaedricMace.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricMace.RequiredUpgradeItems.Add("Silver", 10);
            
            /*Item DaedricShortSpear = new("daedricweapons", "DaedricShortSpear");
            DaedricShortSpear.Name.English("Daedric Short Spear"); 
            DaedricShortSpear.Description.English("A short poky stick from skyrim.");
            DaedricShortSpear.Crafting.Add(CraftingTable.Forge,3); 
            DaedricShortSpear.RequiredItems.Add("Iron", 120);
            DaedricShortSpear.RequiredItems.Add("WolfFang", 20);
            DaedricShortSpear.RequiredItems.Add("Silver", 40);
            DaedricShortSpear.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricShortSpear.RequiredUpgradeItems.Add("Silver", 10);
            
            
            Item DaedricShortSword = new("daedricweapons", "DaedricShortSword");
            DaedricShortSword.Name.English("Daedric Short Sword"); 
            DaedricShortSword.Description.English("A short poky stick from skyrim.");
            DaedricShortSword.Crafting.Add(CraftingTable.Forge,3); 
            DaedricShortSword.RequiredItems.Add("Iron", 120);
            DaedricShortSword.RequiredItems.Add("WolfFang", 20);
            DaedricShortSword.RequiredItems.Add("Silver", 40);
            DaedricShortSword.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricShortSword.RequiredUpgradeItems.Add("Silver", 10);
            
            Item DaedricStaff = new("daedricweapons", "DaedricStaff");
            DaedricStaff.Name.English("Daedric Staff"); 
            DaedricStaff.Description.English("A short poky stick from skyrim.");
            DaedricStaff.Crafting.Add(CraftingTable.Forge,3); 
            DaedricStaff.RequiredItems.Add("Iron", 120);
            DaedricStaff.RequiredItems.Add("WolfFang", 20);
            DaedricStaff.RequiredItems.Add("Silver", 40);
            DaedricStaff.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricStaff.RequiredUpgradeItems.Add("Silver", 10);
            
            Item DaedricSword = new("daedricweapons", "DaedricSword");
            DaedricSword.Name.English("Daedric Sword"); 
            DaedricSword.Description.English("A poky stick from skyrim.");
            DaedricSword.Crafting.Add(CraftingTable.Forge,3); 
            DaedricSword.RequiredItems.Add("Iron", 120);
            DaedricSword.RequiredItems.Add("WolfFang", 20);
            DaedricSword.RequiredItems.Add("Silver", 40);
            DaedricSword.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricSword.RequiredUpgradeItems.Add("Silver", 10);
            
            Item DaedricWarAxe = new("daedricweapons", "DaedricWarAxe");
            DaedricWarAxe.Name.English("Daedric Sword"); 
            DaedricWarAxe.Description.English("A poky stick from skyrim.");
            DaedricWarAxe.Crafting.Add(CraftingTable.Forge,3); 
            DaedricWarAxe.RequiredItems.Add("Iron", 120);
            DaedricWarAxe.RequiredItems.Add("WolfFang", 20);
            DaedricWarAxe.RequiredItems.Add("Silver", 40);
            DaedricWarAxe.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricWarAxe.RequiredUpgradeItems.Add("Silver", 10);
            
            Item DaedricWarHammer = new("daedricweapons", "DaedricWarHammer");
            DaedricWarHammer.Name.English("Daedric Sword"); 
            DaedricWarHammer.Description.English("A poky stick from skyrim.");
            DaedricWarHammer.Crafting.Add(CraftingTable.Forge,3); 
            DaedricWarHammer.RequiredItems.Add("Iron", 120);
            DaedricWarHammer.RequiredItems.Add("WolfFang", 20);
            DaedricWarHammer.RequiredItems.Add("Silver", 40);
            DaedricWarHammer.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricWarHammer.RequiredUpgradeItems.Add("Silver", 10);*/


            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                DaedricSetLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                DaedricSetLogger.LogError($"There was an issue loading your {ConfigFileName}");
                DaedricSetLogger.LogError("Please check your config entries for spelling and format!");
            }
        }


        #region ConfigOptions

        private static ConfigEntry<bool>? _serverConfigLocked;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            public bool? Browsable = false;
        }

        #endregion
    }
}