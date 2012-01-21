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

using System;
using System.Collections.Generic;

namespace Revise.Files {
    /// <summary>
    /// Represents an LIT object.
    /// </summary>
    public class LightObject {
        #region Properties

        /// <summary>
        /// Gets or sets the ID of the object.
        /// </summary>
        /// <value>
        /// The ID of the object.
        /// </value>
        public int ID {
            get;
            set;
        }

        /// <summary>
        /// Gets the numbers of parts.
        /// </summary>
        public int PartCount {
            get {
                return parts.Count;
            }
        }

        #endregion

        private List<LightPart> parts;

        /// <summary>
        /// Initializes a new instance of the <see cref="Revise.Files.LightObject"/> class.
        /// </summary>
        public LightObject() {
            parts = new List<LightPart>();
        }

        /// <summary>
        /// Gets the specified <see cref="Revise.Files.LightPart"/>.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified page does not exist.</exception>
        public LightPart this[int part] {
            get {
                if (part < 0 || part > parts.Count - 1) {
                    throw new ArgumentOutOfRangeException("part", "Part is out of range");
                }

                return parts[part];
            }
        }

        /// <summary>
        /// Adds a new light part.
        /// </summary>
        /// <returns>The created light part.</returns>
        public LightPart AddPart() {
            return AddPart(new LightPart());
        }

        /// <summary>
        /// Adds the specified light part.
        /// </summary>
        /// <param name="part">The light part.</param>
        /// <returns></returns>
        public LightPart AddPart(LightPart part) {
            parts.Add(part);

            return part;
        }

        /// <summary>
        /// Removes the specified light part.
        /// </summary>
        /// <param name="part">The light part to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified light part does not exist.</exception>
        public void RemovePart(int part) {
            if (part < 0 || part > parts.Count - 1) {
                throw new ArgumentOutOfRangeException("part", "Part is out of range");
            }

            parts.RemoveAt(part);
        }

        /// <summary>
        /// Removes the specified light part.
        /// </summary>
        /// <param name="part">The light part to remove.</param>
        /// <exception cref="System.ArgumentException">Thrown when the object does not contain the specified light part.</exception>
        public void RemovePart(LightPart part) {
            if (parts.Contains(part)) {
                throw new ArgumentException("part", "Object does not contain part");
            }

            parts.Remove(part);
        }
    }
}