using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace Cuerious.Entities
{
    class Menu : Entity
    {
        public Dictionary<string, Action> MenuOptions;
        
        public string StyleNormal;
        public string StyleHighlight;

        public Font ChosenFont;

        public int CurrentlySelected = 0;
        public int MaxSelection = 0;
        public bool CanInput = true;

        public Dictionary<string, RichText> myTexts;
        public Dictionary<int, string> Indices = new Dictionary<int, string>();

        public ControllerXbox360 PlayerController1;

        public Menu(Dictionary<string, Action> newMenuOptions, Font newChosenFont, int FontSize, int Spacing, string NewStyleNormal, string NewStyleHighlight)
        {

            MenuOptions = newMenuOptions;
            ChosenFont = newChosenFont;
            StyleNormal = NewStyleNormal;
            StyleHighlight = NewStyleHighlight;
            myTexts = new Dictionary<string, RichText>();
            MaxSelection = newMenuOptions.Count - 1;

            int yOffset = 0;
            int i = 0;
            foreach(KeyValuePair<string, Action> entry in MenuOptions)
            {
                RichText newText = new RichText(entry.Key, ChosenFont, FontSize);
                newText.TextAlign = TextAlign.Left;
                newText.CenterTextOriginY();
                newText.Scroll = 0;

                newText.Y = yOffset;

                yOffset += (Spacing);

                myTexts.Add(entry.Key, newText);
                Indices.Add(i, entry.Key);
                i++;
                AddGraphic(newText);
            }



            CheckSelection();
        }

        public override void Added()
        {
            base.Added();
            PlayerController1 = Game.Session("Player1").GetController<ControllerXbox360>();
        }

        public override void Update()
        {
            base.Update();


            // Do input
            if(CanInput)
            {
                HandleInput();
            }

        }

        public void HandleInput()
        {
            // Menu Controls

            if (PlayerController1.DPad.Up.Pressed)
            {
                if (CurrentlySelected == 0)
                {
                    CurrentlySelected = MaxSelection;
                }
                else
                {
                    CurrentlySelected--;
                }

                CheckSelection();

            }



            if (PlayerController1.DPad.Down.Pressed)
            {
                if (CurrentlySelected == MaxSelection)
                {
                    CurrentlySelected = 0;
                }
                else
                {
                    CurrentlySelected++;
                }

                CheckSelection();

            }

            if (PlayerController1.Start.Pressed)
            {
                DoSelection();
            }
        }

        public void CheckSelection()
        {
            for(int i = 0; i < MenuOptions.Count; i++)
            {
                if(i == CurrentlySelected)
                {
                    myTexts[Indices[i]].String = StyleHighlight + myTexts[Indices[i]].CleanString;
                }
                else
                {
                    myTexts[Indices[i]].String = StyleNormal + myTexts[Indices[i]].CleanString;
                }
            }
        }

        public void DoSelection()
        {
            MenuOptions[Indices[CurrentlySelected]]();
        }
    }
}
