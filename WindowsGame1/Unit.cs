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
    public class Unit
    {
        Vector2 location;
        int hp;
        int damage;
        int moves;
        int player;
        int capacity;
        int range;
        bool hidden;
        bool embarked;
        List<Unit> cargo = new List<Unit>();
        public enum UnitType
        { city, army, fighter, sub, destroyer, transport, cruiser, carrier, battleship };
        UnitType type;

        public Unit(Vector2 loc, UnitType u, int playernum, int hits, int atk, int move)
        {
            location = new Vector2(loc.X, loc.Y);
            type = u;
            player = playernum;
            hp = hits;
            damage = atk;
            moves = move;
            hidden = false;
            embarked = false;
        }

        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        public int Damage
        {
            get { return damage; }
        }

        public int Moves
        {
            get { return moves; }
            set { moves = value; }
        }

        public int Player
        {
            get { return player; }
        }

        public UnitType Type
        {
            get { return type; }
        }

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public int Range
        {
            get { return range; }
            set { range = value; }
        }

        public bool Hidden
        {
            get { return hidden; }
            set { hidden = value; }
        }

        public bool Embarked
        {
            get { return embarked; }
            set { embarked = value; }
        }

        public List<Unit> Cargo
        {
            get { return cargo; }
        }

        public string toString()
        {
            string s = "";
            switch (type)
            {
                case UnitType.army:
                    s = "Army";
                    break;
                case UnitType.fighter:
                    s = "Fighter";
                    break;
                case UnitType.transport:
                    s = "Transport" + ", Cargo size: " + cargo.Count;
                    break;
                case UnitType.destroyer:
                    s = "Destroyer";
                    break;
                case UnitType.sub:
                    s = "Submarine";
                    break;
                case UnitType.cruiser:
                    s = "Cruiser";
                    break;
                case UnitType.carrier:
                    s = "Carrier";
                    break;
                case UnitType.battleship:
                    s = "Battleship";
                    break;
                default:
                    s = "Unidentified Unit";
                    break;
            }

            s += " at position: " + location.X + ", " + location.Y + "  Hits: " + hp + "  Moves: " + moves;

            if (type == UnitType.fighter)
            {
                s += " Range: " + range;
            }
            
            return s;
        }

        public virtual void enterCity()
        {
            moves = 1;
            range = 20;
        }

        public virtual bool loadUnit(Unit u)
        {
            return false;
        }

        public virtual void refreshMoves()
        {
            moves = 1;
        }
    }
}
