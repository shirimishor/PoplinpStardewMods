using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using SObject = StardewValley.Object;
using Netcode;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

            debuffs.Add("combust", 0.05);
            SumChance += 0.005;

            if (Config.AllowDebuffs)
            {
                debuffs.Add("19", 0.06);
                debuffs.Add("26", 0.06);
                debuffs.Add("Poplinp.CP_FairyRing_DizzyDebuff", 0.06);
                debuffs.Add("Poplinp.CP_FairyRing_SleepyDebuff", 0.06);
                SumChance += 0.24;
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

            if (debuff == "combust")
            {
                CombustRing(machine);
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

        private static void CombustRing (SObject machine)
        {
            Vector2 MachineTile = machine.TileLocation;
            GameLocation MachineLocation = machine.Location;
            StaticMonitor.Log($"removing fairy ring in {MachineLocation} at position: {MachineTile}", LogLevel.Info);
            MachineLocation.removeObject(MachineTile, false);
            Game1.playSound("flameSpellHit");
            MachineLocation.temporarySprites.Add(new TemporaryAnimatedSprite(25, MachineTile * 64f, Color.White, 8, false, 80f, 0, -1, -1f, 128));
            Game1.drawObjectDialogue("You hear laughter in the distance. The Fairy Ring has spontaneously combusted!");
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
