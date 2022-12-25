namespace Zombono.Scenes
{
    /// <summary>
    /// InitScene
    /// 
    /// Initialises zombono
    /// </summary>
    internal class InitScene : Scene
    {
        public override void Start()
        {
            // probably better to put this in start
            NCLogging.Log("Zombono SharedAssets Loading...");
            SharedAssets.Load();
        }

        public override void Shutdown()
        {

        }

        public override void SwitchTo(Scene oldScene)
        {

        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Render()
        {
            NCLogging.Log("Going to MapPrototypeScene");
            // the plan here is to show some logos and stuff
            // so this is not the only line of code that will ever be here
            Lightning.Client.SetCurrentScene("MapPrototypeScene");
        }
    }
}
