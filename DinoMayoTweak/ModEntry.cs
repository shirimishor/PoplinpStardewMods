using StardewModdingAPI;
using StardewValley;
using StardewValley.Constants;
using SObject = StardewValley.Object;
using HarmonyLib;

public class ModEntry : Mod
{


    public override void Entry(IModHelper helper)
    {
        var harmony = new Harmony(ModManifest.UniqueID);

        harmony.Patch(
            original: AccessTools.Method(typeof(SObject), "getPriceAfterMultipliers"),
            postfix: new HarmonyMethod(typeof(ModEntry), nameof(getPriceAfterMultipliers_Postfix))
        );
    }

    private static float getPriceAfterMultipliers_Postfix(
    float __result,
    SObject __instance,
    float startPrice,
    long specificPlayerID
    
    )
    {
        float price = __result;
        
        //multiplayer logic copied directly from original method
        foreach (Farmer player in Game1.getAllFarmers())
        {
            if (Game1.player.useSeparateWallets)
            {
                if (specificPlayerID == -1)
                {
                    if (player.UniqueMultiplayerID != Game1.player.UniqueMultiplayerID || !player.isActive())
                    {
                        continue;
                    }
                }
                else if (player.UniqueMultiplayerID != specificPlayerID)
                {
                    continue;
                }
            }
            else if (!player.isActive())
            {
                continue;
            }


            //actual dino mayo tweaking
            if (player.stats.Get(StatKeys.Book_Artifact) != 0 && (__instance.QualifiedItemId == "(O)807"))
            {
                price *= 3f;
            }
        }
            return price;


    }
}

