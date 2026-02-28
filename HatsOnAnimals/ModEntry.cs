using StardewModdingAPI;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Utilities;
using System.Linq;
using System;


namespace HatsOnAnimals
{
    public class ModEntry : Mod
    {
        internal static IMonitor StaticMonitor;
        internal const string HatKey = "HatsOnAnimals.HatId";

        public static Texture2D RemoveHatTexture;

        private static readonly PerScreen<ClickableTextureComponent> removeHatButton = new(() => null);


        public override void Entry(IModHelper helper)
        {
            StaticMonitor = Monitor;
            RemoveHatTexture = helper.ModContent.Load<Texture2D>("assets/HatButton.png");

            

            var harmony = new Harmony(ModManifest.UniqueID);


            harmony.Patch(
                original: AccessTools.Method(
                    typeof(FarmAnimal),
                    nameof(FarmAnimal.draw),
                    new Type[] { typeof(SpriteBatch) } 
                ),
                postfix: new HarmonyMethod(typeof(AnimalDrawPatch), nameof(AnimalDrawPatch.draw_Postfix))
            );

            harmony.Patch(
                original: AccessTools.Method(
                    typeof(AnimalQueryMenu),
                    nameof(AnimalQueryMenu.draw),
                    new Type[] { typeof(SpriteBatch) }
                ),
                postfix: new HarmonyMethod(typeof(AnimalQueryMenuPatch), nameof(AnimalQueryMenuPatch.draw_Postfix))
            );

            harmony.Patch(
                original: AccessTools.Method(typeof(FarmAnimal), nameof(FarmAnimal.pet)),
                prefix: new HarmonyMethod(typeof(AnimalDrawPatch), nameof(AnimalDrawPatch.pet_Prefix))
            );


            harmony.Patch(
                AccessTools.Constructor(typeof(AnimalQueryMenu), new[] { typeof(FarmAnimal) }),
                postfix: new HarmonyMethod(typeof(AnimalQueryMenuPatch), nameof(AnimalQueryMenuPatch.constructor_Postfix))
            );

            harmony.Patch(
                AccessTools.Method(typeof(AnimalQueryMenu), "receiveLeftClick"),
                postfix: new HarmonyMethod(typeof(AnimalQueryMenuPatch), nameof(AnimalQueryMenuPatch.receiveLeftClick_Postfix))
            );

            harmony.Patch(
                AccessTools.Method(typeof(AnimalQueryMenu), "performHoverAction"),
                postfix: new HarmonyMethod(typeof(AnimalQueryMenuPatch), nameof(AnimalQueryMenuPatch.performHover_Postfix))
            );


        }
    }
}
