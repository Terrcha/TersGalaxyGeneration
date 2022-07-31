using System.Numerics;
using Newtonsoft.Json;

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
    
    public class GalaxyOptions
    {
        public string MapName = "test";
        public string OptDefault = "no";
        public string RandomHyperlanes = "yes";

        public float ColonizablePlanetOdds = 1.0f;
        public float PrimitiveOdds = 1.0f;
        public float CrisisStrength = 1.0f;

        public int Priority = 0;
        public int MaxNumberOfEmpires = 60;
        public int MinNumberOfEmpires = 0;
        public int DefaultNumberOfEmpires = 20;
        public int DefaultNumberOfFallenEmpires = 4;
        public int ManxNumberOfFallenEmpires = 4;
        public int NumberOfAdvancedAi = 7;
        public int CoreRadius = 0;
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
        public string[] BlackHoleNames { get; set; } = new[] { "" };
    }

    public class GalaxyGen
    {
        private static Vector2 _mapSize = new Vector2(-400, 400);
        private static MapSetup _mapSetup = MapSetup.Medium;
        private static int _maxDepth = 10;
        private static Vector2[] _generatedSystems = { };
        private static int _numberOfSystems;
        private static List<System> _systems = new List<System>() { };
        private static NameGeneration _nameGeneration;
        private static string _directory;

        static void Main()
        {
            _directory = Environment.CurrentDirectory;

            Directory.CreateDirectory(_directory + @"/GeneratedMaps");
            
            GetRandomNamesList();
            SystemGeneration();
            GenerateSystems(_numberOfSystems, true);
            
            WriteToFile();
        }

        private static void GetRandomNamesList()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory =
                Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"\common\namegen.json";
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
                int starNameIndex = rnd.Next(0, _nameGeneration.StarNames.Length);
                string starName = _nameGeneration.StarNames[starNameIndex];
                int indexToRemove = starNameIndex;
                _nameGeneration.StarNames = _nameGeneration.StarNames.Where((source, index) => index != indexToRemove).ToArray();
                _generatedSystems[i] = new Vector2(systemX, systemY);
                System generatedSystem = new System()
                {
                    Id = i,
                    Name = starName,
                    Pos = _generatedSystems[i],
                    Initializer = $"{starName}_system_initializer"
                };
                _systems.Add(generatedSystem);
            }

            CheckForDuplicates();
        }

        private static void CheckForDuplicates()
        {
            Console.WriteLine("Checking for duplicated systems");

            Vector2[] systemPos = new Vector2[_systems.Count];
            string[] systemName = new string[_systems.Count];

            for (int i = 0; i < _systems.Count; i++)
            {
                systemPos[i] = _systems[i].Pos;
                systemName[i] = _systems[i].Name;
            }
            
            SystemPosDuplicateCheck(systemPos);
        }
        
        private static void SystemPosDuplicateCheck(Vector2[] systemPos)
        {
        var duplicatePosSearch = systemPos
                .Select((pos, index) => new { pos, index })
                .GroupBy(x => x.pos)
                .Select(xg => new
                {
                    pos = xg.Key,
                    indices = xg.Select(x => x.index)
                })
                .Where(x => x.indices.Count() > 1);

            var enumerable = duplicatePosSearch.ToList();
            int numberOfHits = enumerable.ToArray().Length;
            Console.WriteLine($"{numberOfHits} duplicates have been");

            if (numberOfHits > 0)
            {
                int displayDupesCount = 0;
                int[] duplicatedIndices = new int[] { };
                Console.WriteLine("Displaying duplicate systems");
                foreach (var g in enumerable)
                {
                    displayDupesCount++;
                    duplicatedIndices = g.indices.ToArray();
                    Console.WriteLine("########################");
                    for (int i = 0; i < duplicatedIndices.Length; i++)
                    {
                        Console.WriteLine($"{displayDupesCount}: duplicate indices are {duplicatedIndices[i]}");
                        Console.WriteLine($"{displayDupesCount}: Position is {g.pos}");
                    }

                    Console.WriteLine("########################");

                    Console.WriteLine("Removing those systems now");
                    Thread.Sleep(300);
                }

                RemoveDuplicateSystems(duplicatedIndices);
            }
            Console.WriteLine("No duplicates have been found, proceeding to next step");
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

        private static void WriteToFile()
        {
            using (StreamWriter sw = new StreamWriter(@"D:\Repo\TersGalaxyGeneration\TersGalaxyGeneration\GeneratedMaps\test.txt"))
            {
                sw.Write("static_galaxy_scenario = {");
                sw.Write("\n\tname = test");
                sw.Write("\n\tpriority = 0");
                sw.Write("\n\tdefault = no");
                sw.Write("\n\tcolonizable_planet_odds = 1.0");
                sw.Write("\n\tnum_empires = { min = 0 max = 60 }");
                sw.Write("\n\tnum_empire_default = 21");
                sw.Write("\n\tfallen_empire_default = 4");
                sw.Write("\n\tfallen_empire_max = 4");
                sw.Write("\n\tadvanced_empire_default = 7");
                sw.Write("\n\trandom_hyperlanes = yes");
                sw.Write("\n\tprimitive_odds = 1.0");
                sw.Write("\n\tcrisis_strength = 1.0");

                for (int i = 0; i < _numberOfSystems; i++)
                {
                    sw.Write("\n\n\tsystem = {");
                    sw.Write($"\n\t\tid = {_systems[i].Id}");
                    sw.Write("\n\t\tposition = {");
                    sw.Write($"\n\t\t\tx = {_systems[i].Pos.X}");
                    sw.Write($"\n\t\t\ty = {_systems[i].Pos.Y}");
                    sw.Write("\n\t\t}");
                    sw.Write("\n\t}");
                }
                
                sw.Write("\n}");
            }
        }
    }
}