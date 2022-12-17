
namespace Zombono
{
    /// <summary>
    /// MapFileHeader
    /// 
    /// Defines a map file header.
    /// </summary>
    public class MapFileHeader
    {
        public const string Magic = "iwilldie";

        public const int FormatVersionMajor = 1;

        public const int FormatVersionMinor = 0;

        public string MapName { get; set; }

        public string MapAuthor { get; set; }

        public string MapDescription { get; set; }

        /// <summary>
        /// Map tileset path.
        /// Default value - none - loads default tileset.
        /// </summary>
        public string MapTilesetPath { get; set; }

        public int MapWidth { get; set; }

        public int MapHeight { get; set; }

        public MapFileHeader()
        {
            MapName = "***PLEASE FILL IN MAP NAME***";
            MapAuthor = "***PLEASE FILL IN MAP AUTHOR***";
            MapDescription = "***PLEASE FILL IN MAP DESCRIPTION***";
            MapTilesetPath = string.Empty;
        }
    }
}
