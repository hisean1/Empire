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
    public class Sub : Unit
    {
        public Sub(Vector2 loc, int playernum)
            : base(loc/*, false*/, UnitType.sub, playernum, GameVariables.SUB_HEALTH, GameVariables.SUB_DAMAGE, GameVariables.SUB_MOVES)
        {
        }

        public override void refreshMoves()
        {
            Moves = GameVariables.SUB_MOVES;
        }
    }
}
