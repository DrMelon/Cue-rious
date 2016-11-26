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
        public bool IsFrozen;

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

            CheckTileCollision();

            if(!FindGround())
            {
                IsoVel.Z -= 0.00981f;
                OnGround = false;
            }
            else 
            {
                // apply ground friction
                IsoVel.X *= 0.995f;
                IsoVel.Y *= 0.995f;

                if (IsoVel.Z < 0.0f)
                {
                    IsoVel.Z = -IsoVel.Z * 0.8f;

                    if (Math.Abs(IsoVel.Z) < 0.0001)
                    {
                        OnGround = true;

                        IsoVel.Z = 0.0f;
                    }
                }
            }

            // Update Pos from Vel
            if(Math.Abs(IsoVel.X) < 0.0001 && Math.Abs(IsoVel.Y) < 0.0001 && OnGround)
            {
                IsoVel.X = 0;
                IsoVel.Y = 0;
                IsoVel.Z = 0;
                IsFrozen = true;
            }

            if (!IsFrozen)
            {
                IsoPos += IsoVel;
            }

            // Update X and Y to match Iso Pos, also depth layer
            Vector3 convertedPos = IsometricUtils.IsoToScreenSpace(IsoPos);
            Entity.X = convertedPos.X + (IsometricUtils.IsoWidth * 2) - Radius * 2;
            Entity.Y = convertedPos.Y + (IsometricUtils.IsoHeight * 2) - Radius;
            Entity.Layer = (int)convertedPos.Z;


        }

        public bool FindGround()
        {
            Vector3 ground = new Vector3(-1, -1, -1);

            // convert current isopos to tilepos
            Vector3 groundCheckPos = IsoPos;
            groundCheckPos.Z -= Radius / 8;

            IsometricUtils.IsoMap.IsoTile theTile = theMap.GetTile((int)groundCheckPos.X / 2, (int)groundCheckPos.Y / 2);

            if (theTile.tileType != IsometricUtils.IsoMap.IsoTileType.NONE && theTile.tileType != IsometricUtils.IsoMap.IsoTileType.ERROR && theTile.height >= groundCheckPos.Z )
            {
                return true;
            }

            return false;
        }

        public void CheckTileCollision()
        {
            // Check pos + vel + radius in cardinal directions.
            Vector3 checkPos = IsoPos + IsoVel + new Vector3(Radius / 8);

            // if the tile at checkpos is on a higher zlevel than isopos is then dang, gotta bounce
            IsometricUtils.IsoMap.IsoTile theTile = theMap.GetTile((int)checkPos.X / 2, (int)checkPos.Y / 2);
            if (theTile.tileType != IsometricUtils.IsoMap.IsoTileType.NONE && theTile.tileType != IsometricUtils.IsoMap.IsoTileType.ERROR && theTile.height > ((int)checkPos.Z / 2))
            {
                // rebound if it's a wall, check for slopes tho
                // boing
                if (Math.Abs(IsoVel.X) > Math.Abs(IsoVel.Y))
                {
                    IsoVel.X = -IsoVel.X * 0.6f;
                }
                else
                {
                    IsoVel.Y = -IsoVel.Y * 0.6f;
                }
            }
        }



        public override void Render()
        {
            base.Render();

        }

    }
}
