using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zombono.Scenes
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
            TestMap = new("test.zmap");
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

            // again temporary (insert these into the map lol, just to display them)
            for (int tileId = 0; tileId < TestMap.Tiles.Count; tileId++) 
            {
                ZbMapTile mapTile = TestMap.Tiles[tileId];

                int row = tileId / SharedAssets.SPRITESHEET_SHEET_HEIGHT;

                // set position
                mapTile.Position = new(0 + (SharedAssets.SPRITESHEET_TILE_WIDTH * (row % SharedAssets.SPRITESHEET_SHEET_WIDTH)), SharedAssets.SPRITESHEET_TILE_HEIGHT * row);

                // add it
                Lightning.Renderer.AddRenderable(mapTile); // this just draws a texture atlas a lot of times
            }
        }

        public override void Shutdown()
        {

        }

        public override void SwitchTo(Scene oldScene)
        {
            TextManager.DrawText("Hello World!", "DebugFont", new Vector2(300, 300), Color.Red);
        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Render()
        {

        }
    }
}
