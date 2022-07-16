using System.Numerics;

namespace TersGalaxyGeneration
{
    public enum MapSetup
    {
        Tiny,
        Small,
        Medium,
        Large,
        Huge,
        Massive,
        Gargantuan
    }

    public class GalaxyGen
    {
        private Vector2 _minMapSize = new Vector2(-500, -500);
        private Vector2 _maxMapSize = new Vector2(500, 500);

        private static MapSetup _mapSetup = MapSetup.Medium;

        private int _maxDepth = 10;
        private int _maxNumberOfSystems = 1500;
        private int _minNumberOfSystems = 500;

        private Vector2[] _generatedSystems;

        static void Main()
        {
            SystemGeneration();
        }

        public static int GetMapSize()
        {
            int systemAmount = 0;
            switch (_mapSetup)
            {
                case MapSetup.Tiny:
                    systemAmount = 200;
                    break;
                case MapSetup.Small:
                    systemAmount = 400;
                    break;
                case MapSetup.Medium:
                    systemAmount = 600;
                    break;
                case MapSetup.Large:
                    systemAmount = 800;
                    break;
                case MapSetup.Huge:
                    systemAmount = 1000;
                    break;
                case MapSetup.Massive:
                    systemAmount = 1500;
                    break;
                case MapSetup.Gargantuan:
                    systemAmount = 2000;
                    break;
                default:
                    systemAmount = 1000;
                    break;
            }

            return systemAmount;
        }

        /// <summary>
        /// Check user input for which size galaxy should be generated.
        /// </summary>
        private static void CheckUserInputForMapSize()
        {
            
            Console.WriteLine("Choose which system size to generate");
            Console.WriteLine("\t1 - Tiny");
            Console.WriteLine("\t2 - Small");
            Console.WriteLine("\t3 - Medium");
            Console.WriteLine("\t4 - Large");
            Console.WriteLine("\t5 - Huge");
            Console.WriteLine("\t6 - Massive");
            Console.WriteLine("\t7 - Gargantuan");
            switch (Console.ReadLine())
            {
                case "1":
                    _mapSetup = MapSetup.Tiny;
                    Console.WriteLine("MapSetup set to Tiny");
                    break;
                case "2":
                    _mapSetup = MapSetup.Small;
                    Console.WriteLine("MapSetup set to Small");
                    break;
                case "3":
                    _mapSetup = MapSetup.Medium;
                    Console.WriteLine("MapSetup set to Medium");
                    break;
                case "4":
                    _mapSetup = MapSetup.Large;
                    Console.WriteLine("MapSetup set to Large");
                    break;
                case "5":
                    _mapSetup = MapSetup.Huge;
                    Console.WriteLine("MapSetup set to Huge");
                    break;
                case "6":
                    _mapSetup = MapSetup.Massive;
                    Console.WriteLine("MapSetup set to Massive");
                    break;
                case "7":
                    _mapSetup = MapSetup.Gargantuan;
                    Console.WriteLine("MapSetup set to Gargantuan");
                    break;
            }
        }

        private static void SystemGeneration()
        {
            CheckUserInputForMapSize();
            
            
        }
    }
}