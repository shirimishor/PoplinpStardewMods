
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.ItemTypeDefinitions;
using StardewValley.GameData.Weapons;
using StardewValley.Delegates;




namespace ArmoryCabinet
{
    /// <summary>The main entry point for the mod.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {



            GameStateQuery.Register(
                $"{this.ModManifest.UniqueID}_WEAPON_TYPE",
                WeaponType
            );
        }



        public bool WeaponType(string[] query, GameStateQueryContext context)
        {
            if (!GameStateQuery.Helpers.TryGetItemArg(query, 1, context.TargetItem, context.InputItem, out var item, out var error))
            {
                return GameStateQuery.Helpers.ErrorResult(query, error);
            }

            if (item != null)
            {

                for (int i = 2; i < query.Length; i++)
                {
                    if (!ArgUtility.TryGetInt(query, i, out var weaponType, out error))
                    {
                        return GameStateQuery.Helpers.ErrorResult(query, error);
                    }
                    if (item.TypeDefinitionId.Equals("(W)"))
                    {
                        ParsedItemData data = ItemRegistry.GetData(item.QualifiedItemId);
                        WeaponData wd = (WeaponData)data.RawData;
                        return wd.Type == weaponType;
                    }
                }

            }
            return false;
        }


    }

}