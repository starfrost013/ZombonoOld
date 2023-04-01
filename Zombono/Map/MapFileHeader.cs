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

        public const byte FormatVersionMajor = 1;

        public const byte FormatVersionMinor = 1;

        public DateTime Timestamp { get; set; }
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

        public static MapFileHeader Read(BinaryReader stream)
        {
            string magic = stream.ReadString();

            if (magic != Magic)
            {
                Logger.LogError($"Invalid map file - magic not found (expected {Magic}, got {magic}!)", 2000, LoggerSeverity.Error);
            }

            byte formatVersionMajor = stream.ReadByte();
            byte formatVersionMinor = stream.ReadByte();

            if (formatVersionMajor != FormatVersionMajor
                || formatVersionMinor != FormatVersionMinor)
            {
                Logger.LogError($"Invalid map file - incorrect file format version (expected {FormatVersionMajor}.{FormatVersionMinor}, " +
                    $"got {formatVersionMajor}.{formatVersionMinor}!)", 2001, LoggerSeverity.Error);
            }

            return new MapFileHeader
            {
                Timestamp = new DateTime(1970, 1, 1, 1, 1, 1).AddSeconds(stream.ReadInt64()),
                MapName = stream.ReadString(),
                MapAuthor = stream.ReadString(),
                MapDescription = stream.ReadString(),
                MapTilesetPath = stream.ReadString(),
                MapWidth = stream.ReadInt32(),
                MapHeight = stream.ReadInt32(),
            };
        }

        public void Write(BinaryWriter stream)
        {
            stream.Write(Magic);
            stream.Write(FormatVersionMajor);
            stream.Write(FormatVersionMinor);
            stream.Write(DateTimeOffset.Now.ToUnixTimeSeconds());
            stream.Write(MapName);
            stream.Write(MapAuthor);
            stream.Write(MapDescription);
            stream.Write(MapTilesetPath);
            stream.Write(MapWidth);
            stream.Write(MapHeight);
        }
    }
}
