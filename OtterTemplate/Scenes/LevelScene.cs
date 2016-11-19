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
        Tilemap renderMap;
      
        public override void Begin()
        {
            // Fetch controller
            Player1Controller = Game.Session("Player1").GetController<ControllerXbox360>();

            isoMap = new IsometricUtils.IsoMap(4, 4, 4);
            isoMap.SetTile(0, 0, 0, IsometricUtils.IsoMap.IsoTileType.FLOOR);
            isoMap.SetTile(1, 1, 0, IsometricUtils.IsoMap.IsoTileType.FLOOR);
            isoMap.SetTile(1, 2, 0, IsometricUtils.IsoMap.IsoTileType.FLOOR);
            isoMap.SetTile(1, 3, 3, IsometricUtils.IsoMap.IsoTileType.FLOOR);

            renderMap = IsometricUtils.CreateTilemapFrom3D(Assets.GFX_TILEMAP_01, isoMap);

            AddGraphic(renderMap);
        }

        public override void Update()
        {
            base.Update();
        }



    }
}
