using System.Collections.Generic;
using StardewModdingAPI;

namespace HatsOnAnimals
{

    public static class AnimalDicts
    {
        public static void initOffsetData (AnimalDictData animalData)
        {
            if (animalData.initialized) return;

            if (animalData.type == "Chicken")
            {
                if (animalData.isBaby)
                {
                    // DOWN
                    animalData.Add4FrameAnimation(0, -8, -30, 2, -4, -30);
                    // unique down
                    animalData.AddFrame(16, -8, -26, 2, -4, -26);
                    animalData.AddFrame(17, -8, -22, 2, -4, -22);
                    // eating
                    animalData.CloneFrame(24, 16);
                    animalData.CloneFrame(25, 17);
                    animalData.AddFrame(26, -8, -18, 2, -4, -18);
                    animalData.CloneFrame(27, 17);


                    // RIGHT
                    animalData.Add4FrameAnimation(4, 2, -26, 1, -10, -26);
                    // unique right
                    animalData.CloneFrame(18, 4);
                    animalData.AddFrame(19, 2, -22, 1, -10, -22);

                    // UP
                    animalData.Add4FrameAnimation(8, -8, -46, 0, -4, -46);
                    // unique up
                    animalData.CloneFrame(20, 8);
                    animalData.CloneFrame(21, 8);

                    // LEFT
                    animalData.Add4FrameAnimation(12, 2, -26, 3, -10, -26);
                    // unique left
                    animalData.AddFrame(22, 2, -30, 3, -10, -30);
                    animalData.CloneFrame(23, 12);

                    animalData.DoubleFrames();
                    return;
                }

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
                return;
            }

            else if (animalData.type == "Duck")
            {
                if (animalData.isBaby)
                {
                    // DOWN
                    animalData.Add4FrameAnimation(0, -8, -30, 2, -4, -30);
                    // unique down
                    animalData.AddFrame(16, -8, -26, 2, -4, -26);
                    animalData.AddFrame(17, -8, -22, 2, -4, -22);
                    // eating
                    animalData.CloneFrame(24, 16);
                    animalData.CloneFrame(25, 17);
                    animalData.AddFrame(26, -8, -18, 2, -4, -18);
                    animalData.CloneFrame(27, 17);


                    // RIGHT
                    animalData.Add4FrameAnimation(4, 2, -26, 1, -10, -26);
                    // unique right
                    animalData.CloneFrame(18, 4);
                    animalData.AddFrame(19, 2, -22, 1, -10, -22);

                    // UP
                    animalData.Add4FrameAnimation(8, -8, -46, 0, -4, -46);
                    // unique up
                    animalData.CloneFrame(20, 8);
                    animalData.CloneFrame(21, 8);

                    // LEFT
                    animalData.Add4FrameAnimation(12, 2, -26, 3, -10, -26);
                    // unique left
                    animalData.AddFrame(22, 2, -30, 3, -10, -30);
                    animalData.CloneFrame(23, 12);

                    animalData.DoubleFrames();
                    return;
                }
                // DOWN
                animalData.Add4FrameAnimation(0, -14, -58, 2, 2, -58);
                // unique down
                animalData.AddFrame(16, -14, -50, 2, 2, -50);
                animalData.CloneFrame(17, 16);
                // eating
                animalData.CloneFrame(24, 16);
                animalData.AddFrame(25, -14, -42, 2, 2, -42);
                animalData.AddFrame(26, -14, -26, 2, 2, -26);
                animalData.AddFrame(27, -14, -30, 2, 2, -30);

                // RIGHT
                animalData.Add4FrameAnimation(4, 2, -50, 1, -14, -50);
                // unique right
                animalData.AddFrame(18, 2, -46, 1, -14, -46);
                animalData.AddFrame(19, 2, -42, 1, -14, -42);

                // UP
                animalData.Add4FrameAnimation(8, 6, -58, 0, -18, -58);
                // unique up
                animalData.AddFrame(20, 6, -54, 0, -18, -54);
                animalData.AddFrame(21, 6, -50, 0, -18, -50);

                // LEFT
                animalData.Add4FrameAnimation(12, 2, -50, 3, -14, -50);
                // unique left
                animalData.AddFrame(22, 2, -50, 3, -14, -50);
                animalData.AddFrame(23, 2, -50, 3, -14, -50);

                animalData.DoubleFrames();
                return;
            }

            else if (animalData.type == "Rabbit")
            {
                if (animalData.isBaby)
                {
                    // DOWN
                    animalData.Add4FrameAnimation(0, -6, -30, 2);
                    // unique down
                    animalData.CloneFrame(16, 0);
                    animalData.CloneFrame(17, 0);
                    // eating
                    animalData.CloneFrame(24, 0);
                    animalData.AddFrame(25, -6, -26, 2);
                    animalData.CloneFrame(26, 25);
                    animalData.CloneFrame(27, 25);

                    // RIGHT 
                    animalData.Add4FrameAnimation(4, 6, -22, 1, -18, -22);
                    // unique right
                    animalData.CloneFrame(18, 4);
                    animalData.CloneFrame(19, 4);

                    // UP
                    animalData.Add4FrameAnimation(8, -6, -50, 0);
                    // unique up
                    animalData.CloneFrame(20, 8);
                    animalData.CloneFrame(21, 8);

                    // LEFT
                    animalData.Add4FrameAnimation(12, 6, -22, 3, -18, -22);
                    // unique left
                    animalData.CloneFrame(22, 12);
                    animalData.CloneFrame(23, 12);
                    return;
                }

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
                animalData.Add4FrameAnimation(12, 10, -34, 3, -22, -34);
                // unique left
                animalData.CloneFrame(22, 12);
                animalData.CloneFrame(23, 12);
                return;
            }

            else if (animalData.type == "Dinosaur")
            {
                // DOWN
                animalData.Add4FrameAnimation(0, -6, -38, 2);
                // unique down
                animalData.CloneFrame(16, 0);
                animalData.CloneFrame(17, 0);
                // eating
                animalData.CloneFrame(24, 0);
                animalData.AddFrame(25, -8, -34, 2);
                animalData.AddFrame(26, -8, -30, 2);
                animalData.CloneFrame(27, 25);

                // RIGHT 
                animalData.Add4FrameAnimation(4, 10, -34, 1, -22, -34);
                // unique right
                animalData.AddFrame(18, 10, -26, 1, -22, -26);
                animalData.CloneFrame(19, 18);

                // UP
                animalData.Add4FrameAnimation(8, -6, -66, 0);
                // unique up
                animalData.AddFrame(20, -6, 0, -62, 0);
                animalData.CloneFrame(21, 8);

                // LEFT
                animalData.Add4FrameAnimation(12, 10, -34, 3, -22, -34);
                // unique left
                animalData.AddFrame(22, 10, -26, 3, -22, -26);
                animalData.CloneFrame(23, 22);
                return;
            }

            else if (animalData.type == "Cow")
            {
                if (animalData.isBaby)
                {
                    // DOWN
                    animalData.Add4FrameAnimation(0, 29, 10, 2, 25, 10);
                    // unique down
                    animalData.CloneFrame(12, 0);
                    animalData.CloneFrame(13, 0);
                    // eating
                    animalData.AddFrame(16, 29, 14, 2, 25, 14);
                    animalData.AddFrame(17, 29, 22, 2, 25, 22);
                    animalData.AddFrame(18, 29, 26, 2, 25, 26);
                    animalData.AddFrame(19, 29, 30, 2, 25, 30);

                    // RIGHT 
                    animalData.Add4FrameAnimation(4, 55, 6, 1, -1, 6);
                    // unique right
                    animalData.CloneFrame(14, 4);

                    // UP
                    animalData.Add4FrameAnimation(8, 29, -30, 0, 25, -30);
                    // unique up
                    animalData.AddFrame(15, 29, -26, 0, 25, -26);
                    return;
                }

                // DOWN
                animalData.Add4FrameAnimation(0, 29, 10, 2, 25, 10);
                // unique down
                animalData.CloneFrame(12, 0);
                animalData.CloneFrame(13, 0);
                // eating
                animalData.CloneFrame(16, 0);
                animalData.AddFrame(17, 29, 14, 2, 25, 14);
                animalData.AddFrame(18, 29, 18, 2, 25, 18);
                animalData.CloneFrame(19, 18);

                // RIGHT 
                animalData.Add4FrameAnimation(4, 63, -2, 1, -9, -2);
                // unique right
                animalData.CloneFrame(14, 4);

                // UP
                animalData.Add4FrameAnimation(8, 29, -38, 0, 25, -38);
                // unique up
                animalData.CloneFrame(15, 8);
                return;


            }

            else if (animalData.type == "Goat")
            {
                if (animalData.isBaby)
                {
                    // DOWN
                    animalData.Add4FrameAnimation(0, 29, 14, 2, 25, 14);
                    // unique down
                    animalData.AddFrame(12, 29, 18, 2, 25, 18);
                    animalData.CloneFrame(13, 12);
                    // eating
                    animalData.CloneFrame(16, 12);
                    animalData.AddFrame(17, 29, 22, 2, 25, 22);
                    animalData.AddFrame(18, 29, 26, 2, 25, 26);
                    animalData.CloneFrame(19, 18);

                    // RIGHT 
                    animalData.Add4FrameAnimation(4, 67, 10, 1, -13, 10);
                    // unique right
                    animalData.CloneFrame(14, 4);

                    // UP
                    animalData.Add4FrameAnimation(8, 29, -26, 0, 25, -26);
                    // unique up
                    animalData.AddFrame(15, 29, -22, 0, 25, -22);
                    return;
                }

                // DOWN
                animalData.Add4FrameAnimation(0, 29, -6, 2, 25, -6);
                // unique down
                animalData.CloneFrame(12, 0);
                animalData.CloneFrame(13, 0);
                // eating
                animalData.CloneFrame(16, 0);
                animalData.AddFrame(17, 29, 2, 2, 25, 2);
                animalData.AddFrame(18, 29, 10, 2, 25, 10);
                animalData.CloneFrame(19, 18);

                // RIGHT 
                animalData.Add4FrameAnimation(4, 63, -10, 1, -9, -10);
                // unique right
                animalData.AddFrame(14, 63, -6, 1, -9, -6);

                // UP
                animalData.Add4FrameAnimation(8, 29, -34, 0, 25, -34);
                // unique up
                animalData.AddFrame(15, 29, -30, 0, 25, -30);
                return;
            }

            else if (animalData.type == "Pig")
            {
                if (animalData.isBaby)
                {
                    // DOWN
                    animalData.Add4FrameAnimation(0, 29, 14, 2, 25, 14);
                    // unique down
                    animalData.CloneFrame(12, 0);
                    animalData.CloneFrame(13, 0);
                    // eating
                    animalData.AddFrame(16, 29, 18, 2, 25, 18);
                    animalData.AddFrame(17, 29, 22, 2, 25, 22);
                    animalData.AddFrame(18, 29, 26, 2, 25, 26);
                    animalData.AddFrame(19, 29, 30, 2, 25, 30);

                    // RIGHT 
                    animalData.Add4FrameAnimation(4, 67, 6, 1, -13, 6);
                    // unique right
                    animalData.CloneFrame(14, 4);

                    // UP
                    animalData.Add4FrameAnimation(8, 31, -26, 0, 23, -26);
                    // unique up
                    animalData.AddFrame(15, 31, -34, 0, 23, -34);
                    return;
                }

                // DOWN
                animalData.Add4FrameAnimation(0, 29, 10, 2, 25, 10);
                // unique down
                animalData.AddFrame(12, 29, 14, 2, 25, 14);
                animalData.CloneFrame(13, 12);
                // eating
                animalData.CloneFrame(16, 0);
                animalData.AddFrame(17, 29, 18, 2, 25, 18);
                animalData.AddFrame(18, 29, 22, 2, 25, 22);
                animalData.CloneFrame(19, 18);

                // RIGHT 
                animalData.Add4FrameAnimation(4, 67, 6, 1, -13, 6);
                // unique right
                animalData.CloneFrame(14, 4);

                // UP
                animalData.Add4FrameAnimation(8, 29, -46, 0, 25, -46);
                // unique up
                animalData.CloneFrame(15, 8);
                return;
            }

            else if (animalData.type == "Sheep")
            {
                if (animalData.isBaby)
                {
                    // DOWN
                    animalData.Add4FrameAnimation(0, 29, 14, 2, 25, 14);
                    // unique down
                    animalData.AddFrame(12, 29, 18, 2, 25, 18);
                    animalData.CloneFrame(13, 12);
                    // eating
                    animalData.CloneFrame(16, 12);
                    animalData.AddFrame(17, 29, 22, 2, 25, 22);
                    animalData.AddFrame(18, 29, 26, 2, 25, 26);
                    animalData.CloneFrame(19, 18);

                    // RIGHT 
                    animalData.Add4FrameAnimation(4, 67, 14, 1, -13, 14);
                    // unique right
                    animalData.CloneFrame(14, 4);

                    // UP
                    animalData.Add4FrameAnimation(8, 29, -26, 0, 25, -26);
                    // unique up
                    animalData.AddFrame(15, 29, -22, 0, 25, -22);
                    return;
                }

                // DOWN
                animalData.Add4FrameAnimation(0, 29, -2, 2, 25, -2);
                // unique down
                animalData.CloneFrame(12, 0);
                animalData.CloneFrame(13, 0);
                // eating
                animalData.AddFrame(16, 29, 2, 2, 25, 2);
                animalData.AddFrame(17, 29, 6, 2, 25, 6);
                animalData.AddFrame(18, 29, 10, 2, 25, 10);
                animalData.CloneFrame(19, 18);

                // RIGHT 
                animalData.Add4FrameAnimation(4, 67, 10, 1, -13, 10);
                // unique right
                animalData.AddFrame(14, 67, 14, 1, -13, 14);

                // UP
                animalData.Add4FrameAnimation(8, 29, -42, 0, 25, -42);
                // unique up
                animalData.CloneFrame(15, 8);
                return;
            }

            else if (animalData.type == "Ostrich")
            {
                if (animalData.isBaby)
                {
                    // DOWN
                    animalData.Add4FrameAnimation(0, 29, -10, 2, 25, -10);
                    // unique down
                    animalData.CloneFrame(12, 0);
                    animalData.AddFrame(13, 29, -6, 2, 25, -6);
                    // eating
                    animalData.AddFrame(16, 29, 14, 2, 25, 14);
                    animalData.AddFrame(17, 29, 34, 2, 25, 34);
                    animalData.AddFrame(18, 29, 38, 2, 25, 38);
                    animalData.CloneFrame(19, 18);

                    // RIGHT 
                    animalData.Add4FrameAnimation(4, 43, -6, 1, 11, -6);
                    // unique right
                    animalData.CloneFrame(14, 4);

                    // UP
                    animalData.Add4FrameAnimation(8, 29, -18, 0, 25, -18);
                    // unique up
                    animalData.AddFrame(15, 35, -18, 0, 19, -18);
                    return;
                }

                // DOWN
                animalData.Add4FrameAnimation(0, 27, -58, 2);
                // unique down
                animalData.CloneFrame(12, 0);
                animalData.CloneFrame(13, 0);
                // eating
                animalData.AddFrame(16, 27, -26, 2);
                animalData.AddFrame(17, 27, 14, 2);
                animalData.AddFrame(18, 27, 34, 2);
                animalData.AddFrame(19, 27, 30, 2);

                // RIGHT 
                animalData.AddFrame(4, 59, -54, 1, -5, -54);
                animalData.AddFrame(5, 63, -54, 1, -9, -54);
                animalData.CloneFrame(6, 4);
                animalData.CloneFrame(7, 5);
                // unique right
                animalData.CloneFrame(14, 4);

                // UP
                animalData.Add4FrameAnimation(8, 27, -62, 0);
                // unique up
                animalData.AddFrame(15, 31, -62, 0, 23, -62);
                return;
            }

            animalData.initialized = true;




        }

    }
}