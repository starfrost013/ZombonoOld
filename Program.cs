//Lightning 1.1 Visual Studio template
//©2022 starfrost, October 21, 2022
// Use this to get started with Lightnng

// Initialise Lightning, this will run MainScene
// this is in the LightningGL::Lightning class (included via a static global using)

if (args.Contains("-server".ToLowerInvariant()))
{
    InitServer();
}
else
{
    InitClient();
}

