﻿#region License

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

using System.IO;
using System.Numerics;
using Revise.Files.IFO.Interfaces;

namespace Revise.Files.IFO.Blocks {
    /// <summary>
    /// Represents a map water plane.
    /// </summary>
    public class MapWaterPlane : IMapBlock {
        #region Properties

        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        public Vector3 StartPosition {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end position.
        /// </summary>
        public Vector3 EndPosition {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Reads the block data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(BinaryReader reader) {
            StartPosition = new Vector3() {
                X = reader.ReadSingle(),
                Z = reader.ReadSingle(),
                Y = reader.ReadSingle(),
            };

            EndPosition = new Vector3() {
                X = reader.ReadSingle(),
                Z = reader.ReadSingle(),
                Y = reader.ReadSingle(),
            };
        }

        /// <summary>
        /// Writes the block data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer) {
            writer.Write(StartPosition.X);
            writer.Write(StartPosition.Z);
            writer.Write(StartPosition.Y);

            writer.Write(EndPosition.X);
            writer.Write(EndPosition.Z);
            writer.Write(EndPosition.Y);
        }
    }
}