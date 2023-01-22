namespace Zombono
{
    /// <summary>
    /// MapPrototype
    /// 
    /// Prototype/dummy map file (.zmp) loading test
    /// </summary>
    internal class MapPrototypeScene : Scene
    {
        public MapFile TestMap { get; set; }

        public MapPrototypeScene()
        {
            TestMap = new(@"Content\Maps\test.zmap");
        }

        public override void Start()
        {
            NCLogging.Log("Zombono Version 0.00 - Map Test scene, loading test map...");

            TestMap.Read();

            if (!TestMap.Loaded)
            {
                NCLogging.Log("TESTMAP not loaded. shutting down [Temporary for v0.00 only]", ConsoleColor.Red);
                Lightning.Shutdown();
            }


        }

        public override void Shutdown()
        {

        }

        public override void SwitchTo(Scene oldScene)
        {
            // again temporary (insert these into the map lol, just to display them)
            for (int tileId = 0; tileId < TestMap.Tiles.Count; tileId++)
            {
                ZbMapTile mapTile = TestMap.Tiles[tileId];

                int row = tileId / TestMap.Header.MapHeight;

                // set position
                mapTile.Position = new(0 + (SharedAssets.SPRITESHEET_TILE_WIDTH * (tileId % SharedAssets.SPRITESHEET_SHEET_WIDTH)), (SharedAssets.SPRITESHEET_TILE_HEIGHT / 2) * row);

                // add it
                Lightning.Renderer.AddRenderable(mapTile); // this just draws a texture atlas a lot of times
            }

            Lightning.Renderer.SetCurrentCamera(new Camera(CameraType.Follow));

            Lightning.Renderer.AddRenderable(new TextBlock("DemoText", "Map Prototype Scene - this is a demonstration of the map loader", "DebugFont", new(300, 300), Color.Yellow));

            // demo build only
            Lightning.Renderer.Settings.Camera.Position = new(3100, 408);
        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Render()
        {
           Lightning.Renderer.Settings.Camera.Position = new(Lightning.Renderer.Settings.Camera.Position.X - (100 / (float)Lightning.Renderer.DeltaTime),
                Lightning.Renderer.Settings.Camera.Position.Y - (100 / (float)Lightning.Renderer.DeltaTime));
        }
    }
}
