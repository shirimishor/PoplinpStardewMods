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

        public override void Entry(IModHelper helper)
        {
            StaticHelper = Helper;

            Config = Helper.ReadConfig<ModConfig>();
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(SObject), "CheckForActionOnMachine"),
                postfix: new HarmonyMethod(typeof(ModEntry), nameof(CheckForActionOnMachine_Postfix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(SObject), "performObjectDropInAction"),
                postfix: new HarmonyMethod(typeof(ModEntry), nameof(performObjectDropInAction_Postfix))
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


        private static void performObjectDropInAction_Postfix(
            SObject __instance,
            Item dropInItem,
            Farmer who,
            ref bool __result
        )
        {
            if (__instance.QualifiedItemId != "(BC)Poplinp.CP_FairyRing_FairyRing")
                return;

            if (!(dropInItem.QualifiedItemId == "(O)872") || !__result)
                return;

            if (Config.AllowFairyDust)
                return;

            CombustRing(__instance);
            Game1.drawObjectDialogue(StaticHelper.Translation.Get("UsesFairyRing"));
            who.takeDamage(9999, true, null);
            return;

        }

        private static Dictionary<string, double> BuildDebuffTable()
        {
            Dictionary<string, double> debuffs = new Dictionary<string, double>();
            double SumChance = 0;

            debuffs.Add("combust", 0.005);
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
            if (debuff == "death")
            {
                who.takeDamage(9999, true, null);
                return;
            }

            if (debuff == "combust")
            {
                CombustRing(machine);
                Game1.drawObjectDialogue(StaticHelper.Translation.Get("CombustRing"));
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
            MachineLocation.removeObject(MachineTile, false);
            Game1.playSound("flameSpellHit");
            MachineLocation.temporarySprites.Add(new TemporaryAnimatedSprite(25, MachineTile * 64f, Color.White, 8, false, 80f, 0, -1, -1f, 128));
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
                name: () => StaticHelper.Translation.Get("ConfigDebuff.Name"),
                tooltip: () => StaticHelper.Translation.Get("ConfigDebuff.Desc"),
                getValue: () => Config.AllowDebuffs,
                setValue: value => Config.AllowDebuffs = value
            );

            // add some config options
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => StaticHelper.Translation.Get("ConfigDeath.Name"),
                tooltip: () => StaticHelper.Translation.Get("ConfigDeath.Desc"),
                getValue: () => Config.AllowDeath,
                setValue: value => Config.AllowDeath = value
            );

            // add some config options
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => StaticHelper.Translation.Get("ConfigFairyDust.Name"),
                tooltip: () => StaticHelper.Translation.Get("ConfigFairyDust.Desc"),
                getValue: () => Config.AllowFairyDust,
                setValue: value => Config.AllowFairyDust = value
            );
        }
    }
}
