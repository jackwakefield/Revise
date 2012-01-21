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
    /// Provides testing for the <see cref="Revise.Files.TSI"/> class.
    /// </summary>
    [TestFixture]
    public class TSITests {
        private const string TEST_FILE = "Files/UI.TSI";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int TEXTURE_COUNT = 38;
            const int SPRITE_COUNT = 648;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            TSI tsi = new TSI();
            tsi.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(TEXTURE_COUNT, tsi.Textures.Count, "Incorrect texture count");
            Assert.AreEqual(SPRITE_COUNT, tsi.Sprites.Count, "Incorrect sprite count");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            TSI tsi = new TSI();
            tsi.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            tsi.Save(savedStream);
            tsi.Save(@"C:\UI.TSI");

            savedStream.Seek(0, SeekOrigin.Begin);

            TSI savedTSI = new TSI();
            savedTSI.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(tsi.Textures.Count, savedTSI.Textures.Count, "Texture counts do not match");
            Assert.AreEqual(tsi.Sprites.Count, savedTSI.Sprites.Count, "Sprite counts do not match");

            for (int i = 0; i < tsi.Textures.Count; i++) {
                Assert.AreEqual(tsi.Textures[i].FileName, savedTSI.Textures[i].FileName, "Texture file names values do not match");
                Assert.AreEqual(tsi.Textures[i].ColourKey, savedTSI.Textures[i].ColourKey, "Texture colour key values do not match");
            }

            for (int i = 0; i < tsi.Sprites.Count; i++) {
                Assert.AreEqual(tsi.Sprites[i].Texture, savedTSI.Sprites[i].Texture, "Sprite texture values do not match");
                Assert.AreEqual(tsi.Sprites[i].X1, savedTSI.Sprites[i].X1, "Sprite X1 values do not match");
                Assert.AreEqual(tsi.Sprites[i].Y1, savedTSI.Sprites[i].Y1, "Sprite Y1 values do not match");
                Assert.AreEqual(tsi.Sprites[i].X2, savedTSI.Sprites[i].X2, "Sprite X2 values do not match");
                Assert.AreEqual(tsi.Sprites[i].Y2, savedTSI.Sprites[i].Y2, "Sprite Y2 values do not match");
                Assert.AreEqual(tsi.Sprites[i].ID, savedTSI.Sprites[i].ID, "Sprite ID values do not match");
            }
        }
    }
}