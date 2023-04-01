namespace Zombono
{
    internal static class SharedAssets
    {
        internal static TextureAtlas? MainSpritesheet;

        // TEMP?
        internal const int SPRITESHEET_TILE_WIDTH = 32;

        internal const int SPRITESHEET_TILE_HEIGHT = 32;

        internal const int SPRITESHEET_SHEET_WIDTH = 128;

        internal const int SPRITESHEET_SHEET_HEIGHT = 128;

        internal static bool Loaded { get; set; }
        internal static bool Load()
        {
            try
            {
                MainSpritesheet = Lightning.Renderer.AddRenderable(new TextureAtlas("MainSpritesheet", new(SPRITESHEET_TILE_WIDTH, SPRITESHEET_TILE_HEIGHT),
                    new(SPRITESHEET_SHEET_WIDTH, SPRITESHEET_SHEET_HEIGHT))
                {
                    Path = @"Content\Graphics\terrain.png"

                });

                Loaded = true;
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
