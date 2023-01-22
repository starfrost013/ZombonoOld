// see globalusings.cs for namespaces used here

namespace Zombono
{
    /// <summary>
    /// MainScene
    /// 
    /// The main scene of your Lightning game. 
    /// Add additional scenes by creating classes that inherit from Scene.
    /// </summary>
    public class MainScene : Scene
    {
        public override void Start()
        {

        }

        public override void Shutdown()
        {

        }

        public override void SwitchTo(Scene oldScene)
        {
            Lightning.Renderer.AddRenderable(new TextBlock("TextBlockgasd", "MainScene - the game is here", "DebugFont", new(300, 300), Color.Green));
        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Render()
        {

        }
    }
}
