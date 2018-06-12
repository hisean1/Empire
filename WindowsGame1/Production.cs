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
    class Production
    {
        Unit.UnitType prod;

        public Unit.UnitType Prod
        {
            get { return prod; }
            set { prod = value; }
        }

        public Production(ContentManager content)
        {
            LoadContent(content);
            prod = Unit.UnitType.army;
        }

        public void LoadContent(ContentManager content)
        {
            
        }

        public void Update(GameTime gt, ContentManager content)
        {
            KeyboardState kstate = Keyboard.GetState();
            MouseState mstate = Mouse.GetState();

            if (kstate.IsKeyDown(Keys.A))
            {
                prod = Unit.UnitType.army;
            }
            if (kstate.IsKeyDown(Keys.F))
            {
                prod = Unit.UnitType.fighter;
            }
            if (kstate.IsKeyDown(Keys.T))
            {
                prod = Unit.UnitType.transport;
            }
            if (kstate.IsKeyDown(Keys.D))
            {
                prod = Unit.UnitType.destroyer;
            }
            if (kstate.IsKeyDown(Keys.S))
            {
                prod = Unit.UnitType.sub;
            }
            if (kstate.IsKeyDown(Keys.R))
            {
                prod = Unit.UnitType.cruiser;
            }
            if (kstate.IsKeyDown(Keys.C))
            {
                prod = Unit.UnitType.carrier;
            }
            if (kstate.IsKeyDown(Keys.B))
            {
                prod = Unit.UnitType.battleship;
            }
            if (kstate.IsKeyDown(Keys.Enter))
            {
                Game1.CurrentScreenState = Game1.ScreenState.game;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
