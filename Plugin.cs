using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using ItemManager;
using PieceManager;
using ServerSync;
using UnityEngine;

namespace DaedricSet
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class DaedricSetPlugin : BaseUnityPlugin
    {
        internal const string ModName = "DaedricSet";
        internal const string ModVersion = "1.1.7";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        internal static string ConnectionError = "";

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource DaedricSetLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };
        
        public enum Toggle
        {
            On = 1,
            Off = 0
        }

        public void Awake()
        {
            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On, "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

            LoadDaedricBow();

            Item DaedricBattleAxe = new("daedricweapons", "DaedricBattleaxe");
            DaedricBattleAxe.Name.English("Daedric Battle Axe");
            DaedricBattleAxe.Description.English("Skyrim Daedric Battle Axe.");
            DaedricBattleAxe.Crafting.Add(CraftingTable.Forge, 3);
            DaedricBattleAxe.RequiredItems.Add("Iron", 40);
            DaedricBattleAxe.RequiredItems.Add("Silver", 40);
            DaedricBattleAxe.RequiredUpgradeItems.Add("Iron", 20);
            DaedricBattleAxe.RequiredUpgradeItems.Add("Silver", 10);

            Item DaedricDagger = new("daedricweapons", "DaedricDagger");
            DaedricDagger.Name.English("Daedric Dagger");
            DaedricDagger.Description.English("Skyrim Daedric Dagger.");
            DaedricDagger.Crafting.Add(CraftingTable.Forge, 3);
            DaedricDagger.RequiredItems.Add("Iron", 40);
            DaedricDagger.RequiredItems.Add("Silver", 40);
            DaedricDagger.RequiredUpgradeItems.Add("Iron", 20);
            DaedricDagger.RequiredUpgradeItems.Add("Silver", 10);
            DaedricDagger.CraftAmount = 2;

            Item DaedricGlaive = new("daedricweapons", "DaedricGlaive");
            DaedricGlaive.Name.English("Daedric Glaive");
            DaedricGlaive.Description.English("Skyrim Daedric Glaive.");
            DaedricGlaive.Crafting.Add(CraftingTable.Forge, 3);
            DaedricGlaive.RequiredItems.Add("Iron", 40);
            DaedricGlaive.RequiredItems.Add("Silver", 40);
            DaedricGlaive.RequiredUpgradeItems.Add("Iron", 20);
            DaedricGlaive.RequiredUpgradeItems.Add("Silver", 10);

            Item DaedricGreatsword = new("daedricweapons", "DaedricGreatsword");
            DaedricGreatsword.Name.English("Daedric Greatsword");
            DaedricGreatsword.Description.English("Skyyrim Daedric Greatsword.");
            DaedricGreatsword.Crafting.Add(CraftingTable.Forge, 3);
            DaedricGreatsword.RequiredItems.Add("Iron", 40);
            DaedricGreatsword.RequiredItems.Add("Silver", 40);
            DaedricGreatsword.RequiredUpgradeItems.Add("Iron", 20);
            DaedricGreatsword.RequiredUpgradeItems.Add("Silver", 10);


            Item DaedricHalberd = new("daedricweapons", "DaedricHalberd");
            DaedricHalberd.Name.English("Daedric Halberd");
            DaedricHalberd.Description.English("Skyyrim Daedric Halberd.");
            DaedricHalberd.Crafting.Add(CraftingTable.Forge, 3);
            DaedricHalberd.RequiredItems.Add("Iron", 40);
            DaedricHalberd.RequiredItems.Add("Silver", 40);
            DaedricHalberd.RequiredUpgradeItems.Add("Iron", 20);
            DaedricHalberd.RequiredUpgradeItems.Add("Silver", 10);

            Item DaedricHatchet = new("daedricweapons", "DaedricHatchet");
            DaedricHatchet.Name.English("Daedric Hatchet");
            DaedricHatchet.Description.English("Skyrim Daedric Hatchet.");
            DaedricHatchet.Crafting.Add(CraftingTable.Forge, 3);
            DaedricHatchet.RequiredItems.Add("Iron", 40);
            DaedricHatchet.RequiredItems.Add("Silver", 40);
            DaedricHatchet.RequiredUpgradeItems.Add("Iron", 20);
            DaedricHatchet.RequiredUpgradeItems.Add("Silver", 10);
            DaedricHatchet.CraftAmount = 2;

            Item DaedricSkeletonClub = new("daedricweapons", "DaedricSkeletonClub");
            DaedricSkeletonClub.Name.English("Daedric Skeleton Club");
            DaedricSkeletonClub.Description.English("Skyyrim Daedric Skeleton Club.");
            DaedricSkeletonClub.Crafting.Add(CraftingTable.Forge, 3);
            DaedricSkeletonClub.RequiredItems.Add("Iron", 40);
            DaedricSkeletonClub.RequiredItems.Add("Silver", 40);
            DaedricSkeletonClub.RequiredUpgradeItems.Add("Iron", 20);
            DaedricSkeletonClub.RequiredUpgradeItems.Add("Silver", 10);

            Item DaedricMace = new("daedricweapons", "DaedricMace");
            DaedricMace.Name.English("Daedric Mace");
            DaedricMace.Description.English("Skyrim Daedric Mace.");
            DaedricMace.Crafting.Add(CraftingTable.Forge, 3);
            DaedricMace.RequiredItems.Add("Iron", 40);
            DaedricMace.RequiredItems.Add("Silver", 40);
            DaedricMace.RequiredUpgradeItems.Add("Iron", 20);
            DaedricMace.RequiredUpgradeItems.Add("Silver", 10);

            /*Item DaedricShortSpear = new("daedricweapons", "DaedricShortSpear");
            DaedricShortSpear.Name.English("Daedric Short Spear"); 
            DaedricShortSpear.Description.English("A short poky stick from skyrim.");
            DaedricShortSpear.Crafting.Add(CraftingTable.Forge,3); 
            DaedricShortSpear.RequiredItems.Add("Iron", 40);
            DaedricShortSpear.RequiredItems.Add("Silver", 40);
            DaedricShortSpear.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricShortSpear.RequiredUpgradeItems.Add("Silver", 10);
            
            
            Item DaedricShortSword = new("daedricweapons", "DaedricShortSword");
            DaedricShortSword.Name.English("Daedric Short Sword"); 
            DaedricShortSword.Description.English("A short poky stick from skyrim.");
            DaedricShortSword.Crafting.Add(CraftingTable.Forge,3); 
            DaedricShortSword.RequiredItems.Add("Iron", 40);
            DaedricShortSword.RequiredItems.Add("Silver", 40);
            DaedricShortSword.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricShortSword.RequiredUpgradeItems.Add("Silver", 10);
            
            Item DaedricStaff = new("daedricweapons", "DaedricStaff");
            DaedricStaff.Name.English("Daedric Staff"); 
            DaedricStaff.Description.English("A short poky stick from skyrim.");
            DaedricStaff.Crafting.Add(CraftingTable.Forge,3); 
            DaedricStaff.RequiredItems.Add("Iron", 40);
            DaedricStaff.RequiredItems.Add("Silver", 40);
            DaedricStaff.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricStaff.RequiredUpgradeItems.Add("Silver", 10);*/

            Item DaedricSword = new("daedricweapons", "DaedricSword");
            DaedricSword.Name.English("Daedric Sword");
            DaedricSword.Description.English("Skyrim Daedric Sword.");
            DaedricSword.Crafting.Add(CraftingTable.Forge, 3);
            DaedricSword.RequiredItems.Add("Iron", 40);
            DaedricSword.RequiredItems.Add("Silver", 40);
            DaedricSword.RequiredUpgradeItems.Add("Iron", 20);
            DaedricSword.RequiredUpgradeItems.Add("Silver", 10);

            /*Item DaedricWarAxe = new("daedricweapons", "DaedricWarAxe");
            DaedricWarAxe.Name.English("Daedric Sword"); 
            DaedricWarAxe.Description.English("A poky stick from skyrim.");
            DaedricWarAxe.Crafting.Add(CraftingTable.Forge,3); 
            DaedricWarAxe.RequiredItems.Add("Iron", 40);
            DaedricWarAxe.RequiredItems.Add("Silver", 40);
            DaedricWarAxe.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricWarAxe.RequiredUpgradeItems.Add("Silver", 10);
            
            Item DaedricWarHammer = new("daedricweapons", "DaedricWarHammer");
            DaedricWarHammer.Name.English("Daedric Sword"); 
            DaedricWarHammer.Description.English("A poky stick from skyrim.");
            DaedricWarHammer.Crafting.Add(CraftingTable.Forge,3); 
            DaedricWarHammer.RequiredItems.Add("Iron", 40);
            DaedricWarHammer.RequiredItems.Add("Silver", 40);
            DaedricWarHammer.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricWarHammer.RequiredUpgradeItems.Add("Silver", 10);*/
            
            Item DaedricChest = new("daedrictest", "ArmorDaedricChest");
            DaedricChest.Name.English("Daedric Chest"); 
            DaedricChest.Description.English("A work in progress Daedric Armor piece.");
            DaedricChest.Crafting.Add(CraftingTable.Forge,3); 
            DaedricChest.RequiredItems.Add("Iron", 40);
            DaedricChest.RequiredItems.Add("Silver", 40);
            DaedricChest.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricChest.RequiredUpgradeItems.Add("Silver", 10);
            //MaterialReplacer.RegisterGameObjectForShaderSwap(DaedricChest.Prefab, MaterialReplacer.ShaderType.CustomCreature);
            
            Item DaedricLegs = new("daedrictest", "ArmorDaedricLegs");
            DaedricLegs.Name.English("Daedric Legs"); 
            DaedricLegs.Description.English("A work in progress Daedric Armor piece.");
            DaedricLegs.Crafting.Add(CraftingTable.Forge,3); 
            DaedricLegs.RequiredItems.Add("Iron", 40);
            DaedricLegs.RequiredItems.Add("Silver", 40);
            DaedricLegs.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricLegs.RequiredUpgradeItems.Add("Silver", 10);
            //MaterialReplacer.RegisterGameObjectForShaderSwap(DaedricLegs.Prefab, MaterialReplacer.ShaderType.CustomCreature);
            
            Item DaedricHelmet = new("daedrictest", "HelmetDaedric");
            DaedricHelmet.Name.English("Daedric Helmet"); 
            DaedricHelmet.Description.English("A work in progress Daedric Armor piece.");
            DaedricHelmet.Crafting.Add(CraftingTable.Forge,3); 
            DaedricHelmet.RequiredItems.Add("Iron", 40);
            DaedricHelmet.RequiredItems.Add("Silver", 40);
            DaedricHelmet.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricHelmet.RequiredUpgradeItems.Add("Silver", 10);
            //MaterialReplacer.RegisterGameObjectForShaderSwap(DaedricHelmet.Prefab, MaterialReplacer.ShaderType.CustomCreature);
            
            
            
            
            
            Item DaedricChestFem = new("daedrictest", "ArmorDaedricChestFemale");
            DaedricChestFem.Name.English("Daedric Chest Female"); 
            DaedricChestFem.Description.English("A work in progress Daedric Armor piece. (Female)");
            DaedricChestFem.Crafting.Add(CraftingTable.Forge,3); 
            DaedricChestFem.RequiredItems.Add("Iron", 40);
            DaedricChestFem.RequiredItems.Add("Silver", 40);
            DaedricChestFem.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricChestFem.RequiredUpgradeItems.Add("Silver", 10);
            //MaterialReplacer.RegisterGameObjectForShaderSwap(DaedricChest.Prefab, MaterialReplacer.ShaderType.CustomCreature);
            
            Item DaedricLegsFem = new("daedrictest", "ArmorDaedricLegsFemale");
            DaedricLegsFem.Name.English("Daedric Legs Female"); 
            DaedricLegsFem.Description.English("A work in progress Daedric Armor piece. (Female)");
            DaedricLegsFem.Crafting.Add(CraftingTable.Forge,3); 
            DaedricLegsFem.RequiredItems.Add("Iron", 40);
            DaedricLegsFem.RequiredItems.Add("Silver", 40);
            DaedricLegsFem.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricLegsFem.RequiredUpgradeItems.Add("Silver", 10);
            //MaterialReplacer.RegisterGameObjectForShaderSwap(DaedricLegs.Prefab, MaterialReplacer.ShaderType.CustomCreature);
            
            Item DaedricHelmetFem = new("daedrictest", "HelmetDaedricFemale");
            DaedricHelmetFem.Name.English("Daedric Helmet Female"); 
            DaedricHelmetFem.Description.English("A work in progress Daedric Armor piece. (Female)");
            DaedricHelmetFem.Crafting.Add(CraftingTable.Forge,3); 
            DaedricHelmetFem.RequiredItems.Add("Iron", 40);
            DaedricHelmetFem.RequiredItems.Add("Silver", 40);
            DaedricHelmetFem.RequiredUpgradeItems.Add("Iron", 20); 
            DaedricHelmetFem.RequiredUpgradeItems.Add("Silver", 10);
            //MaterialReplacer.RegisterGameObjectForShaderSwap(DaedricHelmet.Prefab, MaterialReplacer.ShaderType.CustomCreature);


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
        
        private void LoadDaedricBow()
        {
            Item daedricbow = new("daedricbow", "DaedricBow");
            daedricbow.Name.English("Daedric Bow"); // You can use this to fix the display name in code
            daedricbow.Description.English("A daedric bow from skyrim.");
            daedricbow.Crafting.Add(CraftingTable.Forge, 2); // Custom crafting stations can be specified as a string
            daedricbow.MaximumRequiredStationLevel =
                2; // Limits the crafting station level required to upgrade or repair the item to 5
            daedricbow.RequiredItems.Add("TrophySurtling", 5);
            daedricbow.RequiredItems.Add("Obsidian", 20);
            daedricbow.RequiredItems.Add("Flametal", 100);
            daedricbow.RequiredUpgradeItems.Add("TrophySurtling",
                25); // Upgrade requirements are per item, even if you craft two at the same time
            daedricbow.RequiredUpgradeItems.Add("Obsidian",
                150); // 10 Silver: You need 10 silver for level 2, 20 silver for level 3, 30 silver for level 4
            daedricbow.RequiredUpgradeItems.Add("Flametal", 75);
        }


        #region ConfigOptions

        private static ConfigEntry<Toggle> _serverConfigLocked = null!;

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
    
    
    [HarmonyPatch(typeof(ZNetScene),nameof(ZNetScene.Awake))]
    static class EffectFixZNetSceneAwakePatch
    {
        static void Postfix(ZNetScene __instance)
        {
            var daedricBowItemDrop = __instance.GetPrefab("DaedricBow").GetComponent<ItemDrop>();
            var bowDraugrFangItemDrop = __instance.GetPrefab("BowDraugrFang").GetComponent<ItemDrop>();
            
            // Swap out the trigger effects on the DaedricBow with that from the DraugrFangBow
            daedricBowItemDrop.m_itemData.m_shared.m_holdStartEffect = bowDraugrFangItemDrop.m_itemData.m_shared.m_holdStartEffect;
            daedricBowItemDrop.m_itemData.m_shared.m_blockEffect = bowDraugrFangItemDrop.m_itemData.m_shared.m_blockEffect;
            daedricBowItemDrop.m_itemData.m_shared.m_triggerEffect = bowDraugrFangItemDrop.m_itemData.m_shared.m_triggerEffect;
        }
    }
}