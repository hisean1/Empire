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
    public class Cruiser : Unit
    {
        public Cruiser(Vector2 loc, int playernum)
            : base(loc, UnitType.cruiser, playernum, GameVariables.CRUISER_HEALTH, GameVariables.CRUISER_DAMAGE, GameVariables.CRUISER_MOVES)
        {
        }

        public override void refreshMoves()
        {
            Moves = GameVariables.CRUISER_MOVES;
        }
    }
}