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


namespace BubbleTotem
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        private static IMonitor StaticMonitor = null!;

        private static int startBubblesTime = -1;
        private static int bubblesLength = 30;

        private static GameLocation? bubblesLocation = null;
        private static Microsoft.Xna.Framework.Point? totemBubblePoint = null;

        private static IReflectionHelper? Reflection;

        /*********
        ** Public methods
        *********/

        public override void Entry(IModHelper helper)
        {
            StaticMonitor = Monitor;
            Reflection = helper.Reflection;

            helper.Events.Content.AssetRequested += OnAssetRequested;
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;

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
                        DisplayName = "Bubble Totem",
                        Description = "desc tbd",
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
                e.LoadFromModFile<Texture2D>(
                    "assets/bubble-totem.png",
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
                ? 7  // Horizontal
                : 6; // Vertical




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

            if (durationSoFar < bubblesLength)
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
    }
}
