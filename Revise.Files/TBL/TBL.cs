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
using System.IO;
using System.Text;

namespace Revise.Files {
    /// <summary>
    /// Provides the ability to create, open and save TBL files.
    /// </summary>
    public class TBL : FileLoader {
        #region Properties

        /// <summary>
        /// Gets the maximum range.
        /// </summary>
        public short MaximumRange {
            get {
                return maximumRange;
            }
            set {
                maximumRange = value;

                Array.Resize<short>(ref startIndexes, value);
                Array.Resize<short>(ref indexCounts, value);
            }
        }

        /// <summary>
        /// Gets the start indexes.
        /// </summary>
        public short[] StartIndexes {
            get {
                return startIndexes;
            }
        }

        /// <summary>
        /// Gets the index counts.
        /// </summary>
        public short[] IndexCounts {
            get {
                return indexCounts;
            }
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        public List<TablePoint> Points {
            get {
                return points;
            }
        }

        #endregion

        private short maximumRange;
        private short[] startIndexes;
        private short[] indexCounts;

        private List<TablePoint> points;

        /// <summary>
        /// Initializes a new instance of the <see cref="Revise.Files.TBL"/> class.
        /// </summary>
        public TBL() {
            points = new List<TablePoint>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            MaximumRange = reader.ReadInt16();

            for (int i = 0; i < maximumRange; i++) {
                startIndexes[i] = reader.ReadInt16();
                indexCounts[i] = reader.ReadInt16();
            }

            int maximumArray = reader.ReadInt16();

            for (int i = 0; i < maximumArray; i++) {
                TablePoint point = new TablePoint();
                point.X = reader.ReadInt16();
                point.Y = reader.ReadInt16();

                points.Add(point);
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(maximumRange);

            for (int i = 0; i < maximumRange; i++) {
                writer.Write(startIndexes[i]);
                writer.Write(indexCounts[i]);
            }

            writer.Write((short)points.Count);

            points.ForEach(point => {
                writer.Write(point.X);
                writer.Write(point.Y);
            });
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            maximumRange = 0;
            startIndexes = new short[0];
            indexCounts = new short[0];

            points.Clear();
        }
    }
}