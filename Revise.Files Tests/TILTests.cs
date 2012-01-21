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
    /// Provides testing for the <see cref="Revise.Files.TIL"/> class.
    /// </summary>
    [TestFixture]
    public class TILTests {
        private const string TEST_FILE = "Files/31_31.TIL";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int HEIGHT = 16;
            const int WIDTH = 16;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            TIL til = new TIL();
            til.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(streamPosition, fileSize, "Not all of the file was read");
            Assert.AreEqual(til.Width, WIDTH, "Incorrect width");
            Assert.AreEqual(til.Height, HEIGHT, "Incorrect height");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            TIL til = new TIL();
            til.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            til.Save(savedStream);
            til.Load(TEST_FILE);

            savedStream.Seek(0, SeekOrigin.Begin);

            TIL savedTIL = new TIL();
            savedTIL.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(til.Width, savedTIL.Width, "Width values do not match");
            Assert.AreEqual(til.Height, savedTIL.Height, "Height values do not match");

            for (int x = 0; x < til.Height; x++) {
                for (int y = 0; y < til.Width; y++) {
                    Assert.AreEqual(til[x, y].Brush, savedTIL[x, y].Brush, "Brush values do not match");
                    Assert.AreEqual(til[x, y].TileIndex, savedTIL[x, y].TileIndex, "Tile index values do not match");
                    Assert.AreEqual(til[x, y].TileSet, savedTIL[x, y].TileSet, "Tile set values do not match");
                    Assert.AreEqual(til[x, y].Tile, savedTIL[x, y].Tile, "Tile values do not match");
                }
            }
        }
    }
}