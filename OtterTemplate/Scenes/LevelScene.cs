using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using Cuerious.Entities;
using Cuerious.Systems;
using Cuerious.Utility;

//----------------
// Author: J. Brown (DrMelon)
// Part of the [Cuerious] Project.
// Date: 09/03/2016
//----------------
// Purpose: Ultra-basic menu scene. Tool/level select.



namespace Cuerious.Scenes
{
    class LevelScene : Scene
    {
        ControllerXbox360 Player1Controller;

        IsometricUtils.IsoMap isoMap;

        // NOTE: might need lists of rendermap ents and tilemaps, with zmin and zmax set for each Y-layer a ball exists on...
        Entity renderMapEntA;
        Tilemap renderMapA;
        Entity renderMapEntB;
        Tilemap renderMapB;

        Ball cueBall;
        Entity cueBallDissolve = new Entity(0, 0, new Image(Assets.GFX_CUEBALL_D));
        float cueBallYPos = 0.0f;
        float cueBallYMoveAmt = 0.0f;
      
        public override void Begin()
        {
            // Fetch controller
            Player1Controller = Game.Session("Player1").GetController<ControllerXbox360>();


            isoMap = new IsometricUtils.IsoMap(5, 5, 5);
            isoMap.SetTile(0, 0, 0, IsometricUtils.IsoMap.IsoTileType.FLOOR);
            isoMap.SetTile(1, 1, 0, IsometricUtils.IsoMap.IsoTileType.FLOOR);
            isoMap.SetTile(1, 2, 0, IsometricUtils.IsoMap.IsoTileType.FLOOR);
            isoMap.SetTile(1, 3, 3, IsometricUtils.IsoMap.IsoTileType.FLOOR);


            // Make ents
            cueBall = new Ball(isoMap, new Image(Assets.GFX_CUEBALL));
            cueBallYPos = cueBall.GetIsoPosition().Y;
            cueBallDissolve.Graphic.CenterOrigin();


            renderMapEntA = new Entity();
            renderMapEntB = new Entity();

            UpdateMaps();

            renderMapEntA.AddGraphic(renderMapA);
            renderMapEntB.AddGraphic(renderMapB);



            Add(renderMapEntA);
            Add(renderMapEntB);
            Add(cueBall);
            Add(cueBallDissolve);
        }

        public override void Update()
        {
            base.Update();

            // check cueball y move amt (Y-SEGMENTATION??)
            cueBallYMoveAmt = Math.Abs(cueBallYPos - cueBall.GetIsoPosition().Y);

           

         //   if (cueBallYMoveAmt > 2.0f)
          //  {
         //       cueBallYPos = cueBall.GetIsoPosition().Y;
         //       cueBallYMoveAmt = 0.0f;
                UpdateMaps();
        //    }


            cueBall.myMovement.IsoPos.Y = 6 + ((float)Math.Sin(Game.Timer * 0.01f) * 4.0f);

            renderMapEntA.Layer = cueBall.Layer + 1;
            renderMapEntB.Layer = cueBall.Layer - 1;

            cueBallDissolve.Layer = cueBall.Layer - 2;
        }

        public override void UpdateLast()
        {
            base.UpdateLast();

            cueBallDissolve.X = cueBall.X;
            cueBallDissolve.Y = cueBall.Y;
        }

        public void UpdateMaps()
        {
            Util.Log("UpdateMaps Called: " + ((((int)cueBall.GetTileIsoPosition().Y) + 1)).ToString());
            renderMapA = IsometricUtils.CreateTilemapFrom3D(Assets.GFX_TILEMAP_01, isoMap, (((int)(cueBall.GetTileIsoPosition().Y + 0.51f))) + 1);
            renderMapB = IsometricUtils.CreateTilemapFrom3D(Assets.GFX_TILEMAP_01, isoMap, -1, (((int)(cueBall.GetTileIsoPosition().Y + 0.51f))) + 1);

            renderMapEntA.Graphic = renderMapA;
            renderMapEntB.Graphic = renderMapB;

  
        }



    }
}
