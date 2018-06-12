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
    public class City: Tile
    {
        private Vector2 location;
        private Unit.UnitType production;
        private int turnsRemaining;

        public City(Vector2 loc)
            : base(true)
        {
            location = loc;
            turnsRemaining = 999999;
        }

        public Vector2 Location
        {
            get { return location; }
        }

        // production Property
        public Unit.UnitType Production
        {
            get { return production; }
            set { production = value; }
        }

        public int TurnsRemaining
        {
            get { return turnsRemaining; }
            set { turnsRemaining = value; }
        }

        public string toString()
        {
            string s = "";
            switch (production)
            {
                case Unit.UnitType.army:
                    s = "armies";
                    break;
                case Unit.UnitType.fighter:
                    s = "fighters";
                    break;
                case Unit.UnitType.transport:
                    s = "transports";
                    break;
                case Unit.UnitType.destroyer:
                    s = "destroyers";
                    break;
                case Unit.UnitType.sub:
                    s = "submarines";
                    break;
                case Unit.UnitType.cruiser:
                    s = "cruisers";
                    break;
                case Unit.UnitType.carrier:
                    s = "carriers";
                    break;
                case Unit.UnitType.battleship:
                    s = "battleships";
                    break;
                default:
                    s = "unidentified units";
                    break;
            }
            return "City at " + location.X + ", " + location.Y + " producing " + s + " in " + turnsRemaining + " turns";
        }

        public void changeProduction(Unit.UnitType newProd)
        {
            switch (newProd)
            {
                case Unit.UnitType.army:
                    production = Unit.UnitType.army;
                    turnsRemaining = (int)(GameVariables.ARMY_BUILD_TIME * GameVariables.INITIAL_PRODUCTION_MODIFIER);
                    break;
                case Unit.UnitType.fighter:
                    production = Unit.UnitType.fighter;
                    turnsRemaining = (int)(GameVariables.FIGHTER_BUILD_TIME * GameVariables.INITIAL_PRODUCTION_MODIFIER);
                    break;
                case Unit.UnitType.transport:
                    production = Unit.UnitType.transport;
                    turnsRemaining = (int)(GameVariables.TRANSPORT_BUILD_TIME * GameVariables.INITIAL_PRODUCTION_MODIFIER);
                    break;
                case Unit.UnitType.destroyer:
                    production = Unit.UnitType.destroyer;
                    turnsRemaining = (int)(GameVariables.DESTROYER_BUILD_TIME * GameVariables.INITIAL_PRODUCTION_MODIFIER);
                    break;
                case Unit.UnitType.sub:
                    production = Unit.UnitType.sub;
                    turnsRemaining = (int)(GameVariables.SUB_BUILD_TIME * GameVariables.INITIAL_PRODUCTION_MODIFIER);
                    break;
                case Unit.UnitType.cruiser:
                    production = Unit.UnitType.cruiser;
                    turnsRemaining = (int)(GameVariables.CRUISER_BUILD_TIME * GameVariables.INITIAL_PRODUCTION_MODIFIER);
                    break;
                case Unit.UnitType.carrier:
                    production = Unit.UnitType.carrier;
                    turnsRemaining = (int)(GameVariables.CARRIER_BUILD_TIME * GameVariables.INITIAL_PRODUCTION_MODIFIER);
                    break;
                case Unit.UnitType.battleship:
                    production = Unit.UnitType.battleship;
                    turnsRemaining = (int)(GameVariables.BATTLESHIP_BUILD_TIME * GameVariables.INITIAL_PRODUCTION_MODIFIER);
                    break;
                default:
                    turnsRemaining = 999999;
                    break;
            }
        }
    }
}
