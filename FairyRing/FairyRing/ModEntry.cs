using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using SObject = StardewValley.Object;
using Netcode;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FairyRing
{
    public class ModEntry : Mod
    {

        internal static ModConfig Config { get; private set; }
        private static IModHelper StaticHelper;
        private static IMonitor StaticMonitor;

        public override void Entry(IModHelper helper)
        {
            StaticHelper = Helper;
            StaticMonitor = Monitor;

            Config = Helper.ReadConfig<ModConfig>();
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(SObject), "CheckForActionOnMachine"),
                postfix: new HarmonyMethod(typeof(ModEntry), nameof(CheckForActionOnMachine_Postfix))
            );
        }


        private static void CheckForActionOnMachine_Postfix(
            SObject __instance,
            Farmer who,
            bool justCheckingForActivity,
            ref bool __result
        )
        {
            if (justCheckingForActivity || !__result)
                return;

            if (__instance.QualifiedItemId != "(BC)Poplinp.CP_FairyRing_FairyRing")
                return;


            var readyNet = StaticHelper.Reflection.GetField<NetBool>(__instance, "readyForHarvest").GetValue();
            bool readyForHarvest = readyNet.Value;


            var heldNet = StaticHelper.Reflection.GetField<NetRef<SObject>>(__instance, "heldObject").GetValue();
            SObject heldObject = heldNet?.Value;

            if (!readyForHarvest && heldObject != null)
            {
                OnMachineOutputCollected(__instance, who, heldObject);
            }
        }

        private static Dictionary<string, double> BuildDebuffTable()
        {
            Dictionary<string, double> debuffs = new Dictionary<string, double>();
            double SumChance = 0;

            if (Config.AllowDebuffs)
            {
                debuffs.Add("19", 0.07375);
                debuffs.Add("26", 0.07375);
                debuffs.Add("Poplinp.CP_FairyRing_DizzyDebuff", 0.07375);
                debuffs.Add("Poplinp.CP_FairyRing_SleepyDebuff", 0.07375);
                SumChance += 0.295;
            }

            if (Config.AllowDeath)
            {
                debuffs.Add("death", 0.005);
                SumChance += 0.005;
            }

            debuffs.Add("none", 1-SumChance);
            return debuffs;

        }
        
        private static string RollDebuff()
        {

            Dictionary<string, double> debuffs = BuildDebuffTable();

            double sum = debuffs.Sum(d => d.Value);
            double roll = Utility.getRandomDouble(0, sum);

            foreach (var d in debuffs)
            {
                roll -= d.Value;
                if (roll <= 0)
                    return d.Key;
            }

            return null;
        }


        private static void OnMachineOutputCollected(SObject machine, Farmer who, Item output)
        {

            var debuff = RollDebuff();
            StaticMonitor.Log($"Applying curse {debuff} ", LogLevel.Info);
            if (debuff == "death")
            {
                who.takeDamage(9999, true, null);
                return;
            }
            if (debuff != "none")
            {
                who.applyBuff(debuff);
                Game1.playSound("debuffHit");
                return;
            }
            Game1.playSound("fairy_heal");


        }

        private void OnGameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;


            // register mod
            configMenu.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => Helper.WriteConfig(Config)
            );

            // add some config options
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => "Allow Debuffs",
                tooltip: () => "Chance to get a debuff when collecting a Fairy Ring product.",
                getValue: () => Config.AllowDebuffs,
                setValue: value => Config.AllowDebuffs = value
            );

            // add some config options
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => "Allow Death",
                tooltip: () => "Chance to trigger death when collecting a Fairy Ring product.",
                getValue: () => Config.AllowDeath,
                setValue: value => Config.AllowDeath = value
            );
        }
    }
}
