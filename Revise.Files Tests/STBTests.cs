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
    /// Provides testing for the <see cref="Revise.Files.STB"/> class.
    /// </summary>
    [TestFixture]
    public class STBTests {
        private const string TEST_FILE = "Files/LIST_QUEST.STB";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int ROW_COUNT = 5501;
            const int COLUMN_COUNT = 6;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            STB stb = new STB();
            stb.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(streamPosition, fileSize, "Not all of the file was read");
            Assert.AreEqual(stb.RowCount, ROW_COUNT, "Incorrect row count");
            Assert.AreEqual(stb.ColumnCount, COLUMN_COUNT, "Incorrect column count");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            STB stb = new STB();
            stb.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            stb.Save(savedStream);
            savedStream.Seek(0, SeekOrigin.Begin);

            STB savedSTB = new STB();
            savedSTB.Load(savedStream);
            savedStream.Close();

            Assert.AreEqual(stb.RowCount, savedSTB.RowCount, "Row counts do not match");
            Assert.AreEqual(stb.ColumnCount, savedSTB.ColumnCount, "Column counts do not match");

            for (int i = 0; i < stb.RowCount; i++) {
                for (int j = 0; j < stb.ColumnCount; j++) {
                    Assert.AreEqual(stb[i][j], savedSTB[i][j], "Cell values do not match");
                }
            }
        }

        /// <summary>
        /// Tests the column and row methods.
        /// </summary>
        [Test]
        public void TestColumnAndRowMethods() {
            const string COLUMN_HEADER = "Test Column";
            const int COLUMN_WIDTH = 101;
            const string CELL_VALUE = "Test Value";

            STB stb = new STB();
            stb.AddColumn(COLUMN_HEADER, COLUMN_WIDTH);
            stb.AddRow();

            stb[0][0] = CELL_VALUE;

            Assert.AreEqual(stb.GetColumnName(0), COLUMN_HEADER, "Incorrect column header");
            Assert.AreEqual(stb.GetColumnWidth(0), COLUMN_WIDTH, "Incorrect column width");
            Assert.AreEqual(stb[0][0], CELL_VALUE, "Incorrect cell value");

            stb.RemoveColumn(0);

            Assert.Throws(typeof(ColumnOutOfRangeException), () => {
                stb.GetColumnName(0);
            }, "Column not removed");

            Assert.Throws(typeof(CellOutOfRangeException), () => {
                stb[0][0] = CELL_VALUE;
            }, "Cell not removed");

            stb.RemoveRow(0);

            Assert.Throws(typeof(RowOutOfRangeException), () => {
                stb[0][0] = CELL_VALUE;
            }, "Row not removed");
        }
    }
}