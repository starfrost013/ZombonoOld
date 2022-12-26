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
                NCError.ShowErrorBox("Error - tried to load invalid map file", 2402, 
                    "MapFile::Path property does not exist during call to MapFile::Read!", NCErrorSeverity.FatalError);
                return;
            }

            if (!Path.Contains(".zmap")) // move to localsettings?
            {
                NCError.ShowErrorBox("Filename must be a zmap file!", 2404,
                    "MapFile::Path property does not contain the string '.zmap'", NCErrorSeverity.FatalError);
            }

            try
            {
                NCLogging.Log($"Loading map {Path}...");

                using (BinaryReader br = new(new FileStream(Path, FileMode.Open)))
                {
                    NCLogging.Log("Reading header...");

                    Header = MapFileHeader.Read(br);

                    NCLogging.Log("Reading map tiles...");
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
            }
            catch (Exception ex)
            {
                NCError.ShowErrorBox($"An error occurred loading map tiles.", 2403, "An exception occurred in MapFile::Read", NCErrorSeverity.FatalError, ex);
            }
        }

        public void Write()
        {
            if (!Uri.IsWellFormedUriString(Path, UriKind.RelativeOrAbsolute))
            {
                NCError.ShowErrorBox($"Tried to write to invalid path {Path}!", 2405, 
                    "Call to Uri::IsWellFormedUriStrig in MapFile::Write returned FALSE", NCErrorSeverity.Error);
                return;
            }

            int mapTileCount = Header.MapHeight * Header.MapWidth;

            if (Tiles.Count != mapTileCount)
            {
                NCError.ShowErrorBox($"Invalid number of tiles ({Tiles.Count} recorded, vs {mapTileCount} ({Header.MapWidth}x{Header.MapHeight})", 2406,
                    "MapTile::Tiles::Count is not equal to (MapTileHeader::MapHeight * MapTileHeader::MapWidth)", NCErrorSeverity.Error);
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
