﻿// Lightning Global Using file for Example Project
// October 21, 2022

// DO NOT MODIFY unless you know what you are doing

// Lightning itself
global using LightningBase;
global using LightningGL;
global using LightningPackager; // remove if you don't want the packager
// Core .NET stuff lightning uses a lot
global using System.Drawing;
global using System.Diagnostics;
global using System.Numerics;
// SDL2 stuff (to make accessing SDL and freetype functions less painful)
global using static LightningBase.FreeTypeApi;
global using static LightningBase.SDL;
global using static LightningBase.SDL_image;
global using static LightningBase.SDL_mixer;
global using static LightningGL.Lightning;
global using NuCore.Utilities;
