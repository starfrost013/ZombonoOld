
namespace Zombono
{
    /// <summary>
    /// Zombono map tile class.
    /// </summary>
    internal class ZbMapTile : Renderable
    {
        internal int Id { get; set; }

        public ZbMapTile(string name, int id) : base(name)
        {
            Id = id;
        }

        public override void Draw()
        {
            // this will pull the global spritesheet (when it's done)
            base.Draw();
        }

        internal byte[] ToByteArray() => BitConverter.GetBytes(Id);
    }
}
