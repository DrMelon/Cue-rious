using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using Cuerious.Entities;
using Cuerious.Systems;

//----------------
// Author: J. Brown (DrMelon)
// Part of the [Cuerious] Project.
// Date: 09/03/2016
//----------------
// Purpose: Ultra-basic menu scene. Tool/level select.

namespace Cuerious.Scenes
{
    class MenuScene : Scene
    {
        // Storage
        Music MenuMusic;
        Image DebugMenuImage;
        Font SystemFont;

        RichText MenuTitle;
        Menu MainMenu;

        RichText PlayerSelectTitle;
        Menu PlayerSelectMenu;

        public SimpleFSM menuFSM;

        ControllerXbox360 Player1Controller;

        public const float TITLE_X = 90;
        public const float TITLE_Y = 72;

        public const float TITLE_OFFX = 90;
        public const float TITLE_OFFY = -72;

        public const float MENU_X = 32;
        public const float MENU_Y = 172;

        public const float MENU_OFFX = -300;
        public const float MENU_OFFY = 172;

        public Tween hideMainMenu;
        public Tween hideMainTitle;
        public Tween showMainMenu;
        public Tween showMainTitle;

        public Tween hidePlayerSelectMenu;
        public Tween hidePlayerSelectTitle;
        public Tween showPlayerSelectMenu;
        public Tween showPlayerSelectTitle;


        public override void Begin()
        {
            // Load Music
            MenuMusic = new Music(Assets.MUSIC_MENU, true);
           // MenuMusic.Play();

            

            // Load BG
            DebugMenuImage = new Image(Assets.GFX_DEBUGMENU);
            DebugMenuImage.Repeat = true;

            // Load font & text
            SystemFont = new Font(Assets.FNT_SYSTEM);

            // Titles
            MenuTitle = new RichText("{color:FFFAF0}{shadowX:2}{shadowY:2}{colorShadow:2e2238}Cue-rious.", SystemFont, 48, 100, 100);
            MenuTitle.SetPosition(TITLE_X, TITLE_Y);
            MenuTitle.CenterOrigin();

            PlayerSelectTitle = new RichText("{color:FFFAF0}{shadowX:2}{shadowY:2}{colorShadow:2e2238}Multiplayer?", SystemFont, 48, 100, 100);
            PlayerSelectTitle.SetPosition(TITLE_OFFX, TITLE_OFFY);
            PlayerSelectTitle.CenterOrigin();

            // Menus
            MainMenu = new Menu(new Dictionary<string, Action>() { { "Play Pool", () => { menuFSM.SwitchState("TransitionToPlayerSelect"); } }, { "Don't", Quit } }, SystemFont, 32, 34, "{color:FFFAF0}{shadowX:2}{shadowY:2}{colorShadow:2e2238}", "{color:EEDC82}{shadowX:3}{shadowY:3}{colorShadow:2e2238}");
            MainMenu.X = MENU_X;
            MainMenu.Y = MENU_Y;

            PlayerSelectMenu = new Menu(new Dictionary<string, Action>() { { "Just the one, thanks.", StartSinglePlayer }, { "Make it a party.", StartMultiPlayer }, { "Actually, um...", ()=> { menuFSM.SwitchState("TransitionBackPlayerSelection"); } } }, SystemFont, 32, 34, "{color:FFFAF0}{shadowX:2}{shadowY:2}{colorShadow:2e2238}", "{color:EEDC82}{shadowX:3}{shadowY:3}{colorShadow:2e2238}");
            PlayerSelectMenu.X = MENU_OFFX;
            PlayerSelectMenu.Y = MENU_OFFY - 32;
            PlayerSelectMenu.CanInput = false;

            // Fetch controller
            Player1Controller = Game.Session("Player1").GetController<ControllerXbox360>();

            // Create FSM
            menuFSM = new SimpleFSM();

            // Create menu states
            FSMState mainMenuState = new FSMState();
            mainMenuState.InitState = InitMainMenuState;
            mainMenuState.UpdateState = UpdateMainMenuState;

            FSMState transitionToPlayerSelect = new FSMState();
            transitionToPlayerSelect.InitState = InitTransitionToPlayerSelect;
            transitionToPlayerSelect.UpdateState = UpdateTransitionToPlayerSelect;

            FSMState playerSelectState = new FSMState();
            playerSelectState.InitState = InitPlayerSelect;
            playerSelectState.UpdateState = UpdatePlayerSelect;


            FSMState transitionBackPlayerSelect = new FSMState();
            transitionBackPlayerSelect.InitState = InitTransitionBackPlayerSelect;
            transitionBackPlayerSelect.UpdateState = UpdateTransitionBackPlayerSelect;

            // Set up states
            menuFSM.possibleStates.Add("MainMenu", mainMenuState);
            menuFSM.possibleStates.Add("TransitionToPlayerSelect", transitionToPlayerSelect);
            menuFSM.possibleStates.Add("PlayerSelect", playerSelectState);
            menuFSM.possibleStates.Add("TransitionBackPlayerSelection", transitionBackPlayerSelect);

            // Turn on FSM
            menuFSM.SwitchState("MainMenu");
            menuFSM.On = true;

            AddGraphic(DebugMenuImage);
            AddGraphic(MenuTitle);
            AddGraphic(PlayerSelectTitle);
            Add(MainMenu);
            Add(PlayerSelectMenu);
        }

