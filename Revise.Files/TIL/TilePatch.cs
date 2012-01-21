#region License

/**
 * Copyright (C) 2012 Jack Wakefield
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#endregion

namespace Revise.Files {
    /// <summary>
    /// Represents a tile.
    /// </summary>
    public struct TilePatch {
        #region Properties

        /// <summary>
        /// Gets or sets the brush value.
        /// </summary>
        /// <value>
        /// The brush value.
        /// </value>
        public byte Brush {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the index of the tile.
        /// </summary>
        /// <value>
        /// The index of the tile.
        /// </value>
        public byte TileIndex {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tile set value.
        /// </summary>
        /// <value>
        /// The tile set value.
        /// </value>
        public byte TileSet {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tile value.
        /// </summary>
        /// <value>
        /// The tile value.
        /// </value>
        public int Tile {
            get;
            set;
        }

        #endregion
    }
}