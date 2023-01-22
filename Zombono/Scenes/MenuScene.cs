
namespace Zombono
{
    public class MenuScene : Scene
    {
        public override void Start()
        {

        }

        public override void Render()
        {
            /*
            // TEMP until NEW TEXT API
            // todo: ACTUALLY CENTRE
            TextManager.DrawText("PLACEHOLDER ALPHA MENU", "Arial.48pt", new(0, 0), Color.Red, Color.White);
            TextManager.DrawText("Zombono Alpha", "Arial.18pt", new Vector2(GlobalSettings.GraphicsResolutionX / 4, GlobalSettings.GraphicsResolutionY / 4), Color.Green);
            TextManager.DrawText("Please Enter Server IP", "Arial.18pt", new Vector2(GlobalSettings.GraphicsResolutionX / 4, GlobalSettings.GraphicsResolutionY / 3.5f), Color.Green);
            */
        }

        public override void SwitchTo(Scene oldScene)
        {
            /*
            // clear background (temp)
            Lightning.Renderer.Clear(Color.FromArgb(32, 32, 32, 32));

            // Load fonts
            // todo: actually centre stuff
            FontManager.AddAsset(new Font("Arial", 18, "Arial.18pt"));
            FontManager.AddAsset(new Font("Arial", 48, "Arial.48pt"));

            UIManager.AddAsset(new TextBox("IpTextbox", 300, "Arial.18pt")
            {
                ZIndex = 5000000,
                BorderColor = Color.Green,
                BackgroundColor = Color.White,
                ForegroundColor = Color.Black,
                Size = new(400, 20),
                BorderSize = new(2, 2),
                Position = new(GlobalSettings.GraphicsResolutionX / 4, (GlobalSettings.GraphicsResolutionY / 3) + 50),
            });

            UIManager.AddAsset(new Button("IpButton", "Arial.18pt")
            {
                ZIndex = 5000000,
                HoverColor = Color.DarkGray,
                PressedColor = Color.Gray,
                ForegroundColor = Color.White,
                BackgroundColor = Color.Green,
                Size = new(400, 20),
                Position = new(GlobalSettings.GraphicsResolutionX / 4, (GlobalSettings.GraphicsResolutionY / 3) + 150),
                Text = "Connect to Server"
            });
            */
        }

        public override void SwitchFrom(Scene newScene)
        {

        }

        public override void Shutdown()
        {

        }
    }
}
