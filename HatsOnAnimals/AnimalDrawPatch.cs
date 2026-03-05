using HarmonyLib;
using StardewValley;
using StardewValley.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using StardewModdingAPI;
using System.Collections.Generic;
using StardewValley.ItemTypeDefinitions;


namespace HatsOnAnimals
{

    public static class AnimalDrawPatch
    {

        public static Dictionary<string, AnimalDictData> animalDictsByType  = new Dictionary<string, AnimalDictData>
        {
            {"Chicken",  new AnimalDictData("Chicken", false, 1)},
            {"Rabbit",  new AnimalDictData("Rabbit", false, 1)},
            {"Dinosaur",  new AnimalDictData("Dinosaur", false)},
            {"Cow",  new AnimalDictData("Cow", false)},
            {"Goat",  new AnimalDictData("Goat", false)},
            {"Pig",  new AnimalDictData("Pig", false)},
            {"Sheep",  new AnimalDictData("Sheep", false)},
            {"Ostrich",  new AnimalDictData("Ostrich", false)}
        };

        public static Dictionary<string, AnimalDictData> animalBabyDictsByType = new Dictionary<string, AnimalDictData>
        {
            {"Chicken",  new AnimalDictData("Chicken", true, 1)},
            {"Rabbit",  new AnimalDictData("Rabbit", true, 1)},
            {"Dinosaur",  new AnimalDictData("Dinosaur", true)},
            {"Cow",  new AnimalDictData("Cow", true)},
            {"Goat",  new AnimalDictData("Goat", true)},
            {"Pig",  new AnimalDictData("Pig", true)},
            {"Sheep",  new AnimalDictData("Sheep", true)},
            {"Ostrich",  new AnimalDictData("Ostrich", true)}
        };


        internal static void TryDrawHat(FarmAnimal animal, SpriteBatch b)
        {
            if (animal == null || animal.modData == null)
                return;

            if (!animal.modData.TryGetValue("Poplinp.HatsOnAnimals.HatId", out string hatId))
                return;



            Hat hat = new Hat(hatId);
            if (!GetFrameOffsets(animal, out int x, out int y, out int direction))
                return;

            Rectangle boundingBox = animal.GetBoundingBox();
            float drawLayer = ((float)(boundingBox.Center.Y + 4) + animal.Position.X / 20000f) / 10000f + 1E-07f;

            Vector2 pos =
                Utility.snapDrawPosition(
                    animal.getLocalPosition(Game1.viewport)
                    + new Vector2(x, y)
                );

            hat.draw(
                b,
                pos,
                1.33333337f,
                1f,
                drawLayer,
                direction,
                true
            );


        }



        private static bool GetFrameOffsets(FarmAnimal animal, out int x, out int y, out int direction)
        {
            x = 0;
            y = 0;
            direction = 2;

            int frame = animal.Sprite.CurrentFrame;
            string type = animal.type.ToString();
            if (isChicken(type)) type = "Chicken";
            if (isCow(type)) type = "Cow";

            AnimalDictData animalData;

            if (animal.isBaby()) animalBabyDictsByType.TryGetValue(type, out animalData);
            else animalDictsByType.TryGetValue(type, out animalData);
            
            AnimalDicts.initOffsetData(animalData);

            if (!animalData.offsets.TryGetValue(frame, out var data))
                return true;

            if (data.disable)
                return false;

            bool flip = (animal.FacingDirection != data.direction);
            direction = animal.FacingDirection;


            if (flip)
            {
                x = data.flippedX;
                y = data.flippedY;


            }
            else
            {
                x = data.X;
                y = data.Y;
            }

            //fixing appearent expections

            if (animalData.fixes == 1)
            {
                if (direction == 3 && frame >= 22 && frame <= 23) direction = 1;
                if (direction == 3 && frame >= 20 && frame <= 21) direction = 0;
                if (frame >= 16 && frame <= 17) direction = 2;
            }



            return true;
        }

        public static bool isChicken(string type)
        {
            return (type == "White Chicken" || type == "Brown Chicken" || type == "Blue Chicken" || type == "Golden Chicken" || type == "Void Chicken");
        }

        public static bool isCow(string type)
        {
            return (type == "White Cow" || type == "Brown Cow");
        }

        public static void draw_Postfix(FarmAnimal __instance, SpriteBatch b)
        {
            TryDrawHat(__instance, b);
        }



        public static bool pet_Prefix(FarmAnimal __instance, Farmer who, bool is_auto_pet)
        {
            try
            {

                if (is_auto_pet)
                    return true;



                if (who.CurrentItem is not Hat hatItem)
                    return true;


                if (__instance.modData.TryGetValue("Poplinp.HatsOnAnimals.HatId", out string existingHatId))
                {
                    Game1.createItemDebris(new Hat(existingHatId), __instance.Position, -1);
                    __instance.modData.Remove("Poplinp.HatsOnAnimals.HatId");
                    Game1.playSound("dirtyHit");
                }


                __instance.modData["Poplinp.HatsOnAnimals.HatId"] = hatItem.ItemId;
                who.reduceActiveItemByOne(); 
                Game1.playSound("dirtyHit");

 
                return false;
            }
            catch (Exception ex)
            {
                ModEntry.StaticMonitor.Log($"Failed applying hat to farm animal:\n{ex}", LogLevel.Error);
                return true; 
            }
        }





    }

}

