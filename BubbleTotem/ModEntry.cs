using System;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Tools;
using StardewValley.GameData.Objects;
using HarmonyLib;
using SObject = StardewValley.Object;
using Microsoft.VisualBasic;


namespace BubbleTotem
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        internal static ModConfig Config { get; private set; }
        private static IModHelper StaticHelper;

        private static IMonitor StaticMonitor = null!;

        private static int startBubblesTime = -1;

        private static GameLocation? bubblesLocation = null;
        private static Microsoft.Xna.Framework.Point? totemBubblePoint = null;

        private static IReflectionHelper? Reflection;

        /*********
        ** Public methods
        *********/

        public override void Entry(IModHelper helper)
        {
            StaticHelper = Helper;
            StaticMonitor = Monitor;
            Reflection = helper.Reflection;

            Config = StaticHelper.ReadConfig<ModConfig>();

            helper.Events.Content.AssetRequested += OnAssetRequested;
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;


            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(
                    typeof(SObject),
                    nameof(SObject.performUseAction)
                ),
                postfix: new HarmonyMethod(
                    typeof(ModEntry),
                    nameof(performUseAction_Postfix)
                )
            );

            harmony.Patch(
                original: AccessTools.Method(
                    typeof(SObject),
                    nameof(SObject.drawInMenu),
                    new[]
                    {
                        typeof(SpriteBatch),
                        typeof(Vector2),
                        typeof(float),
                        typeof(float),
                        typeof(float),
                        typeof(StackDrawType),
                        typeof(Microsoft.Xna.Framework.Color),
                        typeof(bool)
                    }
                ),
                postfix: new HarmonyMethod(
                    typeof(ModEntry),
                    nameof(DrawInMenu_Postfix)
                )
            );

            harmony.Patch(
                original: AccessTools.Method(
                    typeof(SObject),
                    nameof(SObject.drawWhenHeld),
                    new[]
                    {
                        typeof(SpriteBatch),
                        typeof(Vector2),
                        typeof(Farmer)
                    }
                ),
                postfix: new HarmonyMethod(
                    typeof(ModEntry),
                    nameof(DrawWhenHeld_Postfix)
                )
            );


        }

        /*********
        ** Private methods
        *********/

        private void OnAssetRequested(
            object? sender,
            AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.Name.Equals("Data/Objects"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, ObjectData>().Data;

                    data.Add("Poplinp.BubbleTotem_BubbleTotem", new ObjectData()
                    {
                        Name = "Bubble Totem",
                        DisplayName = this.Helper.Translation.Get("BubbleTotem.Name"),
                        Description = this.Helper.Translation.Get("BubbleTotem.Desc"),
                        Type = "Crafting",
                        Category = 0,
                        Price = 20,
                        Texture = "Mods/Poplinp.BubbleTotem/Objects",
                        SpriteIndex = 0,
                        ColorOverlayFromNextIndex = false,
                        Edibility = -300,
                        IsDrink = false,
                        Buffs = null,
                        GeodeDropsDefaultItems = false,
                        GeodeDrops = null,
                        ArtifactSpotChances = null,
                        CanBeGivenAsGift = true,
                        CanBeTrashed = true,
                        ExcludeFromFishingCollection = false,
                        ExcludeFromShippingCollection = false,
                        ExcludeFromRandomSale = true,
                        ContextTags = new()
                        {
                            "color_brown",
                            "not_placeable",
                            "totem_item"
                        },
                        CustomFields = null
                    });
                });
            }

            if (e.NameWithoutLocale.Name.Equals("Data/CraftingRecipes"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, string>().Data;

                    data.Add("Poplinp.BubbleTotem_BubbleTotem", "709 1 167 1 DeluxeBait 5/Field/Poplinp.BubbleTotem_BubbleTotem/false/s Fishing 8/");
                });
            }

            if (e.NameWithoutLocale.Name.Equals("Mods/Poplinp.BubbleTotem/Objects"))
            {

                if (Config.animatedTotem)
                {
                    e.LoadFromModFile<Texture2D>(
                    "assets/totem-sprite.png",
                    AssetLoadPriority.Medium
                    );
                }
                else 
                {
                    e.LoadFromModFile<Texture2D>(
                    "assets/totem-sprite-freeze.png",
                    AssetLoadPriority.Medium
                    );
                }

                
            }

            if (e.NameWithoutLocale.Name.Equals("Mods/Poplinp.BubbleTotem/Animation"))
            {
                e.LoadFromModFile<Texture2D>(
                    "assets/overlay-bubbles.png",
                    AssetLoadPriority.Medium
                );
            }


        }


        private static void performUseAction_Postfix(
            SObject __instance,
            GameLocation location,
            ref bool __result)
        {
            if (__result ||
                __instance.QualifiedItemId !=
                "(O)Poplinp.BubbleTotem_BubbleTotem")
            {
                return;
            }


            __result = bubblerAction(Game1.player, location); 
        }



        private static bool bubblerAction(Farmer who, GameLocation gameLocation)
        {
            Vector2 center = who.Tile;


            int maxDistance = (who.FacingDirection == 1 ||
                               who.FacingDirection == 3)
                ? Config.bubblesMaxTile+1  // Horizontal
                : Config.bubblesMaxTile; // Vertical




            for (int distance = maxDistance; distance > 0; distance--)
            {
                int tileX = (int)center.X;
                int tileY = (int)center.Y;

                switch (who.FacingDirection)
                {
                    case 0: tileY -= distance; break;
                    case 1: tileX += distance; break;
                    case 2: tileY += distance; break;
                    case 3: tileX -= distance; break;
                }

                var bubbleTile = new Microsoft.Xna.Framework.Point(tileX, tileY);

                if (!gameLocation.canFishHere() || !gameLocation.isTileFishable(tileX, tileY))
                {

                    continue;
                }

                Game1.playSound("bubbles");
                TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite(0, 9999f, 1, 999, Game1.player.Position + new Vector2(0f, -96f), flicker: false, flipped: false, verticalFlipped: false, 0f)
                {
                    motion = new Vector2(0f, -1f),
                    scaleChange = 0.01f,
                    alpha = 1f,
                    alphaFade = 0.0075f,
                    shakeIntensity = 1f,
                    initialPosition = Game1.player.Position + new Vector2(0f, -96f),
                    xPeriodic = true,
                    xPeriodicLoopTime = 1000f,
                    xPeriodicRange = 4f,
                    layerDepth = 1f
                };

                sprite.CopyAppearanceFromItemId("(O)Poplinp.BubbleTotem_BubbleTotem");
                gameLocation.temporarySprites.Add(sprite);

                Utility.addSprinklesToLocation(gameLocation, tileX, tileY, 6, 6, 1000, 50, Microsoft.Xna.Framework.Color.Cyan, null, motionTowardCenter: true);

                CreateBubbles(gameLocation, bubbleTile);
               

                return true;
            }

            Game1.showRedMessageUsingLoadString("Strings\\StringsFromCSFiles:Object.cs.13053");
            return false;
        }


        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!Game1.IsMasterGame ||
                startBubblesTime == -1 ||
                bubblesLocation == null ||
                !totemBubblePoint.HasValue)
            {
                return;
            }

            if (bubblesLocation.fishSplashPoint.Value !=
                totemBubblePoint.Value)
            {
                return;
            }

            Reflection?
                .GetField<int>(bubblesLocation, "fishSplashPointTime")
                .SetValue(Game1.timeOfDay);
        }

        private void OnTimeChanged(
            object? sender,
            TimeChangedEventArgs e)
        {
            if (startBubblesTime == -1 || bubblesLocation == null)
                return;

            int durationSoFar =
                Utility.CalculateMinutesBetweenTimes(
                    startBubblesTime,
                    Game1.timeOfDay
                );

            if (durationSoFar < Config.bubblesLength)
                return;

            if (totemBubblePoint.HasValue &&
                bubblesLocation.fishSplashPoint.Value ==
                totemBubblePoint.Value)
            {
                bubblesLocation.fishSplashPoint.Value =
                    Microsoft.Xna.Framework.Point.Zero;
            }


            startBubblesTime = -1;
            totemBubblePoint = null;
            bubblesLocation = null;
        }

        private static void CreateBubbles(
            GameLocation gameLocation,
            Microsoft.Xna.Framework.Point fishPoint)
        {
            bubblesLocation = gameLocation;
            totemBubblePoint = fishPoint;

            bubblesLocation.fishSplashPoint.Value = fishPoint;

            Reflection?
                .GetField<int>(bubblesLocation, "fishSplashPointTime")
                .SetValue(Game1.timeOfDay);

            startBubblesTime = Game1.timeOfDay;



        }

        private static void DrawInMenu_Postfix(
            SObject __instance,
            SpriteBatch spriteBatch,
            Vector2 location,
            float scaleSize,
            float transparency,
            float layerDepth,
            Microsoft.Xna.Framework.Color color)
        {
            if (!Config.animatedTotem || __instance.QualifiedItemId !=
                "(O)Poplinp.BubbleTotem_BubbleTotem")
            {
                return;
            }

            Texture2D texture =
                Game1.content.Load<Texture2D>(
                    "Mods/Poplinp.BubbleTotem/Animation"
                );

            const int frameSize = 16;
            const int frameCount = 10;
            const double frameDurationMs = 80.0;

            int frame = (int)(
                Game1.currentGameTime.TotalGameTime.TotalMilliseconds
                / frameDurationMs
            ) % frameCount;

            Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(
                frame * frameSize,
                0,
                frameSize,
                frameSize
            );

            spriteBatch.Draw(
                texture,
                location + new Vector2(32f, 32f),
                sourceRect,
                color * transparency,
                0f,
                new Vector2(frameSize / 2f, frameSize / 2f),
                4f * scaleSize,
                SpriteEffects.None,
                layerDepth + 0.00001f
            );
        }


        private static void DrawWhenHeld_Postfix(
            SObject __instance,
            SpriteBatch spriteBatch,
            Vector2 objectPosition,
            Farmer f)
        {
            if (!Config.animatedTotem || __instance.QualifiedItemId !=
                "(O)Poplinp.BubbleTotem_BubbleTotem")
            {
                return;
            }

            Texture2D texture = Game1.content.Load<Texture2D>(
                "Mods/Poplinp.BubbleTotem/Animation"
            );

            const int frameSize = 16;
            const int frameCount = 10;
            const double frameDurationMs = 80.0;

            int frame = (int)(
                Game1.currentGameTime.TotalGameTime.TotalMilliseconds
                / frameDurationMs
            ) % frameCount;

            Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(
                frame * frameSize,
                0,
                frameSize,
                frameSize
            );

            float drawLayer = Math.Max(
                0f,
                (float)(f.StandingPixel.Y + 3) / 10000f
            );

            spriteBatch.Draw(
                texture,
                objectPosition,
                sourceRect,
                Microsoft.Xna.Framework.Color.White,
                0f,
                Vector2.Zero,
                4f,
                SpriteEffects.None,
                drawLayer + 0.00001f
            );
        }


        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            // register mod
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(Config)
            );

            // add some config options
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("ConfigDuration.Name"),
                tooltip: () => this.Helper.Translation.Get("ConfigDuration.Desc"),
                getValue: () => Config.bubblesLength,
                setValue: value => Config.bubblesLength = value,
                min: 10,
                max: 600,
                interval: 10
            );

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("ConfigMaxTile.Name"),
                tooltip: () => this.Helper.Translation.Get("ConfigMaxTile.Desc"),
                getValue: () => Config.bubblesMaxTile,
                setValue: value => Config.bubblesMaxTile = value,
                min: 2,
                max: 7,
                interval: 1
            );

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("ConfigAnimated.Name"),
                tooltip: () => this.Helper.Translation.Get("ConfigAnimated.Desc"),
                getValue: () => Config.animatedTotem,
                setValue: value => 
                {
                    if (Config.animatedTotem == value)
                        return;

                    Config.animatedTotem = value;

                    Helper.GameContent.InvalidateCache(
                        "Mods/Poplinp.BubbleTotem/Objects"
                    );
                }
            );
        }
    }
}
