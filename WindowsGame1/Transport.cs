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
    public class Transport : Unit
    {
        public Transport(Vector2 loc, int playernum)
            : base(loc, UnitType.transport, playernum, GameVariables.TRANSPORT_HEALTH, GameVariables.TRANSPORT_DAMAGE, GameVariables.TRANSPORT_MOVES)
        {
            Capacity = GameVariables.TRANSPORT_CAPACITY;
        }

        public override void refreshMoves()
        {
            Moves = GameVariables.TRANSPORT_MOVES;
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