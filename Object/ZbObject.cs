namespace Zombono
{
    internal class ZbObject : Renderable
    {
        public ZbObject(string name) : base(name)
        {
            // repurpose name for class name for reading from object
            if (string.IsNullOrWhiteSpace(name)) Name = GetType().Name;
        }

        internal virtual byte[] ToByteArray()
        {
            return Name.ToByteArrayWithLength();
        }

        internal virtual ZbObject FromByteArray(BinaryReader stream)
        {
            return new ZbObject(stream.ReadString());
        }
    }
}
