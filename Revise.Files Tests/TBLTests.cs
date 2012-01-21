#region License

/**
 * Copyright (C) 2011 Jack Wakefield
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
using NUnit.Framework;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="Revise.Files.TBL"/> class.
    /// </summary>
    [TestFixture]
    public class TBLTests {
        private const string TEST_FILE = "Files/O_RANGE.TBL";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int MAXIMUM_RANGE = 42;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            TBL tbl = new TBL();
            tbl.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(streamPosition, fileSize, "Not all of the file was read");
            Assert.AreEqual(tbl.MaximumRange, MAXIMUM_RANGE, "Incorrect maximum range");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            TBL tbl = new TBL();
            tbl.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            tbl.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            TBL savedTBL = new TBL();
            savedTBL.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(tbl.MaximumRange, savedTBL.MaximumRange, "Maximum range does not match");

            for (int i = 0; i < tbl.MaximumRange; i++) {
                Assert.AreEqual(tbl.StartIndexes[i], tbl.StartIndexes[i], "Start index does not match");
                Assert.AreEqual(tbl.IndexCounts[i], tbl.IndexCounts[i], "Index count does not match");
            }

            Assert.AreEqual(tbl.Points.Count, savedTBL.Points.Count, "Points count does not match");

            for (int i = 0; i < tbl.Points.Count; i++) {
                Assert.AreEqual(tbl.Points[i].X, tbl.Points[i].X, "Points do not match");
                Assert.AreEqual(tbl.Points[i].Y, tbl.Points[i].Y, "Points do not match");
            }
        }
    }
}