using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;


namespace HatsOnAnimals;

public static class AnimalQueryMenuPatch
{
    private const string HatKey = "Poplinp.HatsOnAnimals.HatId";
    private const int RemoveHatButtonID = 1001;


    private static readonly ConditionalWeakTable<AnimalQueryMenu, ClickableTextureComponent> Buttons
        = new();

    public static void constructor_Postfix(AnimalQueryMenu __instance)
    {
        var animalField = AccessTools.Field(typeof(AnimalQueryMenu), "animal");
        FarmAnimal animal = (FarmAnimal)animalField.GetValue(__instance);

        if (animal == null)
            return;

        if (!animal.modData.ContainsKey(HatKey))
            return;

        var removeHatButton = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(__instance.xPositionOnScreen + 384 + 64+4, __instance.yPositionOnScreen + 512 - 256 - IClickableMenu.borderWidth, 64, 64), ModEntry.RemoveHatTexture, new Rectangle(0, 0, 16, 16), 4f)
        {
            myID = 1001,
            leftNeighborID = 104,
        };

        Buttons.Add(__instance, removeHatButton);

    }

    public static void receiveLeftClick_Postfix(AnimalQueryMenu __instance, int x, int y)
    {

        if (!Buttons.TryGetValue(__instance, out ClickableTextureComponent removeHatButton))
            return;

        if (!removeHatButton.containsPoint(x, y))
            return;

        var animalField = AccessTools.Field(typeof(AnimalQueryMenu), "animal");
        FarmAnimal animal = (FarmAnimal)animalField.GetValue(__instance);

        if (animal == null)
            return;

        if (!animal.modData.TryGetValue(HatKey, out string hatId))
            return;

        StardewValley.Objects.Hat hat = new StardewValley.Objects.Hat(hatId);
        Game1.createItemDebris(hat, animal.Position, -1);
        animal.modData.Remove(HatKey);

        Game1.playSound("dirtyHit");

        Game1.exitActiveMenu();
    }


    public static void performHover_Postfix(AnimalQueryMenu __instance, int x, int y)
    {
        if (!Buttons.TryGetValue(__instance, out var button))
            return;

        if (button.containsPoint(x, y))
            button.scale = Math.Min(4.1f, button.scale + 0.05f);
        else
            button.scale = Math.Max(4f, button.scale - 0.05f);
    }



    public static void draw_Postfix(AnimalQueryMenu __instance, SpriteBatch b)
    {
        if (Buttons.TryGetValue(__instance, out var button))
        {
            button.draw(b);
            __instance.drawMouse(b);
        }
    }





}


