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
    public class Carrier : Unit
    {
        public Carrier(Vector2 loc, int playernum)
            : base(loc, UnitType.carrier, playernum, GameVariables.CARRIER_HEALTH, GameVariables.CARRIER_DAMAGE, GameVariables.CARRIER_MOVES)
        {
            Capacity = GameVariables.CARRIER_CAPACITY;
        }

        public override void refreshMoves()
        {
            Moves = GameVariables.CARRIER_MOVES;
        }

        /// <summary>
        /// Loads a unit
        /// </summary>
        /// <param name="u">Unit to load</param>
        public override bool loadUnit(Unit u)
        {
            if (Cargo.Count < Capacity)
            {
                Cargo.Add(u);
                return true;
            }
            return false;
        }
    }
}
