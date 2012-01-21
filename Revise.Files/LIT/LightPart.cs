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
    /// Represents an LIT part.
    /// </summary>
    public class LightPart {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the part.
        /// </summary>
        /// <value>
        /// The name of the part.
        /// </value>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ID of the part.
        /// </summary>
        /// <value>
        /// The ID of the part.
        /// </value>
        public int ID {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the lightmap graphics file.
        /// </summary>
        /// <value>
        /// The name of the lightmap graphics file.
        /// </value>
        public string FileName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the lightmap index value.
        /// </summary>
        /// <value>
        /// The lightmap index value.
        /// </value>
        public int LightmapIndex {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pixels per object value.
        /// </summary>
        /// <value>
        /// The pixels per object value.
        /// </value>
        public int PixelsPerObject {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the objects per width value.
        /// </summary>
        /// <value>
        /// The width of the objects per width value.
        /// </value>
        public int ObjectsPerWidth {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object position.
        /// </summary>
        /// <value>
        /// The object position.
        /// </value>
        public int ObjectPosition {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Revise.Files.LightPart"/> class.
        /// </summary>
        public LightPart() {
            Name = string.Empty;
            FileName = string.Empty;
        }
    }
}