using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Empire
{
    public class MenuScreen
    {
        double prevTime;
        double prevMS;
        bool mapCreated;

        SpriteFont font;
        Rectangle playButton;
        Texture2D playButtonTexture;

        public MenuScreen(ContentManager content)
        {
            LoadContent(content);
            playButton = new Rectangle((GameVariables.GAME_WIDTH / 2) - 200, (GameVariables.GAME_HEIGHT / 2) - 64, 400, 128);
            prevTime = 0;
            prevMS = 0;
            mapCreated = false;
        }

        /// <summary>
        /// Returns true if the specified Rectangle is clicked on
        /// </summary>
        /// <param name="m">MouseState to check</param>
        /// <param name="r">Rectangle to check</param>
        /// <param name="gameTime">Game time</param>
        /// <returns>True if the rectangle was clicked on, false otherwise</returns>
        public bool ClickedOn(MouseState m, Rectangle r, GameTime gameTime)
        {
            if ((gameTime.TotalGameTime.TotalMilliseconds - prevMS) > 300)
            {
                if (m.X >= r.X && m.X <= (r.X + r.Width) && m.Y >= r.Y && m.Y <= (r.Y + r.Height))
                {
                    prevMS = gameTime.TotalGameTime.TotalMilliseconds;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Loads menu content
        /// </summary>
        /// <param name="content">Global content manager</param>
        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("TitleFont");
            playButtonTexture = content.Load<Texture2D>("PlayButton");
        }

        /// <summary>
        /// Updates the game if the player is currently on the menu screen
        /// </summary>
        /// <param name="gameTime">Game timing values</param>
        /// <param name="content">Global content manager</param>
        public void Update(GameTime gameTime, ContentManager content)
        {
            MouseState mstate = Mouse.GetState();
            bool lclick = false;

            // Check for a single click
            if (mstate.LeftButton == ButtonState.Pressed && (gameTime.TotalGameTime.TotalSeconds - prevTime > 1))
            {
                lclick = true;
            }

            // Check where a left click was
            if (lclick)
            {
                if (ClickedOn(mstate, playButton, gameTime))
                {
                    Game1.CurrentScreenState = Game1.ScreenState.game;
                    if (!mapCreated)
                    {
                        Game1.Map = CreateMap();
                    }
                }
            }
        }

        /// <summary>
        /// Draws the game if the player is currently on the menu screen
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(font, "Hello World!", new Vector2(750, 400), Color.Green);
            spriteBatch.Draw(playButtonTexture, playButton, Color.White);
        }

        /// <summary>
        /// Creates a map for the game world
        /// </summary>
        /// <returns>A 2D Tile array that contains the map tiles</returns>
        public Tile[,] CreateMap()
        {
            Tile[,] map = new Tile[GameVariables.MAP_WIDTH, GameVariables.MAP_HEIGHT];
            try
            {
                using (StreamReader sr = new StreamReader("DefaultMap.txt"))
                {
                    char c;
                    for (int i = 0; i < GameVariables.MAP_HEIGHT; i++)
                    {
                        for (int j = 0; j < GameVariables.MAP_WIDTH; j++)
                        {
                            c = (char)sr.Read();
                            if (c == 'w')
                            {
                                map[j, i] = new Tile(false);
                            }
                            else if (c == 'l')
                            {
                                map[j, i] = new Tile(true);
                            }
                            else if (c == '*')
                            {
                                map[j, i] = new City(new Vector2(j, i));
                                Game1.CityList.Add((City)map[j, i]);
                            }
                        }
                        sr.ReadLine();
                    }
                }
            }
            catch (FileNotFoundException fnfex)
            {
                Game1.CurrentScreenState = Game1.ScreenState.crash;
                Game1.Errormsg = fnfex.Message;
            }
            catch (IndexOutOfRangeException ioorex)
            {
                Game1.CurrentScreenState = Game1.ScreenState.crash;
                Game1.Errormsg = ioorex.Message;
            }
            catch (Exception ex)
            {
                Game1.CurrentScreenState = Game1.ScreenState.crash;
                Game1.Errormsg = ex.Message + " --- Ya dun fucked up A-aron";
            }
            return map;
        }
    }
}
