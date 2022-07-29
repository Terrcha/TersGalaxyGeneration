using System.Diagnostics;
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
        private static Vector2 _mapSize = new Vector2(-500, 500);

        private static MapSetup _mapSetup = MapSetup.Medium;

        private static int _maxDepth = 10;
        private static int _maxNumberOfSystems = 1500;
        private static int _minNumberOfSystems = 500;

        private static Vector2[] _generatedSystems;
        private static int _numberOfSystems;

        private static List<System> _systems;

        static void Main()
        {
            
            
            SystemGeneration();
            GenerateSystems();
        }

        private static void System()
        {
            int id;
            string name;
            Vector2 pos;
            string initializer;
        }

        private static void SystemGeneration()
        {
            CheckUserInputForMapSize();
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

            _numberOfSystems = GetMapSize();
        }

        private static int GetMapSize()
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

        private static void GenerateSystems()
        {
            // Initialise
            _systems = new List<System>(_numberOfSystems);
            _generatedSystems = new Vector2[_numberOfSystems];
            
            for (int i = 0; i < _numberOfSystems; i++)
            {
                Random rnd = new Random();
                int systemX = rnd.Next((int)_mapSize.X, (int)_mapSize.Y);
                int systemY = rnd.Next((int)_mapSize.X, (int)_mapSize.Y);

                _generatedSystems[i] = new Vector2(systemX, systemY);
                System generatedSystem = new System()
                {
                    Id = i,
                    Name = "i",
                    Pos = _generatedSystems[i],
                    Initializer = $"{i}_sytem_initializer"
                };
                _systems.Add(generatedSystem);
            }
            
        }

        private void CheckForDuplicates()
        {
        }
    }

    public class System
    {
        public int Id;
        public string Name = "";
        public Vector2 Pos;
        public string Initializer = "";
    }
}