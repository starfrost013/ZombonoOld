namespace Zombono
{
    public class MapFile
    {
        public string Path { get; private set; }
        public MapFileHeader Header { get; set; }
        internal List<ZbMapTile> Tiles { get; init; } // not sure if we should have this

        /// <summary>
        /// Determines if this map file has been loaded.
        /// </summary>
        public bool Loaded { get; private set; }

        public MapFile(string path)
        {
            Path = path;
            Header = new();
            Tiles = new();
        }

        public void Read()
        {
            if (!File.Exists(Path))
            {
                Logger.LogError("Error - tried to load invalid map file", 2402, LoggerSeverity.FatalError);
                return;
            }

            if (!Path.Contains(".zmap")) // move to localsettings?
            {
                Logger.LogError("Filename must be a zmap file!", 2404, LoggerSeverity.FatalError);
            }

            try
            {
                Logger.Log($"Loading map {Path}...");

                using (BinaryReader br = new(new FileStream(Path, FileMode.Open)))
                {
                    Logger.Log("Reading header...");

                    Header = MapFileHeader.Read(br);

                    Logger.Log("Reading map tiles...");
                    // read tiles
                    for (int y = 0; y < Header.MapHeight; y++)
                    {
                        for (int x = 0; x < Header.MapWidth; x++)
                        {
                            ZbMapTile mapTile = new($"Map-{x}-{y}", br.ReadUInt16());
                            Tiles.Add(mapTile);
                        }
                    }
                }

                // todo: read the ZbObjects!!!!!!!

                Logger.Log("Loading ZbObjects UNIMPLEMENTED (V0.00!!!!)");

                Loaded = true;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred loading map tiles.", 2403, LoggerSeverity.FatalError, ex);
            }
        }

        public void Write()
        {
            if (!Uri.IsWellFormedUriString(Path, UriKind.RelativeOrAbsolute))
            {
                Logger.LogError($"Tried to write to invalid path {Path}!", 2405, LoggerSeverity.Error);
                return;
            }

            int mapTileCount = Header.MapHeight * Header.MapWidth;

            if (Tiles.Count != mapTileCount)
            {
                Logger.LogError($"Invalid number of tiles ({Tiles.Count} recorded, vs {mapTileCount} ({Header.MapWidth}x{Header.MapHeight})", 2406,
                    LoggerSeverity.Error);
                return;
            }

            using (BinaryWriter bw = new(new FileStream(Path, FileMode.OpenOrCreate)))
            {
                Header.Write(bw);

                // they get loaded in row order
                foreach (ZbMapTile mapTile in Tiles)
                {
                    bw.Write(mapTile.Id);
                }
            }
        }
    }
}
