using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using Cuerious.Utility;

namespace Cuerious.Entities
{
    class IsoMovement : Component
    {
        public Vector3 IsoPos;
        public Vector3 IsoVel;
        public bool OnGround;
        public float Radius;
        public IsometricUtils.IsoMap theMap;

        public IsoMovement(IsometricUtils.IsoMap map, float R)
        {
            theMap = map;
            Radius = R;
        }

        public override void Added()
        {
            base.Added();

        }

        public override void Update()
        {
            base.Update();

            // Update Pos from Vel
            IsoPos += IsoVel;

            // Update X and Y to match Iso Pos, also depth layer
            Vector3 convertedPos = IsometricUtils.IsoToScreenSpace(IsoPos);
            Entity.X = convertedPos.X + (IsometricUtils.IsoWidth*2) - Radius*2;
            Entity.Y = convertedPos.Y + (IsometricUtils.IsoHeight * 2) - Radius;
            Entity.Layer = 0;//(int)convertedPos.Z;
        }

        public bool FindGround()
        {
            Vector3 ground = new Vector3(-1, -1, -1);

            // convert current isopos to tilepos
            Vector3 groundCheckPos = IsoPos;
            groundCheckPos.Z -= Radius;
            Vector3 tilePos = groundCheckPos / 16;    

            if(theMap.GetTile((int)tilePos.X, (int)tilePos.Y, (int)tilePos.Z) != IsometricUtils.IsoMap.IsoTileType.NONE)
            {
                return true;
            }

            return false;
        }



        public override void Render()
        {
            base.Render();

        }

    }
}
