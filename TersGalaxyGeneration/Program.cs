using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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
    
    public class System
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public Vector2 Pos { get; set; }
        public string Initializer { get; set; } = "";
    }

    public class Nebulae
    {
        public string Name { get; set; } = "";
        public Vector2 Pos { get; set; }
        public int Radius { get; set; }
    }

    public class NameGeneration
    {
        public string[] StarNames { get; set; } = new[] { "" };
        public string[] NebulaeNames { get; set; } = new[] { "" };
        public string[] BlackHoleNames { get; set; } = new[] { ""};

    }

    public class GalaxyGen
    {
        private static Vector2 _mapSize = new Vector2(-500, 500);
        private static MapSetup _mapSetup = MapSetup.Medium;
        private static int _maxDepth = 10;
        private static Vector2[] _generatedSystems = { };
        private static int _numberOfSystems;
        private static List<System> _systems = new List<System>() { };
        private static NameGeneration _nameGeneration;

        static void Main()
        {
            GetRandomNamesList();
            SystemGeneration();
            GenerateSystems(_numberOfSystems, true);

        }

        private static void GetRandomNamesList()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"\common\namegen.json";
            if (File.Exists(projectDirectory))
            {
                _nameGeneration = JsonConvert.DeserializeObject<NameGeneration>(File.ReadAllText(projectDirectory));
            }
            else
            {
                Console.WriteLine("Could not convert name generation json to class");
                return;
            }
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

        private static void GenerateSystems(int numberOfSystemToGenerate, bool initializeLists)
        {
            if (initializeLists)
            {
                // Initialise
                _systems = new List<System>(_numberOfSystems);
                _generatedSystems = new Vector2[_numberOfSystems];
            }
            

            for (int i = 0; i < numberOfSystemToGenerate; i++)
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
                    Initializer = $"{i}_system_initializer"
                };
                _systems.Add(generatedSystem);
            }

            CheckForDuplicates();
        }

        private static void CheckForDuplicates()
        {
            Console.WriteLine("Checking for duplicated systems coords");
            
            Vector2[] systemPos = new Vector2[_systems.Count];

            for (int i = 0; i < _systems.Count; i++)
            {
                systemPos[i] = _systems[i].Pos;
            }

            var duplicateSearch = systemPos
                .Select((pos, index) => new { pos, index })
                .GroupBy(x => x.pos)
                .Select(xg => new
                {
                    pos = xg.Key,
                    Indices = xg.Select(x => x.index)
                })
                .Where(x => x.Indices.Count() > 1);

            var enumerable = duplicateSearch.ToList();
            int numberOfHits = enumerable.ToArray().Length;
            Console.WriteLine($"{numberOfHits} duplicates have been");
            
            if (numberOfHits > 0)
            {
                int diplayDupesCount = 0;
                int[] duplicatedIndices = new int[] { };
                Console.WriteLine("Displaying duplicate systems");
                foreach (var g in enumerable)
                {
                    diplayDupesCount++;
                    duplicatedIndices = g.Indices.ToArray();
                    Console.WriteLine("########################");
                    for (int i = 0; i < duplicatedIndices.Length; i++)
                    {
                    
                        Console.WriteLine($"{diplayDupesCount}: duplicate indices are {duplicatedIndices[i]}");
                        Console.WriteLine($"{diplayDupesCount}: Position is {g.pos}");
                    }
                    Console.WriteLine("########################");
                
                    Console.WriteLine("Removing those systems now");
                    Thread.Sleep(300);
                
                }
                
                RemoveDuplicateSystems(duplicatedIndices);
            }
            else
            {
                Console.WriteLine("No duplicates have been found, proceeding to next step");
            }
        }

        private static void RemoveDuplicateSystems(int[] indicesToRemove)
        {
            for (int i = indicesToRemove.Length - 1; i >= 0; i--)
            {
                _systems.RemoveAt(indicesToRemove[i]);
                Console.WriteLine(_systems.Count);
            }
            
            GenerateSystems(_numberOfSystems - _systems.Count, false);
        }
    }

  
}