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
    public class Army: Unit
    {
        public Army(Vector2 loc, int playernum)
            : base(loc, UnitType.army, playernum, GameVariables.ARMY_HEALTH, GameVariables.ARMY_DAMAGE, GameVariables.ARMY_MOVES)
        {
        }

        public override void refreshMoves()
        {
            Moves = GameVariables.ARMY_MOVES;
        }
    }
}
