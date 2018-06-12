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
    public class GameVariables
    {
        #region Map Constants
        public const int TILE_SIZE = 16;
        public const int MAP_WIDTH = 100;
        public const int MAP_HEIGHT = 60;
        public const int GAME_WIDTH = TILE_SIZE * MAP_WIDTH;
        public const int GAME_HEIGHT = TILE_SIZE * MAP_HEIGHT;
        public const int PANEL_BUFFER = 64;
        #endregion

        #region Production Times
        public const int ARMY_BUILD_TIME = 5;
        public const int FIGHTER_BUILD_TIME = 10;
        public const int TRANSPORT_BUILD_TIME = 30;
        public const int DESTROYER_BUILD_TIME = 30;
        public const int SUB_BUILD_TIME = 20;
        public const int CRUISER_BUILD_TIME = 60;
        public const int CARRIER_BUILD_TIME = 80;
        public const int BATTLESHIP_BUILD_TIME = 100;
        public const double INITIAL_PRODUCTION_MODIFIER = 1.2;
        #endregion

        #region Unit Statistics
        public const int ARMY_MOVES = 1;
        public const int ARMY_HEALTH = 1;
        public const int ARMY_DAMAGE = 1;
        public const int FIGHTER_MOVES = 4;
        public const int FIGHTER_HEALTH = 1;
        public const int FIGHTER_DAMAGE = 1;
        public const int FIGHTER_RANGE = 20;
        public const int TRANSPORT_MOVES = 2;
        public const int TRANSPORT_HEALTH = 3;
        public const int TRANSPORT_DAMAGE = 1;
        public const int TRANSPORT_CAPACITY_MODIFIER = 2;
        public const int TRANSPORT_CAPACITY = TRANSPORT_CAPACITY_MODIFIER * TRANSPORT_HEALTH;
        public const int DESTROYER_MOVES = 2;
        public const int DESTROYER_HEALTH = 3;
        public const int DESTROYER_DAMAGE = 1;
        public const int SUB_MOVES = 2;
        public const int SUB_HEALTH = 2;
        public const int SUB_DAMAGE = 2;
        public const int CRUISER_MOVES = 2;
        public const int CRUISER_HEALTH = 6;
        public const int CRUISER_DAMAGE = 1;
        public const int CARRIER_MOVES = 2;
        public const int CARRIER_HEALTH = 8;
        public const int CARRIER_DAMAGE = 1;
        public const int CARRIER_CAPACITY_MODIFIER = 1;
        public const int CARRIER_CAPACITY = CARRIER_CAPACITY_MODIFIER * CARRIER_HEALTH;
        public const int BATTLESHIP_MOVES = 2;
        public const int BATTLESHIP_HEALTH = 10;
        public const int BATTLESHIP_DAMAGE = 2;
        #endregion

        #region Player Colors
        public static Color p1color = Color.Yellow;
        public static Color p2color = Color.Red;
        public static Color p3color = Color.Cyan;
        public static Color p4color = Color.Magenta;
        public static Color p5color = Color.White;
        public static Color p6color = Color.Orange;
        #endregion

        #region Panel Locations
        // Buffer and spacing constants
        public const int PANEL_VERT_BUFFER = 2;
        public const int PANEL_HORIZ_BUFFER = 16;
        public const int PANEL_COLUMN_SIZE = 16;  // font size is 12, adding 2px vert buffer above and below
        public const int PANEL_ROW_SIZE = 400;

        // Column 1
        public static Vector2 LOCATION_INFO_LOCATION = new Vector2(PANEL_HORIZ_BUFFER, GAME_HEIGHT + PANEL_VERT_BUFFER);
        public static Vector2 LOCATION_UNIT_INFO_LOCATION = new Vector2(PANEL_HORIZ_BUFFER, GAME_HEIGHT + PANEL_COLUMN_SIZE + PANEL_VERT_BUFFER);
        public static Vector2 UNIT_INFORMATION = new Vector2(PANEL_HORIZ_BUFFER, GAME_HEIGHT + 2 * PANEL_COLUMN_SIZE + PANEL_VERT_BUFFER);

        // Column 2
        public static Vector2 TURN_INFORMATION = new Vector2(PANEL_HORIZ_BUFFER + PANEL_ROW_SIZE, GAME_HEIGHT + PANEL_VERT_BUFFER);
        public static Vector2 COMBAT_INFORMATION = new Vector2(PANEL_HORIZ_BUFFER + PANEL_ROW_SIZE, GAME_HEIGHT + PANEL_VERT_BUFFER + PANEL_COLUMN_SIZE);
        public static Vector2 DEBUG_INFORMATION = new Vector2(PANEL_HORIZ_BUFFER + PANEL_ROW_SIZE, GAME_HEIGHT + PANEL_VERT_BUFFER + 2 * PANEL_COLUMN_SIZE);

        // Column 3
        public static Vector2 CONFIRM_DIALOG = new Vector2(PANEL_HORIZ_BUFFER + 2 * PANEL_ROW_SIZE, GAME_HEIGHT + PANEL_VERT_BUFFER);
        public static Vector2 COMBAT_INFO_2 = new Vector2(PANEL_HORIZ_BUFFER + 2 * PANEL_ROW_SIZE, GAME_HEIGHT + PANEL_VERT_BUFFER + 2 * PANEL_COLUMN_SIZE);
        #endregion


    }
}
