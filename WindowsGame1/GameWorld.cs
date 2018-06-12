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
    class GameWorld
    {
        #region Map Textures
        Texture2D landTileTexture;
        Texture2D waterTileTexture;
        Texture2D unknownTileTexture;
        Texture2D cityTexture;
        #endregion

        #region Unit Textures
        Texture2D armyTexture;
        Texture2D transportTexture;
        Texture2D fighterTexture;
        Texture2D destroyerTexture;
        Texture2D subTexture;
        Texture2D cruiserTexture;
        Texture2D carrierTexture;
        Texture2D battleshipTexture;
        #endregion

        #region Selection Textures
        Texture2D currentUnitTile;
        Texture2D cursorTile;
        #endregion

        #region Fonts
        SpriteFont infoFont;
        #endregion

        #region Local Variables
        Rectangle cursorTileLocation;           // Where to draw cursors tile highlight
        Rectangle currentUnitLocation;          // Where to draw current unit highlight
        string locationInfo = "";               // Panel location info string
        string unitInfo = "";                   // Panel unit info string
        string locationUnitInfo = "";           // Panel location unit info string
        string combatInfo = "";                 // Panel combat information string
        string combatInfo2 = "";                // Panel combat information string 2
        string debugInfo = "";                  // Panel debug information string
        string confirmDialog = "";              // Panel confirmation dialog
        int playerTurn = 1;                     // (1-6 player who is moving)
        int turnNumber = 0;                     // Global turn number
        int currentUnitNum = 0;                 // Current unit
        int placeholderUnitNum = 0;             // Placeholder for unit creation
        bool creatingUnit = false;              // New unit mode (allow new unit to move)
        bool surgeProtection = false;           // Stops one keypress from moving multiple units
        bool confirmMove = false;               // True if user is confirming a (probably stupid) move
        bool unitDestroyed = false;             // True if a unit has been destroyed in this update
        bool fighterLanding = false;            // True if a fighter is safely out of range
        bool transportLoading = false;          // True if a transport is being loaded
        Vector2 confirmLocation;                // Location user is attempting to move to
        #endregion

        #region Unit Lists
        List<Unit> pUnits = new List<Unit>();
        List<Unit> ai1Units = new List<Unit>();
        List<Unit> ai2Units = new List<Unit>();
        #endregion

        #region City Lists
        List<City> pCities = new List<City>();
        List<City> ai1Cities = new List<City>();
        List<City> ai2Cities = new List<City>();
        #endregion

        public int TurnNumber
        {
            get { return turnNumber; }
        }

        public GameWorld(ContentManager content, Production p)
        {
            LoadContent(content);
            confirmDialog = "Are you sure? (Y) or (N)";

            #region Test Units
            Fighter testArmy = new Fighter(new Vector2(4, 18), 1);
            pUnits.Add(testArmy);
            Fighter testArmy2 = new Fighter(new Vector2(5, 18), 1);
            pUnits.Add(testArmy2);
            Carrier testTransport = new Carrier(new Vector2(4, 20), 1);
            pUnits.Add(testTransport);
            #endregion
        }

        public void LoadContent(ContentManager content)
        {
            #region Load Map Textures
            landTileTexture = content.Load<Texture2D>("Land");
            waterTileTexture = content.Load<Texture2D>("Water");
            unknownTileTexture = content.Load<Texture2D>("Unknown");
            cityTexture = content.Load<Texture2D>("City");
            #endregion

            #region Load Unit Textures
            armyTexture = content.Load<Texture2D>("Army");
            transportTexture = content.Load<Texture2D>("Transport");
            fighterTexture = content.Load<Texture2D>("Fighter");
            destroyerTexture = content.Load<Texture2D>("Destroyer");
            subTexture = content.Load<Texture2D>("Sub");
            cruiserTexture = content.Load<Texture2D>("Cruiser");
            carrierTexture = content.Load<Texture2D>("Carrier");
            battleshipTexture = content.Load<Texture2D>("Battleship");
            #endregion

            #region Load Selection Textures
            currentUnitTile = content.Load<Texture2D>("CurrentUnit");
            cursorTile = content.Load<Texture2D>("CursorTile");
            #endregion

            #region Load Fonts
            infoFont = content.Load<SpriteFont>("InfoFont");
            #endregion
        }

        public void Update(GameTime gameTime, ContentManager content)
        {
            #region State Info
            MouseState mstate = Mouse.GetState();
            KeyboardState kstate = Keyboard.GetState();

            bool mouseInGame = false;
            if (mstate.Y < GameVariables.GAME_HEIGHT &&
                mstate.X < GameVariables.GAME_WIDTH &&
                mstate.Y >= 0 && mstate.X >= 0)
            {
                mouseInGame = true;
            }

            int tileX = mstate.X / GameVariables.TILE_SIZE;
            int tileY = mstate.Y / GameVariables.TILE_SIZE;
            Tile mouseTile = new Tile(false);
            if (mouseInGame)
            {
                // DONT MODIFY THIS TILE IT IS A COPY
                mouseTile = Game1.Map[tileX, tileY];
            }
            #endregion

            #region Update Cursor Tile
            locationInfo = "";
            locationUnitInfo = "";
            if (mouseTile.IsOccupied)
            {
                locationUnitInfo += "ESTOY OCCUPADO MAMA | ";
            }
            if (mstate.Y < GameVariables.GAME_HEIGHT &&
                mstate.X < GameVariables.GAME_WIDTH &&
                mstate.Y >= 0 && mstate.X >= 0)
            {
                cursorTileLocation = new Rectangle(tileX * GameVariables.TILE_SIZE - 2,
                                                   tileY * GameVariables.TILE_SIZE - 2,
                                                   20, 20);
                locationInfo = "X: " + tileX + " | Y: " + tileY + " | O: " + Game1.Map[tileX, tileY].IsOccupied;
                foreach (Unit u in pUnits)
                {
                    if (u.Location.X == tileX && u.Location.Y == tileY)
                    {
                        locationUnitInfo = u.toString();
                    }
                }
                foreach (City c in Game1.CityList)
                {
                    if (c.Location.X == tileX && c.Location.Y == tileY)
                    {
                        locationUnitInfo = c.toString();
                    }
                }

                // TODO: add info for enemy units
            }
            #endregion

            if (surgeProtection && kstate.GetPressedKeys().Length == 0)
            {
                surgeProtection = false;
            }

            if (pCities.Count == 0)
            {
                if (turnNumber == 0)    // Game starting, give player starting city
                {
                    captureCity(Game1.CityList[0], 1);  // fix eventually
                }
                else                    // Player has lost
                {
                    // lose here
                }
            }

            if (playerTurn == 1)    // Player is moving
            {
                #region Player Turn
                #region New Turn
                if (currentUnitNum >= pUnits.Count + pCities.Count)  // Refresh units moves and advance turn counter
                {
                    foreach (Unit u in pUnits)
                    {
                        u.refreshMoves();
                    }
                    foreach (City c in pCities)
                    {
                        c.TurnsRemaining--;
                    }
                    currentUnitNum = 0;
                    turnNumber++;
                    //playerTurn++;
                    //playerTurn = playerTurn%numPlayers;
                }
                #endregion

                // Update Current Unit
                if (currentUnitNum < pUnits.Count)  // Current unit is a unit
                {
                    #region Update Panel Info
                    currentUnitLocation = new Rectangle((int)((pUnits[currentUnitNum].Location.X * GameVariables.TILE_SIZE) - 2),
                                                        (int)((pUnits[currentUnitNum].Location.Y * GameVariables.TILE_SIZE) - 2),
                                                        20, 20);
                    unitInfo = pUnits[currentUnitNum].toString();
                    #endregion

                    if (pUnits[currentUnitNum].Moves == 0)  // Old unit is out of moves, advance to next
                    {
                        if (pUnits[currentUnitNum] is Fighter)  // Special fighter handling
                        {
                            if (pUnits[currentUnitNum].Range == 0)  // Fighter out of range exception
                            {
                                if (fighterLanding) // Safe landing
                                {
                                    pUnits[currentUnitNum].enterCity();
                                }
                                else
                                {
                                    destroyUnit(pUnits[currentUnitNum]);
                                }
                            }
                            fighterLanding = false;
                        }
                        if (creatingUnit)
                        {
                            currentUnitNum = placeholderUnitNum;
                            creatingUnit = false;
                        }
                        else
                        {
                            currentUnitNum++;
                        }
                    }
                }
                else                                // Current unit is a city
                {/*
                    #region Update Panel Info
                        currentUnitLocation = new Rectangle((int)((pCities[currentUnitNum - pUnits.Count].Location.X * GameVariables.TILE_SIZE) - 2),
                                                            (int)((pCities[currentUnitNum - pUnits.Count].Location.Y * GameVariables.TILE_SIZE) - 2),
                                                            20, 20);
                        unitInfo = pCities[currentUnitNum - pUnits.Count].toString();
                        #endregion
                    */
                    if (pCities[currentUnitNum - pUnits.Count].TurnsRemaining == 0) // Check if city has produced a unit
                    {
                        #region Produce unit
                        switch (pCities[currentUnitNum - pUnits.Count].Production)
                        {
                            case Unit.UnitType.army:
                                pCities[currentUnitNum - pUnits.Count].TurnsRemaining = GameVariables.ARMY_BUILD_TIME;
                                pUnits.Add(new Army(pCities[currentUnitNum - pUnits.Count].Location, 1));
                                break;
                            case Unit.UnitType.fighter:
                                pCities[currentUnitNum - pUnits.Count].TurnsRemaining = GameVariables.FIGHTER_BUILD_TIME;
                                pUnits.Add(new Fighter(pCities[currentUnitNum - pUnits.Count].Location, 1));
                                break;
                            case Unit.UnitType.transport:
                                pCities[currentUnitNum - pUnits.Count].TurnsRemaining = GameVariables.TRANSPORT_BUILD_TIME;
                                pUnits.Add(new Transport(pCities[currentUnitNum - pUnits.Count].Location, 1));
                                break;
                            case Unit.UnitType.destroyer:
                                pCities[currentUnitNum - pUnits.Count].TurnsRemaining = GameVariables.DESTROYER_BUILD_TIME;
                                pUnits.Add(new Destroyer(pCities[currentUnitNum - pUnits.Count].Location, 1));
                                break;
                            case Unit.UnitType.sub:
                                pCities[currentUnitNum - pUnits.Count].TurnsRemaining = GameVariables.SUB_BUILD_TIME;
                                pUnits.Add(new Sub(pCities[currentUnitNum - pUnits.Count].Location, 1));
                                break;
                            case Unit.UnitType.cruiser:
                                pCities[currentUnitNum - pUnits.Count].TurnsRemaining = GameVariables.CRUISER_BUILD_TIME;
                                pUnits.Add(new Cruiser(pCities[currentUnitNum - pUnits.Count].Location, 1));
                                break;
                            case Unit.UnitType.carrier:
                                pCities[currentUnitNum - pUnits.Count].TurnsRemaining = GameVariables.CARRIER_BUILD_TIME;
                                pUnits.Add(new Carrier(pCities[currentUnitNum - pUnits.Count].Location, 1));
                                break;
                            case Unit.UnitType.battleship:
                                pCities[currentUnitNum - pUnits.Count].TurnsRemaining = GameVariables.BATTLESHIP_BUILD_TIME;
                                pUnits.Add(new Battleship(pCities[currentUnitNum - pUnits.Count].Location, 1));
                                break;
                            default:
                                break;
                        }
                        placeholderUnitNum = currentUnitNum + 2;
                        currentUnitNum = pUnits.Count - 1;
                        creatingUnit = true;
                        #endregion
                    }
                    else    // Nothing to produce
                    {
                        currentUnitNum++;
                    }
                }

                bool cancelMove;

                // Check keyboard input
                #region Q press
                if (kstate.IsKeyDown(Keys.Q) && !surgeProtection && !confirmMove)
                {
                    surgeProtection = true;

                    if (currentUnitNum < pUnits.Count)  // anti-stupid
                    {
                        // Collision detection
                        cancelMove = handleCollisions(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X - 1, pUnits[currentUnitNum].Location.Y - 1));

                        if (!cancelMove)
                        {
                            moveUnit(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X - 1, pUnits[currentUnitNum].Location.Y - 1));
                        }
                    }
                }
                #endregion
                #region W press
                if (kstate.IsKeyDown(Keys.W) && !surgeProtection && !confirmMove)
                {
                    surgeProtection = true;

                    if (currentUnitNum < pUnits.Count)  // anti-stupid
                    {
                        cancelMove = handleCollisions(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X, pUnits[currentUnitNum].Location.Y - 1));

                        if (!cancelMove)
                        {
                            moveUnit(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X, pUnits[currentUnitNum].Location.Y - 1));
                        }
                    }
                }
                #endregion
                #region E press
                if (kstate.IsKeyDown(Keys.E) && !surgeProtection && !confirmMove)
                {
                    surgeProtection = true;

                    if (currentUnitNum < pUnits.Count)  // anti-stupid
                    {
                        cancelMove = handleCollisions(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X + 1, pUnits[currentUnitNum].Location.Y - 1));

                        if (!cancelMove)
                        {
                            moveUnit(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X + 1, pUnits[currentUnitNum].Location.Y - 1));
                        }
                    }
                }
                #endregion
                #region A press
                if (kstate.IsKeyDown(Keys.A) && !surgeProtection && !confirmMove)
                {
                    surgeProtection = true;

                    if (currentUnitNum < pUnits.Count)  // anti-stuipd
                    {
                        cancelMove = handleCollisions(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X - 1, pUnits[currentUnitNum].Location.Y));

                        if (!cancelMove)
                        {
                            moveUnit(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X - 1, pUnits[currentUnitNum].Location.Y));
                        }
                    }
                }
                #endregion
                #region D press
                if (kstate.IsKeyDown(Keys.D) && !surgeProtection && !confirmMove)
                {
                    surgeProtection = true;

                    if (currentUnitNum < pUnits.Count)  // anti-stupid
                    {
                        cancelMove = handleCollisions(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X + 1, pUnits[currentUnitNum].Location.Y));

                        if (!cancelMove)
                        {
                            moveUnit(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X + 1, pUnits[currentUnitNum].Location.Y));
                        }
                    }
                }
                #endregion
                #region Z press
                if (kstate.IsKeyDown(Keys.Z) && !surgeProtection && !confirmMove)
                {
                    surgeProtection = true;

                    if (currentUnitNum < pUnits.Count)  // anti-stupid
                    {
                        cancelMove = handleCollisions(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X - 1, pUnits[currentUnitNum].Location.Y + 1));

                        if (!cancelMove)
                        {
                            moveUnit(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X - 1, pUnits[currentUnitNum].Location.Y + 1));
                        }
                    }
                }
                #endregion
                #region X press
                if (kstate.IsKeyDown(Keys.X) && !surgeProtection && !confirmMove)
                {
                    surgeProtection = true;

                    if (currentUnitNum < pUnits.Count)  // anti-stupid
                    {
                        cancelMove = handleCollisions(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X, pUnits[currentUnitNum].Location.Y + 1));

                        if (!cancelMove)
                        {
                            moveUnit(pUnits[currentUnitNum], new Vector2(pUnits[currentUnitNum].Location.X, pUnits[currentUnitNum].Location.Y + 1));
                        }
                    }
                }
                #endregion
                #region C press
                if (kstate.IsKeyDown(Keys.C) && !surgeProtection && !confirmMove)
                {
                    surgeProtection = true;
                    Vector2 destination = new Vector2(pUnits[currentUnitNum].Location.X + 1, pUnits[currentUnitNum].Location.Y + 1);

                    if (currentUnitNum < pUnits.Count)  // anti-stupid
                    {
                        cancelMove = handleCollisions(pUnits[currentUnitNum], destination);
                        debugInfo += "cancelmove: " + cancelMove;
                        if (!cancelMove)
                        {
                            moveUnit(pUnits[currentUnitNum], destination);
                        }
                    }
                }
                #endregion
                #region Space press
                if (kstate.IsKeyDown(Keys.Space) && !surgeProtection && !confirmMove)
                {
                    surgeProtection = true;
                    Vector2 destination = new Vector2(pUnits[currentUnitNum].Location.X, pUnits[currentUnitNum].Location.Y);
                    
                    pUnits[currentUnitNum].Moves--;
                }
                #endregion
                #region Y press
                if (kstate.IsKeyDown(Keys.Y) && !surgeProtection && confirmMove)
                {
                    surgeProtection = true; // may not need surge protection for this
                    /*
                    pUnits[currentUnitNum].Location = confirmLocation;  // do the move
                     * */
                    handleCollisions(pUnits[currentUnitNum], confirmLocation);  // do the move?
                    if (!unitDestroyed)
                    {
                        pUnits[currentUnitNum].Moves--;
                    }
                    else
                    {
                        unitDestroyed = false;      // Reset unitDestroyed
                    }
                    confirmMove = false;            // Reset confirmation
                }
                #endregion
                #region N press
                if (kstate.IsKeyDown(Keys.N) && !surgeProtection && confirmMove)
                {
                    surgeProtection = true;

                    confirmMove = false;
                }
                #endregion
                #endregion
            }
            else
            {
                // Play AIs here
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw world
            try
            {
                Rectangle drawLocation;
                for (int i = 0; i < GameVariables.MAP_WIDTH; i++)
                {
                    for (int j = 0; j < GameVariables.MAP_HEIGHT; j++)
                    {
                        drawLocation = new Rectangle(i * GameVariables.TILE_SIZE, j * GameVariables.TILE_SIZE, GameVariables.TILE_SIZE, GameVariables.TILE_SIZE);
                        if (!Game1.Map[i, j].IsDiscovered)
                        {
                            spriteBatch.Draw(unknownTileTexture, drawLocation, Color.White);
                        }
                        else if (Game1.Map[i, j].IsLand)
                        {
                            spriteBatch.Draw(landTileTexture, drawLocation, Color.White);
                            if (Game1.Map[i, j] is City)
                            {
                                spriteBatch.Draw(cityTexture, drawLocation, Game1.Map[i, j].CityColor);
                            }
                        }
                        else
                        {
                            spriteBatch.Draw(waterTileTexture, drawLocation, Color.White);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Game1.CurrentScreenState = Game1.ScreenState.crash;
                Game1.Errormsg = ex.Message;
            }

            // Draw units

            // Draw player's units
            foreach (Unit u in pUnits)
            {
                if (!u.Hidden)  // Don't draw hidden units
                {
                    spriteBatch.Draw(getTexture(u), new Vector2(u.Location.X * GameVariables.TILE_SIZE, u.Location.Y * GameVariables.TILE_SIZE), GameVariables.p1color);
                }
            }

            // Draw current unit highlight
            spriteBatch.Draw(currentUnitTile, currentUnitLocation, Color.White);

            // Draw cursor tile highlight
            spriteBatch.Draw(cursorTile, cursorTileLocation, Color.White);

            // Draw info panel
            spriteBatch.DrawString(infoFont, locationInfo, GameVariables.LOCATION_INFO_LOCATION, Color.White);
            spriteBatch.DrawString(infoFont, unitInfo, GameVariables.UNIT_INFORMATION, Color.White);
            spriteBatch.DrawString(infoFont, locationUnitInfo, GameVariables.LOCATION_UNIT_INFO_LOCATION, Color.White);
            spriteBatch.DrawString(infoFont, "Turn #" + turnNumber, GameVariables.TURN_INFORMATION, Color.White);
            //spriteBatch.DrawString(infoFont, combatInfo, GameVariables.COMBAT_INFORMATION, Color.White);
            spriteBatch.DrawString(infoFont, debugInfo, GameVariables.DEBUG_INFORMATION, Color.White);
            if (confirmMove)
            {
                spriteBatch.DrawString(infoFont, confirmDialog, GameVariables.CONFIRM_DIALOG, Color.OrangeRed);
            }
            //spriteBatch.DrawString(infoFont, combatInfo2, GameVariables.COMBAT_INFO_2, Color.White);
        }

        /// <summary>
        /// Gets the texture required for a given unit
        /// </summary>
        /// <param name="u">Unit to find texture</param>
        /// <returns>Texture for unit</returns>
        public Texture2D getTexture(Unit u)
        {
            switch (u.Type)
            {
                case Unit.UnitType.army:
                    return armyTexture;
                case Unit.UnitType.transport:
                    return transportTexture;
                case Unit.UnitType.fighter:
                    return fighterTexture;
                case Unit.UnitType.destroyer:
                    return destroyerTexture;
                case Unit.UnitType.sub:
                    return subTexture;
                case Unit.UnitType.cruiser:
                    return cruiserTexture;
                case Unit.UnitType.carrier:
                    return carrierTexture;
                case Unit.UnitType.battleship:
                    return battleshipTexture;
                default:
                    return Game1.ErrorTexture;
            }
        }

        /// <summary>
        /// Captures a city for a given player
        /// </summary>
        /// <param name="c">City to be captured</param>
        /// <param name="player">Player capturing the city</param>
        private void captureCity(City c, int player)
        {
            switch (player)
            {
                case 1:
                    pCities.Add(c);
                    break;
                default:
                    break;
            }
            c.Owner = player;
            c.changeProduction(getNewProduction());
            combatInfo2 = "City at " + c.Location.X + ", " + c.Location.Y + " has been captured!";
        }

        /// <summary>
        /// Asks player for city production
        /// </summary>
        /// <returns>Unit to be produced</returns>
        private Unit.UnitType getNewProduction()
        {
            // TODO: Ask user for production
            //Production factory = new Production();
            Unit.UnitType production = Unit.UnitType.army;
            bool produce = false;
            while (!produce)
            {

                produce = true;
            }
            // always armies for now
            return production;
        }

        /// <summary>
        /// Handles collisions between a moving unit and its destination
        /// </summary>
        /// <param name="u">Unit that is moving</param>
        /// <param name="destination">Tile unit is moving to</param>
        /// <returns>Returns true if movement was cancelled as a result of the collision</returns>
        private bool handleCollisions(Unit u, Vector2 destination)
        {
            debugInfo = "";
            switch (u.Type) // Unit making the move
            {
                case Unit.UnitType.army:
                    #region Army Collision Handling
                    if (destination.X < GameVariables.MAP_WIDTH &&  // If still in game world
                       destination.X >= 0 &&
                       destination.Y < GameVariables.MAP_HEIGHT &&
                       destination.Y >= 0)
                    {
                        Tile destinationTile = Game1.map[(int)destination.X, (int)destination.Y];   // Destination tile
                        if (!destinationTile.IsLand)  // If tile is water
                        {
                            if (destinationTile.IsOccupied)    // Tile has unit
                            {
                                if (destinationTile.UnitOnTile.Player != u.Player)    // Tile has enemy
                                {
                                    if (!resolveCombat(u, destinationTile.UnitOnTile))
                                    {
                                        return true;
                                    }// FIGHT!
                                }
                                else        // Non-enemy water tile
                                {
                                    if (destinationTile.UnitOnTile is Transport)  // Attempting to load a transport
                                    {
                                        debugInfo = "Attempting to load transport";
                                        if (destinationTile.UnitOnTile.loadUnit(u)) // Transport successfully loads
                                        {
                                            debugInfo += "... loaded successfully";
                                            transportLoading = true;
                                            u.Embarked = true;
                                            u.Hidden = true;
                                        }
                                        else
                                        {
                                            debugInfo += "... transport is full";
                                            // problem: army is too fat
                                            if (!confirmMove)   // ask for confirmation
                                            {
                                                debugInfo += " | Confirming move";
                                                confirmMove = true; // try again with permission
                                                confirmLocation = destination;
                                                return true;
                                            }
                                            else    //DO IT, DO IT NOW
                                            {
                                                debugInfo += " | Overriding safeties";
                                                moveUnit(u, destination);
                                                destroyUnit(u);
                                                return true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // problem: friendly fire (water?)
                                        if (!confirmMove)   // ask for confirmation
                                        {
                                            debugInfo += " | Confirming move";
                                            confirmMove = true; // try again with permission
                                            confirmLocation = destination;
                                            return true;
                                        }
                                        else    //DO IT, DO IT NOW
                                        {
                                            debugInfo += " | Overriding safeties";
                                            destroyUnit(u);
                                            return true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // problem: army is taking a swim
                                if (!confirmMove)   // ask for confirmation
                                {
                                    debugInfo += " | Confirming move";
                                    confirmMove = true; // try again with permission
                                    confirmLocation = destination;
                                    return true;
                                }
                                else    //DO IT, DO IT NOW
                                {
                                    debugInfo += " | Overriding safeties";
                                    moveUnit(u, destination);
                                    destroyUnit(u);
                                    return true;
                                }
                            }
                        }
                        else if (destinationTile is City) // Tile is land and city
                        {
                            foreach (City c in Game1.CityList)
                            {
                                if (c.Location == destination && !pCities.Contains(c))  // Attacking an enemy city
                                {
                                    debugInfo += " -Attacking enemy city";
                                    resolveSiege((Army)u, c);
                                    return true;
                                }
                                else if (c.Location == destination)   // Attacking a friendly city
                                {
                                    // problem: traitor!
                                    if (!confirmMove)   // ask for confirmation
                                    {
                                        debugInfo += " | Confirming move";
                                        confirmMove = true; // try again with permission
                                        confirmLocation = destination;
                                        return true;
                                    }
                                    else    //DO IT, DO IT NOW
                                    {
                                        debugInfo += " | Overriding safeties";
                                        moveUnit(u, destination);
                                        destroyUnit(u);
                                        return true;
                                    }
                                }
                                // else, these are not the droids you are looking for
                            }
                        }
                        else if (destinationTile.IsOccupied)  // Tile is occupied
                        {
                            if (destination.X == u.Location.X && destination.Y == u.Location.Y) // Unit is holding position
                            {
                                moveUnit(u, destination);
                            }
                            else if (destinationTile.UnitOnTile.Player != u.Player)   // Tile is land enemy
                            {
                                return !resolveCombat(u, destinationTile.UnitOnTile);
                            }
                            else    // Tile is land friendly
                            {
                                // problem: friendly fire
                                if (!confirmMove)   // ask for confirmation
                                {
                                    debugInfo += " | Confirming move";
                                    confirmMove = true; // try again with permission
                                    confirmLocation = destination;
                                    return true;
                                }
                                else    //DO IT, DO IT NOW
                                {
                                    debugInfo += " | Overriding safeties";
                                    destroyUnit(destinationTile.UnitOnTile);  // Destroy unit on destination tile
                                    moveUnit(u, destination);   // Replace with current unit
                                }
                            }
                        }
                    }
                    else
                    {
                        // problem: army is a free thinker
                        // -- Nothing to do here; ignore input --
                        return true;
                    }
                    #endregion
                    break;
                case Unit.UnitType.fighter:
                    #region Fighter Collision Handling
                    if (destination.X < GameVariables.MAP_WIDTH &&  // If still in game world
                       destination.X >= 0 &&
                       destination.Y < GameVariables.MAP_HEIGHT &&
                       destination.Y >= 0)
                    {
                        Tile destinationTile = Game1.map[(int)destination.X, (int)destination.Y];   // Destination tile

                        if (destinationTile is City) // Tile is land and city
                        {
                            foreach (City c in Game1.CityList)
                            {
                                if (c.Location == destination && !pCities.Contains(c))  // Attacking an enemy city
                                {
                                    if (!confirmMove)   // ask for confirmation
                                    {
                                        debugInfo += " | Confirming move";
                                        confirmMove = true; // try again with permission
                                        confirmLocation = destination;
                                        return true;
                                    }
                                    else    //DO IT, DO IT NOW
                                    {
                                        debugInfo += " | Overriding safeties";
                                        moveUnit(u, destination);
                                        destroyUnit(u);
                                        return true;
                                    }
                                }
                                else if (c.Location == destination)   // Landing in city
                                {
                                    fighterLanding = true;
                                    moveUnit(u, destination);
                                    u.enterCity();
                                    return true;
                                }
                                // else, these are not the droids you are looking for
                            }
                        }
                        else if (destinationTile.IsOccupied)    // Tile has unit
                        {
                            if (destinationTile.UnitOnTile.Player != u.Player)    // Tile has enemy
                            {
                                if (!resolveCombat(u, destinationTile.UnitOnTile))
                                {
                                    return true;
                                }// FIGHT!
                            }
                            else    // Tile has friendly :)
                            {
                                if (destinationTile.UnitOnTile is Carrier)  // Attempting to load a carrier
                                {
                                    debugInfo = "Attempting to load transport";
                                    if (destinationTile.UnitOnTile.loadUnit(u)) // Transport successfully loads
                                    {
                                        debugInfo += "... loaded successfully";
                                        transportLoading = true;
                                        fighterLanding = true;
                                        u.Embarked = true;
                                        u.enterCity();
                                    }
                                    else
                                    {
                                        // problem: full carrier
                                        if (!confirmMove)   // ask for confirmation
                                        {
                                            debugInfo += " | Confirming move";
                                            confirmMove = true; // try again with permission
                                            confirmLocation = destination;
                                            return true;
                                        }
                                        else    //DO IT, DO IT NOW
                                        {
                                            debugInfo += " | Overriding safeties";
                                            moveUnit(u, destination);
                                            destroyUnit(u);
                                            return true;
                                        }
                                    }
                                }
                                else    // Friendly isn't a carrier
                                {
                                    // problem: friendly fire
                                    if (!confirmMove)   // ask for confirmation
                                    {
                                        debugInfo += " | Confirming move";
                                        confirmMove = true; // try again with permission
                                        confirmLocation = destination;
                                        return true;
                                    }
                                    else    //DO IT, DO IT NOW
                                    {
                                        debugInfo += " | Overriding safeties";
                                        destroyUnit(destinationTile.UnitOnTile);  // Destroy unit on destination tile
                                        moveUnit(u, destination);   // Replace with current unit
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // problem: fighter thinks it's Icarus
                        // -- Nothing to do here; ignore input --
                        return true;
                    }
                    #endregion
                    break;
                case Unit.UnitType.transport:
                // Standard ship collisions, fall through
                case Unit.UnitType.destroyer:
                // Standard ship collisions, fall through
                case Unit.UnitType.sub:
                // Standard ship collisions, fall through
                case Unit.UnitType.cruiser:
                // Standard ship collisions, fall through
                case Unit.UnitType.carrier:
                // Standard ship collisions, fall through
                case Unit.UnitType.battleship:
                    #region Ship Collision Handling
                    if (destination.X < GameVariables.MAP_WIDTH &&  // If still in game world
                       destination.X >= 0 &&
                       destination.Y < GameVariables.MAP_HEIGHT &&
                       destination.Y >= 0)
                    {
                        Tile destinationTile = Game1.map[(int)destination.X, (int)destination.Y];   // Destination tile

                        if (destinationTile is City) // Tile is land and city
                        {
                            foreach (City c in Game1.CityList)
                            {
                                if (c.Location == destination && !pCities.Contains(c))  // Attacking an enemy city
                                {
                                    if (!confirmMove)   // ask for confirmation
                                    {
                                        debugInfo += " | Confirming move";
                                        confirmMove = true; // try again with permission
                                        confirmLocation = destination;
                                        return true;
                                    }
                                    else    //DO IT, DO IT NOW
                                    {
                                        debugInfo += " | Overriding safeties";
                                        moveUnit(u, destination);
                                        destroyUnit(u);
                                        return true;
                                    }
                                }
                                else if (c.Location == destination)   // Docking in city
                                {
                                    debugInfo += " | Docking unit";
                                    moveUnit(u, destination);
                                    u.enterCity();
                                    u.Hidden = true;
                                    return true;
                                }
                                // else, these are not the droids you are looking for
                            }
                        }
                        else if (destinationTile.IsOccupied)    // Tile has unit
                        {
                            if (destinationTile.UnitOnTile.Player != u.Player)    // Tile has enemy
                            {
                                if (!resolveCombat(u, destinationTile.UnitOnTile))
                                {
                                    return true;
                                }// FIGHT!
                            }
                            else    // Tile has friendly :)
                            {
                                // problem: friendly fire
                                if (!confirmMove)   // ask for confirmation
                                {
                                    debugInfo += " | Confirming move";
                                    confirmMove = true; // try again with permission
                                    confirmLocation = destination;
                                    return true;
                                }
                                else    //DO IT, DO IT NOW
                                {
                                    debugInfo += " | Overriding safeties";
                                    if (!destinationTile.IsLand)
                                    {
                                        destroyUnit(destinationTile.UnitOnTile);  // Destroy unit on destination tile
                                        moveUnit(u, destination);   // Replace with current unit
                                    }
                                    else
                                    {
                                        destroyUnit(u);
                                    }
                                    return true;
                                }
                            }
                        }
                        else if (destinationTile.IsLand)  // Tile is empty land
                        {
                            if (!confirmMove)   // ask for confirmation
                            {
                                debugInfo += " | Confirming move";
                                confirmMove = true; // try again with permission
                                confirmLocation = destination;
                                return true;
                            }
                            else    //DO IT, DO IT NOW
                            {
                                debugInfo += " | Overriding safeties";
                                moveUnit(u, destination);
                                destroyUnit(u);
                                return true;
                            }
                        }
                        // Normal move
                    }
                    else
                    {
                        // problem: ship thinks it's Magellan
                        // -- Nothing to do here; ignore input --
                        return true;
                    }
                    #endregion
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Resolves combat between two units
        /// </summary>
        /// <param name="atk">Attacking unit</param>
        /// <param name="def">Defending unit</param>
        /// <returns>Returns true if attacker has won</returns>
        private bool resolveCombat(Unit atk, Unit def)
        {
            Random r = new Random();
            while (atk.HP > 0 && def.HP > 0)
            {
                if (r.Next(0, 1) == 1)
                {
                    def.HP -= atk.Damage;
                }
                else
                {
                    atk.HP -= def.Damage;
                }
            }

            if (atk is Carrier || atk is Transport)
            {
                destroyCargo(atk);
            }
            if (def is Carrier || def is Transport)
            {
                destroyCargo(def);
            }

            if (atk.HP <= 0)
            {
                destroyUnit(atk);
                return false;
            }
            destroyUnit(def);
            return true;
        }

        /// <summary>
        /// Destroys the cargo of a damaged transport or carrier
        /// </summary>
        /// <param name="u">Unit whose cargo to destroy</param>
        private void destroyCargo(Unit u)
        {
            if (u is Carrier)
            {
                while (u.Cargo.Count > (u.HP * GameVariables.CARRIER_CAPACITY_MODIFIER))
                {
                    u.Cargo.Remove(u.Cargo[u.Cargo.Count]); // Removes the last unit in cargo list
                }
            }
            else
            {
                while (u.Cargo.Count > (u.HP * GameVariables.TRANSPORT_CAPACITY_MODIFIER))
                {
                    u.Cargo.Remove(u.Cargo[u.Cargo.Count]); // Removes the last unit in cargo list
                }
            }
        }

        /// <summary>
        /// Resolves a siege when an army attacks a city
        /// </summary>
        /// <param name="a">Attacking army</param>
        /// <param name="c">Defending city</param>
        private void resolveSiege(Army a, City c)
        {
            combatInfo = "Army at " + a.Location.X + ", " + a.Location.Y + " attacking city at " + c.Location.X + ", " + c.Location.Y;
            Random r = new Random();
            int num = r.Next(0, 2); // 0 or 1
            combatInfo += " | Rolled a " + num;
            destroyUnit(a); // Happens before siege for combatInfo2 text accuracy
            if (num == 1)
            {
                captureCity(c, a.Player);
            }
        }

        /// <summary>
        /// Destroys the given unit
        /// </summary>
        /// <param name="u">Unit to be destroyed</param>
        private void destroyUnit(Unit u)
        {
            if (u is Carrier || u is Transport)
            {
                destroyCargo(u);
            }
            Game1.Map[(int)u.Location.X, (int)u.Location.Y].IsOccupied = false; // Remove from tile
            Game1.Map[(int)u.Location.X, (int)u.Location.Y].UnitOnTile = null;

            switch (u.Player)
            {
                case 1:
                    pUnits.Remove(u);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                default:
                    break;
            }
            combatInfo2 = u.toString() + " has been destroyed.";
            unitDestroyed = true;
        }

        /// <summary>
        /// Moves a unit to a location
        /// </summary>
        /// <param name="u">Unit to be moved</param>
        /// <param name="loc">Location to move to</param>
        private void moveUnit(Unit u, Vector2 loc)
        {
            if (!u.Embarked || transportLoading)
            {
                //debugInfo += " Unembarked move";
                if (!(Game1.Map[(int)u.Location.X, (int)u.Location.Y].UnitOnTile is Transport) ||
                    !(Game1.Map[(int)u.Location.X, (int)u.Location.Y].UnitOnTile is Carrier))  // Remove previous unit unless leaving a transport
                {
                    Game1.Map[(int)u.Location.X, (int)u.Location.Y].IsOccupied = false;   // Unit leaving tile
                    Game1.Map[(int)u.Location.X, (int)u.Location.Y].UnitOnTile = null;
                }
            }
            if(u.Embarked && !transportLoading)
            {
                Game1.Map[(int)u.Location.X, (int)u.Location.Y].UnitOnTile.Cargo.Remove(u/*
                    Game1.Map[(int)u.Location.X, (int)u.Location.Y].UnitOnTile.Cargo[
                        Game1.Map[(int)u.Location.X, (int)u.Location.Y].UnitOnTile.Cargo.Count - 1]*/);
                u.Embarked = false;
                u.Hidden = false;
            }
            u.Location = loc;
            if (!fighterLanding)
            {
                u.Moves--;
            }
            if (transportLoading)
            {
                transportLoading = false;
            }
            else
            {
                Game1.Map[(int)u.Location.X, (int)u.Location.Y].IsOccupied = true;    // Unit entering tile
                Game1.Map[(int)u.Location.X, (int)u.Location.Y].UnitOnTile = u;
            }
            if (u is Fighter && !fighterLanding)
            {
                u.Range--;
            }
            if (u is Transport || u is Carrier)
            {
                foreach (Unit c in u.Cargo)
                {
                    if (c != null)
                    {
                        c.Location = loc;
                    }
                }
            }
            if (!u.Embarked && u.Hidden)   // This seems to work
            {
                u.Hidden = false;
            }
        }
    }
}
