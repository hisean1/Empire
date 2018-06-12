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
    public class Fighter : Unit
    {
        public Fighter(Vector2 loc, int playernum)
            : base(loc/*, false*/, UnitType.fighter, playernum, GameVariables.FIGHTER_HEALTH, GameVariables.FIGHTER_DAMAGE, GameVariables.FIGHTER_MOVES)
        {
            Range = 20;
        }

        public override void enterCity()
        {
            Range = GameVariables.FIGHTER_RANGE;
            Moves = 0;
            Hidden = true;
        }

        public override void refreshMoves()
        {
            Moves = GameVariables.FIGHTER_MOVES;
        }
    }
}