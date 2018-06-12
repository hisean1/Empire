using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Empire
{
    public class Tile
    {
        private bool isLand;
        private bool isOccupied;
        private bool isDiscovered;
        private Unit unitOnTile;
        private int owner;
        
        public Tile(bool land)
        {
            isLand = land;
            isOccupied = false;
            isDiscovered = true;
            unitOnTile = null;
        }

        // isLand Property
        public bool IsLand
        {
            get { return isLand; }
        }

        // isDiscovered Property
        public bool IsDiscovered
        {
            get { return isDiscovered; }
            set { isDiscovered = value; }
        }

        // isOccupied Property
        public bool IsOccupied
        {
            get { return isOccupied; }
            set { isOccupied = value; }
        }

        // unitOnTile Property
        public Unit UnitOnTile
        {
            get { return unitOnTile; }
            set { unitOnTile = value; }
        }

        public int Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        // color Property
        public Color CityColor
        {
            get
            {
                switch (owner)
                {
                    case 1:
                        return GameVariables.p1color;
                    case 2:
                        return GameVariables.p2color;
                    case 3:
                        return GameVariables.p3color;
                    case 4:
                        return GameVariables.p4color;
                    case 5:
                        return GameVariables.p5color;
                    case 6:
                        return GameVariables.p6color;
                    default:
                        return Color.Black;
                }
            }
        }
    }
}
