using System.Collections.Generic;
using StardewModdingAPI;

namespace HatsOnAnimals
{

    public static class AnimalDicts
    {

        public static void InitChickenOffsetData(AnimalDictData animalData)
        {
            // DOWN
            animalData.Add4FrameAnimation(0, -14, -54, 2);
            // unique down
            animalData.AddFrame(16, -14, -50, 2);
            animalData.CloneFrame(17, 16);
            // eating
            animalData.AddFrame(24, -10, -42, 2);
            animalData.CloneFrame(25, 24);
            animalData.CloneFrame(26, 24);
            animalData.CloneFrame(27, 24);

            // RIGHT 
            animalData.Add4FrameAnimation(4, 6, -50, 1, -18, -50);
            // unique right
            animalData.AddFrame(18, 6, -46, 1, -18, -46);
            animalData.CloneFrame(19, 18);

            // UP
            animalData.Add4FrameAnimation(8, -2, -58, 0, -10, -58);
            // unique up
            animalData.AddFrame(20, -2, -54, 0, -10, -54);
            animalData.CloneFrame(21, 20);
            

            // LEFT
            animalData.Add4FrameAnimation(12, 6, -50, 3, -18, -50);
            // unique left
            animalData.AddFrame(22, 6, -46, 3, -18, -46);
            animalData.CloneFrame(23, 22);


            animalData.initialized = true;

        }

        public static void InitRabbitOffsetData(AnimalDictData animalData)
        {
            // DOWN
            animalData.Add4FrameAnimation(0, -8, -30, 2, -4, -30);
            // unique down
            animalData.CloneFrame(16, 0);
            animalData.CloneFrame(17, 0);
            // eating
            animalData.CloneFrame(24, 0);
            animalData.AddFrame(25, -8, -26, 2, -4, -26);
            animalData.AddFrame(26, -8, -22, 2, -4, -22);
            animalData.CloneFrame(27, 26);

            // RIGHT 
            animalData.Add4FrameAnimation(4, 10, -34, 1, -22, -34);
            // unique right
            animalData.CloneFrame(18, 4);
            animalData.CloneFrame(19, 4);

            // UP
            animalData.Add4FrameAnimation(8, -8, -50, 0, -4, -50);
            // unique up
            animalData.CloneFrame(20, 8);
            animalData.CloneFrame(21, 8);

            // LEFT
            animalData.Add4FrameAnimation(12, -22, -34, 3, 10, -34);
            // unique left
            animalData.CloneFrame(22, 12);
            animalData.CloneFrame(23, 12);

            animalData.initialized = true;

        }
    }
}