namespace Zombono.Scenes
{
    /// <summary>
    /// InitScene
    /// 
    /// Initialises zombono
    /// </summary>
    internal class InitScene : Scene
    {
        public override void Start()
        {
            // probably better to put this in start
            NCLogging.Log("Zombono SharedAssets Loading...");
            SharedAssets.Load();

            string fileName = "test.zmap";

            if (!File.Exists(fileName))
            {
                NCLogging.Log("MapPrototypeScene needs test.zmap. It doesn't exist, so generating it.");
                // Temporary: create a test map
                MapFile mapFile = new(fileName);
                mapFile.Header.MapWidth = 64;
                mapFile.Header.MapHeight = 64;
                mapFile.Header.MapName = "Zombono Version 0.00 Test Map 2022/12/29";
                mapFile.Header.MapAuthor = "starfrost";
                mapFile.Header.MapDescription = "A test map to test the map loader. The first map ever made for this game";

                for (int mapId = 0; mapId < 64 * 64; mapId++)
                {
                    mapFile.Tiles.Add(new("zbMapTile", (ushort)Random.Shared.Next(0, 16)));
                }

                mapFile.Write();
            }

        }

        public override void Shutdown()
        {

        }

        public override void SwitchTo(Scene oldScene)
        {

        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Render()
        {
            NCLogging.Log("Going to MapPrototypeScene");
            // the plan here is to show some logos and stuff
            // so this is not the only line of code that will ever be here

            
            Lightning.Client.SetCurrentScene("MapPrototypeScene");
        }
    }
}