
namespace Zombono
{
    /// <summary>
    /// Zombono map tile class.
    /// </summary>
    internal class ZbMapTile : Renderable
    {
        internal ushort Id { get; set; }

        public ZbMapTile(string name, ushort id) : base(name)
        {
            Id = id;
        }

        public override void Draw()
        {
            Debug.Assert(SharedAssets.MainSpritesheet != null); // already loaded before here
            SharedAssets.MainSpritesheet.Index = Id;
            SharedAssets.MainSpritesheet.DrawFrame();
        }

        internal byte[] ToByteArray() => BitConverter.GetBytes(Id);
    }
}
