using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuerious.Entities
{
    class Ball : Otter.Entity
    {
        public IsoMovement myMovement;
        public Utility.IsometricUtils.IsoMap theMap;

        public Ball(Utility.IsometricUtils.IsoMap map, Otter.Graphic gfx)
        {
            theMap = map;

            gfx.CenterOrigin();

            AddGraphic(gfx);


            myMovement = new IsoMovement(theMap, 8);
            myMovement.IsoPos.X = 2;
            myMovement.IsoPos.Y = 4;
            myMovement.IsoPos.Z = 0;
            myMovement.OnGround = true;

            AddComponent(myMovement);

        }

        public override void Added()
        {
            base.Added();

        }

        public override void Update()
        {
            base.Update();

        }

        public Otter.Vector3 GetIsoPosition()
        {
            return myMovement.IsoPos;
        }

        public Otter.Vector3 GetTileIsoPosition()
        {
            return myMovement.IsoPos / 2;
        }

    }
}
