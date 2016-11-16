using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using OtterTemplate.Entities;

//----------------
// Author: J. Brown (DrMelon)
// Part of the [OtterTemplate] Project.
// Date: 09/03/2016
//----------------
// Purpose: Ultra-basic menu scene. Tool/level select.

namespace OtterTemplate.Scenes
{
    class MenuScene : Scene
    {
        // Storage
        Music MenuMusic;
        Image DebugMenuImage;
        Font SystemFont;
        RichText MenuTitle;
        Menu MainMenu;

        ControllerXbox360 Player1Controller;

        int CurrentSelection = 0;
        int MaxSelection = 2;

        public override void Begin()
        {
            // Load Music
            MenuMusic = new Music(Assets.MUSIC_MENU, true);
            MenuMusic.Play();

            // Load BG
            DebugMenuImage = new Image(Assets.GFX_DEBUGMENU);
            DebugMenuImage.Repeat = true;

            // Load font & text
            SystemFont = new Font(Assets.FNT_SYSTEM);

            // Title
            MenuTitle = new RichText("{color:FFFAF0}{shadowX:2}{shadowY:2}{colorShadow:2e2238}Cue-rious.", SystemFont, 48, 100, 100);
            MenuTitle.SetPosition(90, 72);
            MenuTitle.CenterOrigin();

            // Main Menu
            MainMenu = new Menu(new Dictionary<string, Action>() { { "Play Pool", DoThing }, { "Don't", Quit } }, SystemFont, 32, 34, "{color:FFFAF0}{shadowX:2}{shadowY:2}{colorShadow:2e2238}", "{color:EEDC82}{shadowX:3}{shadowY:3}{colorShadow:2e2238}");
            MainMenu.X = 32;
            MainMenu.Y = 172;

            // Fetch controller
            Player1Controller = Game.Session("Player1").GetController<ControllerXbox360>();



            AddGraphic(DebugMenuImage);
            AddGraphic(MenuTitle);
            Add(MainMenu);
        }

        public override void Update()
        {
            base.Update();

            // Scroll BG
            DebugMenuImage.X += 1;
            DebugMenuImage.Y -= 1;

        }

        public void DoThing()
        {
            Util.Log("Thing got did.");
            
        }

        public void Quit()
        {
            Game.Close();
        }


    }
}
