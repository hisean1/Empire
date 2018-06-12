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
    public class Battleship : Unit
    {
        public Battleship(Vector2 loc, int playernum)
            : base(loc, UnitType.battleship, playernum, GameVariables.BATTLESHIP_HEALTH, GameVariables.BATTLESHIP_DAMAGE, GameVariables.BATTLESHIP_MOVES)
        {
        }

        public override void refreshMoves()
        {
            Moves = GameVariables.BATTLESHIP_MOVES;
        }
    }
}