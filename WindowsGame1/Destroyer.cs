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
    public class Destroyer : Unit
    {
        public Destroyer(Vector2 loc, int playernum)
            : base(loc/*, false*/, UnitType.destroyer, playernum, GameVariables.DESTROYER_HEALTH, GameVariables.DESTROYER_DAMAGE, GameVariables.DESTROYER_MOVES)
        {
        }

        public override void refreshMoves()
        {
            Moves = GameVariables.DESTROYER_MOVES;
        }
    }
}
