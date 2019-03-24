using CocosSharp;
using SpellDefense.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common
{
    public class GridManager : CCNode
    {
        GridTile[,] gridTiles;
        int gridHeight;
        int gridWidth;
        CCSprite gridSprite;
        float tile_width;
        float tile_height;

        public GridManager(int width, int height, string gridImage)
        {
            gridHeight = height;
            gridWidth = width;
            gridTiles = new GridTile[width, height];

            gridSprite = new CCSprite(gridImage);
            tile_width = gridSprite.ScaledContentSize.Width;
            tile_height = gridSprite.ScaledContentSize.Height;

            for (int cellY = 0; cellY < height; cellY++)
            {
                for (int cellX = 0; cellX < width; cellX++)
                {
                    gridTiles[cellX, cellY] = new GridTile(gridImage, cellX, cellY);
                    PlaceTile(gridTiles[cellX, cellY], new CCPoint(cellX, cellY));
                }
            }
        }

        public void PlaceTile(CCNode tile, CCPoint loc)
        {
            tile.Position = GetScreenPosFromTilePos(loc);
            gridTiles[(int)loc.X, (int)loc.Y].filled = true;
            this.AddChild(tile);
        }

        public void PlaceGamePiece(GamePiece gamePiece, CCPoint loc)
        {
            gamePiece.Position = GetScreenPosFromTilePos(loc);
            gamePiece.gridPos = loc;
            gridTiles[(int)loc.X, (int)loc.Y].filled = true;
            this.AddChild(gamePiece);
        }

        public CCPoint GetScreenPosFromTilePos(CCPoint tilePos)
        {
            CCPoint screenLoc = CCPoint.Zero;
            screenLoc.X = (tilePos.X * tile_width / 2) + (tilePos.Y * tile_width / 2);
            screenLoc.Y = (tilePos.Y * tile_height / 2) - (tilePos.X * tile_height / 2);
            return screenLoc;
        }

        public CCPoint GetTileFromScreenTouch(CCPoint touch)
        {
            CCPoint tileLoc = CCPoint.Zero;
            float TILE_WIDTH_HALF = tile_width / 2;
            float TILE_HEIGHT_HALF = tile_height / 2;

            tileLoc.X = (touch.X / TILE_WIDTH_HALF - touch.Y / TILE_HEIGHT_HALF) / 2;
            tileLoc.Y = (touch.Y / TILE_HEIGHT_HALF + (touch.X / TILE_WIDTH_HALF)) / 2;

            return tileLoc;
        }

        public void LeaveTile(CCPoint loc)
        {
            gridTiles[(int)loc.X, (int)loc.Y].filled = false;
        }

        public CCPoint FindNextMove(CCPoint start, CCPoint end)
        {
            CCPoint dest = start;
            if (start.X < end.X)
                dest.X += 1;
            else if (start.X > end.X)
                dest.X -= 1;
            if (start.Y < end.Y)
                dest.Y += 1;
            else if (start.Y > end.Y)
                dest.Y -= 1;
            return dest;
        }
    }
}
