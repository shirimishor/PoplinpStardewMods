
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.ItemTypeDefinitions;
using StardewValley.GameData.Weapons;
using StardewValley.Delegates;
using StardewValley.GameData.Shops;
using StardewModdingAPI.Events;
using System.Linq;



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

            helper.Events.Content.AssetRequested += OnAssetRequested;
        

        GameStateQuery.Register(
                $"{this.ModManifest.UniqueID}_WEAPON_TYPE",
                WeaponType
            );
        }


        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (!e.NameWithoutLocale.IsEquivalentTo("Data/Shops"))
                return;

            e.Edit(asset =>
            {
                var shops = asset.AsDictionary<string, ShopData>().Data;

                if (shops.TryGetValue("AdventureShop", out var shop))
                {
                    AddIfItemExists(shop, "Poplinp.ArmoryCabinet.armory_cabinet", "(F)Poplinp.ArmoryCabinet.armory_cabinet", 5000);
                    AddIfItemExists(shop, "Poplinp.ArmoryCabinet.armory_cabinet_dark", "(F)Poplinp.ArmoryCabinet.armory_cabinet_dark", 5000);
                    AddIfItemExists(shop, "Poplinp.ArmoryCabinet.armory_cabinet_light_narrow", "(F)Poplinp.ArmoryCabinet.armory_cabinet_light_narrow", 5000);
                    AddIfItemExists(shop, "Poplinp.ArmoryCabinet.armory_cabinet_dark_narrow", "(F)Poplinp.ArmoryCabinet.armory_cabinet_dark_narrow", 5000);
                }

                if (shops.TryGetValue("Furniture Catalogue", out shop))
                {
                    AddIfItemExists(shop, "Poplinp.ArmoryCabinet.armory_cabinet", "(F)Poplinp.ArmoryCabinet.armory_cabinet", 0);
                    AddIfItemExists(shop, "Poplinp.ArmoryCabinet.armory_cabinet_dark", "(F)Poplinp.ArmoryCabinet.armory_cabinet_dark", 0);
                    AddIfItemExists(shop, "Poplinp.ArmoryCabinet.armory_cabinet_light_narrow", "(F)Poplinp.ArmoryCabinet.armory_cabinet_light_narrow", 0);
                    AddIfItemExists(shop, "Poplinp.ArmoryCabinet.armory_cabinet_dark_narrow", "(F)Poplinp.ArmoryCabinet.armory_cabinet_dark_narrow", 0);
                }
            });
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


        private static ShopItemData CreateShopItem(string id, string itemId, int price)
        {
                return new ShopItemData
                {
                    Id = id,
                    ItemId = itemId,

                    Price = price,
                    TradeItemAmount = 1,
                    AvailableStock = -1,
                    AvoidRepeat = false,
                    UseObjectDataPrice = false,
                    IgnoreShopPriceModifiers = false,
                    MinStack = -1,
                    MaxStack = -1,
                    Quality = -1,
                    ToolUpgradeLevel = -1,
                    IsRecipe = false,
                    AvailableStockLimit = LimitedStockMode.Global,
                    PriceModifierMode = StardewValley.GameData.QuantityModifier.QuantityModifierMode.Stack,
                    AvailableStockModifierMode = StardewValley.GameData.QuantityModifier.QuantityModifierMode.Stack,
                    StackModifierMode = StardewValley.GameData.QuantityModifier.QuantityModifierMode.Stack,
                    QualityModifierMode = StardewValley.GameData.QuantityModifier.QuantityModifierMode.Stack,
                };
        }

        private static void AddIfItemExists(ShopData shop, string id, string itemId, int price)
        {
            if (!ItemRegistry.Exists(itemId))
                return;

            if (shop.Items.Any(p => p.Id == id))
                return;

            shop.Items.Add(CreateShopItem(id, itemId, price));
        }


    }

}