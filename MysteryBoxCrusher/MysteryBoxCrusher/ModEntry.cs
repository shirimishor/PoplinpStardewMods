using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.GameData.Machines;

namespace MysteryBoxCrusher
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            
        }

        public static Item GetItemForMysteryBox(
            StardewValley.Object machine,
            Item inputItem,
            bool probe,
            MachineItemOutput outputData,
            Farmer player,
            out int? overrideMinutesUntilReady
        )
        {
            overrideMinutesUntilReady = null;

            if (probe || inputItem == null)
                return null;

            Game1.stats.Increment("MysteryBoxesOpened");


            return Utility.getTreasureFromGeode(inputItem);
        }


        
    }
}
