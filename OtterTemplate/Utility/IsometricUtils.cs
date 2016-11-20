using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace Cuerious.Utility
{
    public static class IsometricUtils
    {

        public static float IsoWidth = 32;
        public static float IsoHeight = 16;

        // This returns the X, Y screen position, and the layer number.
        public static Vector3 IsoToScreenSpace(Vector3 pos)
        {
            Vector3 ret = new Vector3(0, 0, 0);

            ret.X = pos.X * (IsoWidth / 2) + pos.Y * (IsoWidth / 2);
            ret.Y = -pos.Z * (IsoHeight / 2) + pos.Y * (IsoHeight / 2) - pos.X * (IsoHeight / 2);
            ret.Z = (-(2 * pos.Z) + pos.Y - (pos.X * 0.5f));

            return ret;
        }


        public class IsoMap
        {
            public enum IsoTileType
            {
                NONE,
                FLOOR,
                SLOPE_POS_X,
                SLOPE_NEG_X,
                SLOPE_POS_Y,
                SLOPE_NEG_Y,

                ERROR
            }


            public IsoTileType[,,] mapArray;
            public int sizeX, sizeY, sizeZ;

            public IsoMap(int sX, int sY, int sZ)
            {
                mapArray = new IsoTileType[sX, sY, sZ];
                sizeX = sX;
                sizeY = sY;
                sizeZ = sZ;
            }

            public IsoTileType GetTile(int x, int y, int z)
            {
                if(x < 0 || y < 0 || z < 0 || x >= sizeX || y >= sizeY || z >= sizeZ)
                {
                    // NO
                    Util.Log("Tile index out of range!");
                    return IsoTileType.ERROR;
                }

                return mapArray[x, y, z];
            }

            public void SetTile(int x, int y, int z, IsoTileType type)
            {
                if (x < 0 || y < 0 || z < 0 || x >= sizeX || y >= sizeY || z >= sizeZ)
                {
                    // NO
                    Util.Log("Tile index out of range!");
                    return;
                }

                mapArray[x, y, z] = type;
            }


        }


        // This creates a 2d Otter tilemap object from a 3d map array
        public static Tilemap CreateTilemapFrom3D(string tileset, IsoMap isoMap, int zmax = -1, int zmin = -1)
        {
            // Need at least 4x4 space to represent one iso-tile.
            Tilemap outmap = new Tilemap(tileset, (isoMap.sizeX+2) * 4 * 16, (isoMap.sizeY+2) * 4 * 16, 16, 16);


            int zMin = zmin;
            int zMax = zmax;

            if(zmin < 0 || zmin > isoMap.sizeY)
            {
                zMin = 0;
            }
            if(zmax < 0 || zmax > isoMap.sizeY)
            {
                zMax = isoMap.sizeY;
            }

            // Tile layering notes:
            // 1 Layer for Floors, 1 for walls?
            // Or do we need a floor and wall layer for each Y coordinate?
            // Maybe it's simply good enough to just draw high y-coord stuff last.
            // yeah, loop Y coord, do floor->wall pass. so higher-y floors draw over walls?
            // single layer per y-coord this way

            for(int Y = zMin; Y < zMax; Y++)
            {
                outmap.AddLayer("YLayer" + Y.ToString(), (-Y*2) - 1);
                outmap.AddLayer("YLayerW" + Y.ToString(), (-Y*2) - 2);

                for (int Z = 0; Z < isoMap.sizeZ; Z++)
                {
                    for(int X = 0; X < isoMap.sizeX; X++)
                    {
                       
                        if(isoMap.GetTile(X, Y, Z) == IsoMap.IsoTileType.FLOOR)
                        {
                            // draw floor tiles
                            // 4x4 tile array
                            Vector3 scrPos = IsometricUtils.IsoToScreenSpace(new Vector3(X, Y, Z));

                            int xPlacement = (int)(scrPos.X / 8) + 1;
                            int yPlacement = (int)(scrPos.Y / 8) + 1;

                            outmap.SetTile(xPlacement, yPlacement, 9, "YLayer" + Y.ToString());
                            outmap.SetTile((xPlacement) + 1, yPlacement, 10, "YLayer" + Y.ToString());
                            outmap.SetTile((xPlacement) + 2, yPlacement, 7, "YLayer" + Y.ToString());
                            outmap.SetTile((xPlacement) + 3, yPlacement, 8, "YLayer" + Y.ToString());

                            outmap.SetTile(xPlacement, yPlacement + 1, 3, "YLayer" + Y.ToString());
                            outmap.SetTile((xPlacement) + 1, yPlacement + 1, 4, "YLayer" + Y.ToString());
                            outmap.SetTile((xPlacement) + 2, yPlacement + 1, 5, "YLayer" + Y.ToString());
                            outmap.SetTile((xPlacement) + 3, yPlacement + 1, 6, "YLayer" + Y.ToString());

                            // draw walls on top (based on zcoord?)

                            outmap.SetTile(xPlacement, yPlacement + 1, 19, "YLayerW" + Y.ToString());
                            outmap.SetTile((xPlacement) + 1, yPlacement + 1, 20, "YLayerW" + Y.ToString());
                            outmap.SetTile((xPlacement) + 2, yPlacement + 1, 21, "YLayerW" + Y.ToString());
                            outmap.SetTile((xPlacement) + 3, yPlacement + 1, 22, "YLayerW" + Y.ToString());

                            if(Z > 0)
                            {
                                // do sandwich blocks!
                                for(int i = 0; i < Z+1; i++)
                                {
                                    outmap.SetTile(xPlacement, yPlacement + 2 + i, 17, "YLayerW" + Y.ToString());
                                    outmap.SetTile((xPlacement) + 1, yPlacement + 2 + i, 11, "YLayerW" + Y.ToString());
                                    outmap.SetTile((xPlacement) + 2, yPlacement + 2 + i, 12, "YLayerW" + Y.ToString());
                                    outmap.SetTile((xPlacement) + 3, yPlacement + 2 + i, 18, "YLayerW" + Y.ToString());

                                    if(i == Z)
                                    {
                                        outmap.SetTile(xPlacement, yPlacement + 2 + i, 13, "YLayerW" + Y.ToString());
                                        outmap.SetTile((xPlacement) + 1, yPlacement + 2 + i, 14, "YLayerW" + Y.ToString());
                                        outmap.SetTile((xPlacement) + 2, yPlacement + 2 + i, 15, "YLayerW" + Y.ToString());
                                        outmap.SetTile((xPlacement) + 3, yPlacement + 2 + i, 16, "YLayerW" + Y.ToString());
                                    }
                                    
                                }
                            }
                            else
                            {
                                outmap.SetTile(xPlacement, yPlacement + 2, 13, "YLayerW" + Y.ToString());
                                outmap.SetTile((xPlacement) + 1, yPlacement + 2, 14, "YLayerW" + Y.ToString());
                                outmap.SetTile((xPlacement) + 2, yPlacement + 2, 15, "YLayerW" + Y.ToString());
                                outmap.SetTile((xPlacement) + 3, yPlacement + 2, 16, "YLayerW" + Y.ToString());
                            }

                            
                        }



                    }
                }
            }
            
            
     

            // Then, do a layer with the wall tiles on top.
            

            return outmap;
        }
    }
}
