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
using Revise.Files.Exceptions;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="Revise.Files.LTB"/> class.
    /// </summary>
    [TestFixture]
    public class LTBTests {
        private const string TEST_FILE = "Files/ULNGTB_CON.LTB";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int ROW_COUNT = 200;
            const int COLUMN_COUNT = 6;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            LTB ltb = new LTB();
            ltb.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(streamPosition, fileSize, "Not all of the file was read");
            Assert.AreEqual(ltb.RowCount, ROW_COUNT, "Incorrect row count");
            Assert.AreEqual(ltb.ColumnCount, COLUMN_COUNT, "Incorrect column count");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            LTB ltb = new LTB();
            ltb.Load(TEST_FILE);
            
            MemoryStream savedStream = new MemoryStream();
            ltb.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            LTB savedLTB = new LTB();
            savedLTB.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(ltb.RowCount, savedLTB.RowCount, "Row counts do not match");
            Assert.AreEqual(ltb.ColumnCount, savedLTB.ColumnCount, "Column counts do not match");

            for (int i = 0; i < ltb.RowCount; i++) {
                for (int j = 0; j < ltb.ColumnCount; j++) {
                    Assert.AreEqual(ltb[i][j], savedLTB[i][j], "Cell values do not match");
                }
            }
        }

        /// <summary>
        /// Tests the row and column methods.
        /// </summary>
        [Test]
        public void TestRowAndColumnMethods() {
            const string CELL_VALUE = "Test Value";

            LTB ltb = new LTB();
            ltb.AddRow();
            ltb.AddColumn();
            ltb[0][0] = CELL_VALUE;

            Assert.AreEqual(ltb.ColumnCount, 1, "Column count is incorrect");
            Assert.AreEqual(ltb.RowCount, 1, "Row count is incorrect");
            Assert.AreEqual(ltb[0][0], CELL_VALUE, "Row value is incorrect");

            ltb.RemoveColumn(0);

            Assert.Throws(typeof(CellOutOfRangeException), () => {
                ltb[0][0] = CELL_VALUE;
            }, "Column not removed");

            ltb.RemoveRow(0);

            Assert.Throws(typeof(RowOutOfRangeException), () => {
                ltb[0][0] = CELL_VALUE;
            }, "Row not removed");
        }
    }
}