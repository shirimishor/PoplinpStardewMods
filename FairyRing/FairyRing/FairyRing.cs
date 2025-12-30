using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using SObject = StardewValley.Object;
using Netcode;
using System;
using System.Linq;

namespace YourModNamespace
{
    public class ModEntry : Mod
    {

        private static IModHelper StaticHelper;
        private static IMonitor StaticMonitor;

        public override void Entry(IModHelper helper)
        {
            StaticHelper = Helper;
            StaticMonitor = Monitor;


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

        private static string RollDebuff()
        {
            var debuffs = new (string id, double weight)[]
            {
                ("none", 0.7),
                ("19", 0.07375),
                ("Poplinp.CP_FairyRing_DizzyDebuff", 0.07375),
                ("Poplinp.CP_FairyRing_SleepyDebuff", 0.07375),
                ("26", 0.07375),
                ("death", 0.005)
            };

            double sum = debuffs.Sum(d => d.weight);
            double roll = Utility.getRandomDouble(0, sum);

            foreach (var d in debuffs)
            {
                roll -= d.weight;
                if (roll <= 0)
                    return d.id;
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
    }
}
