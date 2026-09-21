```csharp
using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;

namespace VenneChecker
{
    /// <summary>
    /// Manages the cheat mod database.
    /// Maps Photon custom property keys to cheat display names.
    /// Also loads user-defined entries from VenneChecker_Cheats.txt.
    /// Includes known cheat assembly namespaces for local detection.
    /// </summary>
    public static class CheatDatabase
    {
        // Maps property key -> display name
        private static readonly Dictionary<string, string> CheatMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Simple set for backward compat
        private static readonly HashSet<string> CheatNames =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Known cheat mod assembly namespaces / type names for local scanning
        // These detect mods that don't set CustomProperties.
        private static readonly Dictionary<string, string> CheatNamespaces =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private static string _filePath;

        public static string FilePath => _filePath;

        public static int Count =>
            CheatMap.Count + CheatNames.Count + CheatNamespaces.Count;

        public static void Load()
        {
            try
            {
                LoadBuiltInCheats();

                _filePath = Path.Combine(
                    Paths.ConfigPath,
                    "VenneChecker_Cheats.txt"
                );

                if (!File.Exists(_filePath))
                    CreateDefaultFile();

                LoadUserFile();
            }
            catch (Exception ex)
            {
                Log.Error($"CheatDatabase.Load failed: {ex.Message}");
            }
        }

        public static void ReloadList()
        {
            try
            {
                CheatMap.Clear();
                CheatNames.Clear();
                CheatNamespaces.Clear();

                LoadBuiltInCheats();
                LoadUserFile();
            }
            catch (Exception ex)
            {
                Log.Error($"CheatDatabase.ReloadList failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a property key is a known cheat.
        /// </summary>
        public static bool IsCheat(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            return CheatMap.ContainsKey(key) || CheatNames.Contains(key);
        }

        /// <summary>
        /// Gets the display name for a cheat property key.
        /// </summary>
        public static string GetDisplayName(string key)
        {
            if (string.IsNullOrEmpty(key))
                return key;

            if (CheatMap.TryGetValue(key, out string name))
                return name;

            return key;
        }

        /// <summary>
        /// Checks if a namespace/type name belongs to a known cheat mod assembly.
        /// </summary>
        public static bool IsCheatNamespace(string namespaceName)
        {
            if (string.IsNullOrEmpty(namespaceName))
                return false;

            return CheatNamespaces.ContainsKey(namespaceName);
        }

        /// <summary>
        /// Gets the display name for a cheat namespace.
        /// </summary>
        public static string GetNamespaceDisplayName(string namespaceName)
        {
            if (string.IsNullOrEmpty(namespaceName))
                return namespaceName;

            if (CheatNamespaces.TryGetValue(
                namespaceName,
                out string name))
            {
                return name;
            }

            return namespaceName;
        }

        /// <summary>
        /// Returns all known cheat namespaces for assembly scanning.
        /// </summary>
        public static Dictionary<string, string> GetCheatNamespaces()
        {
            return new Dictionary<string, string>(
                CheatNamespaces,
                StringComparer.OrdinalIgnoreCase
            );
        }

        /// <summary>
        /// Built-in known cheat property keys.
        /// These are Photon CustomProperties keys that cheat mods may set.
        /// </summary>
        private static void LoadBuiltInCheats()
        {
            // ============================================================
            // CHEAT MODS
            // ============================================================

            CheatMap["ObsidianMC"] = "Obsidian";
            CheatMap["obsidianmc"] = "Obsidian";

            CheatMap["genesis"] = "Genesis";

            CheatMap["elux"] = "Elux";

            CheatMap["VioletFreeUser"] = "Violet Free";
            CheatMap["VioletPaidUser"] = "Violet Paid";
            CheatMap["Violet On Top"] = "Violet";

            CheatMap["Hidden Menu"] = "Hidden";
            CheatMap["hidden menu"] = "Hidden";

            CheatMap["void"] = "Void";
            CheatMap["void_menu_open"] = "Void";

            CheatMap["cronos"] = "Cronos";

            CheatMap["ORBIT"] = "Orbit";
            CheatMap["ØƦƁƖƬ"] = "Orbit";

            CheatMap["ElixirMenu"] = "Elixir";
            CheatMap["Elixir"] = "Elixir";

            CheatMap["MistUser"] = "Mist";

            CheatMap["Untitled"] = "Untitled";

            CheatMap["dark"] = "ShibaGT Dark";

            CheatMap["oblivionuser"] = "Oblivion";

            CheatMap["eyerock reborn"] = "EyeRock";

            CheatMap["asteroidlite"] = "Asteroid Lite";

            CheatMap["cokecosmetics"] = "Coke Cosmetx";

            CheatMap["Atlas"] = "Atlas";

            CheatMap["Euphoric"] = "Euphoria";

            CheatMap["CurrentEmote"] = "Vortex Emotes";

            CheatMap["EmoteWheel"] = "Emotes";

            CheatMap["Explicit"] = "Explicit";
            CheatMap["explicit"] = "Explicit";

            CheatMap["envision build transform flourish"] = "Envision";

            CheatMap["https://chqser.lol/"] = "Chqser";

            CheatMap["y u lookin in here wei"] = "Malachi";

            CheatMap["Track Track Track Sahur"] = "Sahur Tracker";


            // ============================================================
            // CONTROL MOD DETECTION LIST
            // ============================================================

            CheatMap["control"] = "Control";
            CheatMap["controlfree"] = "Control Free";

            CheatMap["stupid"] = "IIStupid";

            CheatMap["6XpyykmrCthKhFeUfkYGxv7xnXpoe2"] = "Colossal";
            CheatMap["colossal"] = "Colossal";
            CheatMap["ccm"] = "Colossal";

            CheatMap["6p72ly3j85pau2g9mda6ib8px"] =
                "Colossal V2";

            CheatMap["rexon"] = "Rexon";

            CheatMap["destiny"] = "Destiny";

            CheatMap["symex"] = "Symex";

            CheatMap["graze"] = "Grate";

            CheatMap["imposter"] = "Gorilla Among Us";

            CheatMap["hgrehngio889584739_hugb\n"] =
                "Resurgence";

            CheatMap["RGBA"] =
                "Custom Cosmetics Cheat";


            // ============================================================
            // MALACHI
            // ============================================================

            CheatMap["OT"] = "Malachi OT";
            CheatMap["OS"] = "Malachi OS";


            // ============================================================
            // ASTRE / PROJECTILE LIB
            // ============================================================

            CheatMap["projlib_enable"] =
                "ProjectileLib (Cheat)";


            // ============================================================
            // ADDITIONAL MODS
            // ============================================================

            CheatMap["WalkSim"] =
                "WalkSim";

            CheatMap["Malachi Menu Reborn"] =
                "Malachi Menu Reborn";

            CheatMap["MalachiTemp"] =
                "MalachiTemp";


            // ============================================================
            // CHEAT NAMESPACE / ASSEMBLY DETECTION
            // ============================================================
            //
            // These detect mods via loaded assemblies/types.
            // They are separate from Photon CustomProperties detection.
            //

            CheatNamespaces["Seralyth"] =
                "Seralyth Menu";

            CheatNamespaces["Seralyth.Patches"] =
                "Seralyth Menu";

            CheatNamespaces["Seralyth.Patches.Menu"] =
                "Seralyth Menu";


            CheatNamespaces["ForeverCosmetx"] =
                "ForeverCosmetx (Cosmetic Hack)";

            CheatNamespaces["ForeverCosmetx.Patches"] =
                "ForeverCosmetx (Cosmetic Hack)";


            CheatNamespaces["CokeCosmetx"] =
                "Coke Cosmetx";

            CheatNamespaces["MonkeCosmetics"] =
                "MonkeCosmetics";

            CheatNamespaces["iiMenu"] =
                "iiMenu (Cheat)";


            CheatNamespaces["Obsidian"] =
                "Obsidian";

            CheatNamespaces["ObsidianMC"] =
                "Obsidian";

            CheatNamespaces["Genesis"] =
                "Genesis";

            CheatNamespaces["Elux"] =
                "Elux";

            CheatNamespaces["Violet"] =
                "Violet";

            CheatNamespaces["VoidMenu"] =
                "Void";

            CheatNamespaces["Cronos"] =
                "Cronos";

            CheatNamespaces["OrbitMenu"] =
                "Orbit";

            CheatNamespaces["ElixirMenu"] =
                "Elixir";


            // ============================================================
            // CONTROL
            // ============================================================

            CheatNamespaces["ControlMod"] =
                "Control";

            CheatNamespaces["Control.Mods"] =
                "Control";

            CheatNamespaces["Control.Menu"] =
                "Control";

            CheatNamespaces["Control.Menu.Main"] =
                "Control";

            CheatNamespaces["Control.gui.ModMenuPlugin"] =
                "Control";

            CheatNamespaces["Control.Mods.Advantage"] =
                "Control";

            CheatNamespaces["Control.Mods.Overpowered"] =
                "Control";

            CheatNamespaces["Control.Mods.NewOverpowered"] =
                "Control";


            // ============================================================
            // COLOSSAL / PROJECTILE LIB
            // ============================================================

            CheatNamespaces["Colossal"] =
                "Colossal";

            CheatNamespaces["ProjectileLib"] =
                "ProjectileLib (Cheat)";


            // ============================================================
            // MALACHI / ASTRE
            // ============================================================

            CheatNamespaces["Malachi"] =
                "Malachi";

            CheatNamespaces["Astre"] =
                "Astre";

            CheatNamespaces["ASTRE_Temp_V2.Mods"] =
                "Astre";


            // ============================================================
            // XENON / ASTRE
            // ============================================================

            CheatNamespaces["XENONTEMP"] =
                "XENON/Astre";

            CheatNamespaces["XENONTEMP.Mods"] =
                "XENON/Astre";

            CheatNamespaces["XENONTEMP.Menu"] =
                "XENON/Astre";

            CheatNamespaces["XENONTEMP.Menu.Main"] =
                "XENON/Astre";


            // ============================================================
            // CANVAS GUI / EUPH MENU
            // ============================================================

            CheatNamespaces["CanvasGUI"] =
                "CanvasGUI / Euph Menu";

            CheatNamespaces["CanvasGUI.Mods"] =
                "CanvasGUI / Euph Menu";

            CheatNamespaces["CanvasGUI.Mods.Categories"] =
                "CanvasGUI / Euph Menu";

            CheatNamespaces["CanvasGUI.Management.Menu"] =
                "CanvasGUI / Euph Menu";


            // ============================================================
            // CHQSER
            // ============================================================

            CheatNamespaces["ChqserFree"] =
                "Chqser";

            CheatNamespaces["ChqserFree.Menu"] =
                "Chqser";

            CheatNamespaces["ChqserFree.Mods"] =
                "Chqser";

            CheatNamespaces["ChqserFree.Mods.Fun"] =
                "Chqser";

            CheatNamespaces["ChqserFree.Mods.Global"] =
                "Chqser";

            CheatNamespaces["ChqserFree.Mods.Master"] =
                "Chqser";

            CheatNamespaces["ChqserFree.Mods.Safety"] =
                "Chqser";

            CheatNamespaces["ChqserFree.Classes.Menu"] =
                "Chqser";


            // ============================================================
            // NXO
            // ============================================================

            CheatNamespaces["NXO"] =
                "NXO Remastered";

            CheatNamespaces["NXO.Mods"] =
                "NXO Remastered";

            CheatNamespaces["NXO.Menu"] =
                "NXO Remastered";

            CheatNamespaces["NXO.Mods.Categories"] =
                "NXO Remastered";


            // ============================================================
            // OTHER MENUS
            // ============================================================

            CheatNamespaces["Nylox"] =
                "Nylox";

            CheatNamespaces["GKongMenu"] =
                "GKong Menu";

            CheatNamespaces["GKongMenu.GUI"] =
                "GKong Menu";


            // ============================================================
            // ADDITIONAL MOD ASSEMBLIES / NAMESPACES
            // ============================================================

            CheatNamespaces["WalkSim"] =
                "WalkSim";

            CheatNamespaces["MalachiMenuReborn"] =
                "Malachi Menu Reborn";

            CheatNamespaces["MalachiMenuReborn.Mods"] =
                "Malachi Menu Reborn";

            CheatNamespaces["MalachiTemp"] =
                "MalachiTemp";

            CheatNamespaces["MalachiTemp.Mods"] =
                "MalachiTemp";
        }


        private static void LoadUserFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return;

                string[] lines = File.ReadAllLines(_filePath);

                foreach (string line in lines)
                {
                    string trimmed = line.Trim();

                    if (string.IsNullOrEmpty(trimmed) ||
                        trimmed.StartsWith("#"))
                    {
                        continue;
                    }

                    CheatNames.Add(trimmed);
                }
            }
            catch (Exception ex)
            {
                Log.Error(
                    $"CheatDatabase.LoadUserFile failed: {ex.Message}"
                );
            }
        }


        private static void CreateDefaultFile()
        {
            try
            {
                string defaultContent =
                    "# STAR MODCHECKER Cheat Database (User Additions)\n" +
                    "# Built-in entries are loaded automatically.\n" +
                    "# This file is for YOUR additional entries.\n" +
                    "# Add one cheat mod property key per line.\n" +
                    "# Lines beginning with # are comments.\n" +
                    "#\n" +
                    "# Built-in detected mods include:\n" +
                    "# Obsidian, Genesis, Elux, Violet, Void, Cronos,\n" +
                    "# Orbit, Elixir, Mist, Untitled, ShibaGT Dark,\n" +
                    "# Oblivion, EyeRock, Asteroid Lite, Atlas, Euphoria,\n" +
                    "# Envision, Chqser, Malachi, Explicit, Hidden,\n" +
                    "# Control, Colossal, IIStupid, Rexon, Destiny,\n" +
                    "# Symex, Grate, Resurgence, ProjectileLib,\n" +
                    "# WalkSim, Malachi Menu Reborn, MalachiTemp, and more.\n" +
                    "#\n" +
                    "# Add your own below:\n";

                Directory.CreateDirectory(
                    Path.GetDirectoryName(_filePath)
                );

                File.WriteAllText(
                    _filePath,
                    defaultContent
                );
            }
            catch (Exception ex)
            {
                Log.Error(
                    $"Failed to create default cheats file: {ex.Message}"
                );
            }
        }
    }
}