        public override void Update()
        {
            base.Update();

            // Scroll BG
            DebugMenuImage.X += 1;
            DebugMenuImage.Y -= 1;

            // Update FSM
            menuFSM.Update();

        }

        public void InitMainMenuState()
        {
            // Reset visuals 
            MainMenu.SetPosition(MENU_X, MENU_Y);
            MainMenu.CanInput = true;
            MenuTitle.SetPosition(TITLE_X, TITLE_Y);
        }

        public void UpdateMainMenuState()
        {
            
        }

        public void InitTransitionToPlayerSelect()
        {
            MainMenu.CanInput = false;

            hideMainMenu = Tweener.Tween(MainMenu, new { X = MENU_OFFX, Y = MENU_OFFY }, 1.3f * 60, 0, true);
            hideMainMenu.Ease(Otter.Ease.BackIn);

            hideMainTitle = Tweener.Tween(MenuTitle, new { X = TITLE_OFFX, Y = TITLE_OFFY }, 1.3f * 60, 0, true);
            hideMainTitle.Ease(Otter.Ease.BackIn);

            showPlayerSelectMenu = Tweener.Tween(PlayerSelectMenu, new { X = MENU_X, Y = MENU_Y - 32 }, 1.3f * 60, 1.3f * 60, true);
            showPlayerSelectMenu.Ease(Otter.Ease.BackOut);

            showPlayerSelectTitle = Tweener.Tween(PlayerSelectTitle, new { X = TITLE_X, Y = TITLE_Y }, 1.3f * 60, 1.3f * 60, true);
            showPlayerSelectTitle.Ease(Otter.Ease.BackOut);

            showPlayerSelectTitle.OnComplete(() => { menuFSM.SwitchState("PlayerSelect"); });
        }

        public void UpdateTransitionToPlayerSelect()
        {
            


        }

        public void InitPlayerSelect()
        {
            // Reset visuals
            PlayerSelectMenu.SetPosition(MENU_X, MENU_Y - 32);
            PlayerSelectMenu.CanInput = true;
            PlayerSelectTitle.SetPosition(TITLE_X, TITLE_Y);
        }

        public void UpdatePlayerSelect()
        {

        }

        public void InitTransitionBackPlayerSelect()
        {
            PlayerSelectMenu.CanInput = false;

            hidePlayerSelectMenu = Tweener.Tween(PlayerSelectMenu, new { X = MENU_OFFX, Y = MENU_OFFY - 32 }, 1.3f * 60, 0, true);
            hidePlayerSelectMenu.Ease(Otter.Ease.BackIn);

            hidePlayerSelectTitle = Tweener.Tween(PlayerSelectTitle, new { X = TITLE_OFFX, Y = TITLE_OFFY }, 1.3f * 60, 0, true);
            hidePlayerSelectTitle.Ease(Otter.Ease.BackIn);

            showMainMenu = Tweener.Tween(MainMenu, new { X = MENU_X, Y = MENU_Y }, 1.3f * 60, 1.3f * 60, true);
            showMainMenu.Ease(Otter.Ease.BackOut);

            showMainTitle = Tweener.Tween(MenuTitle, new { X = TITLE_X, Y = TITLE_Y }, 1.3f * 60, 1.3f * 60, true);
            showMainTitle.Ease(Otter.Ease.BackOut);

            showMainTitle.OnComplete(() => { menuFSM.SwitchState("MainMenu"); });
        }

        public void UpdateTransitionBackPlayerSelect()
        {



        }

        public void StartSinglePlayer()
        {
            Util.Log("Single player selected.");

            hidePlayerSelectMenu = Tweener.Tween(PlayerSelectMenu, new { X = MENU_OFFX, Y = MENU_OFFY - 32 }, 1.3f * 60, 0, true);
            hidePlayerSelectMenu.Ease(Otter.Ease.BackIn);

            hidePlayerSelectTitle = Tweener.Tween(PlayerSelectTitle, new { X = TITLE_OFFX, Y = TITLE_OFFY }, 1.3f * 60, 0, true);
            hidePlayerSelectTitle.Ease(Otter.Ease.BackIn);

            var boop = Tweener.Tween(DebugMenuImage, new { Alpha = Color.Black.R }, 1.3f * 60, 0.75f * 60, true);
            boop.OnComplete(() => { Game.Instance.SwitchScene(new LevelScene()); });

        }

        public void StartMultiPlayer()
        {
            Util.Log("Multiplayer selected.");
        }

        public void Quit()
        {
            Game.Close();
        }


    }
}
