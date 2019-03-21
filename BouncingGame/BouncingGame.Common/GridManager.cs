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
        GridTile[,] cells;
        int gridHeight;
        int gridWidth;
        CCSprite gridSprite;
        float tile_width;
        float tile_height;

        public GridManager(int width, int height, string gridImage)
        {
            gridHeight = height;
            gridWidth = width;
            cells = new GridTile[width, height];

            gridSprite = new CCSprite(gridImage);
            tile_width = gridSprite.ScaledContentSize.Width;
            tile_height = gridSprite.ScaledContentSize.Height;

            for (int cellY = 0; cellY < height; cellY++)
            {
                for (int cellX = 0; cellX < width; cellX++)
                {
                    cells[cellX, cellY] = new GridTile(gridImage, cellX, cellY);
                    PlaceItem(cells[cellX, cellY], new CCPoint(cellX, cellY));
                }
            }
        }

        public void PlaceItem(CCNode nodeItem, CCPoint loc)
        {
            float screenX, screenY;
            screenX = (loc.X * tile_width / 2) + (loc.Y * tile_width / 2);
            screenY = (loc.Y * tile_height / 2) - (loc.X * tile_height / 2);
            nodeItem.Position = new CCPoint(screenX, screenY);
            cells[(int)loc.X, (int)loc.Y].filled = true;
            this.AddChild(nodeItem);
        }

        public void LeaveTile(CCPoint loc)
        {
            cells[(int)loc.X, (int)loc.Y].filled = false;
        }

        public CCPoint FindNextMove(CCPoint start, CCPoint end)
        {
            CCPoint dest = start;
            if (start.X < end.X)
                dest.X -= 1;
            else if (start.X > end.X)
                dest.X += 1;
            if (start.Y < end.Y)
                dest.Y -= 1;
            else if (start.Y > end.Y)
                dest.Y += 1;
            return dest;
        }
    }
}
