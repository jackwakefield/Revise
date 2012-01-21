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
    /// Provides testing for the <see cref="Revise.Files.LOD"/> class.
    /// </summary>
    [TestFixture]
    public class LODTests {
        private const string TEST_FILE = "Files/PATCHTYPE.LOD";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const string NAME = "block_height";

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            LOD lod = new LOD();
            lod.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(NAME, lod.Name, "Incorrect name");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            LOD lod = new LOD();
            lod.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            lod.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            LOD savedLOD = new LOD();
            savedLOD.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(lod.Name, savedLOD.Name, "Names do not match");

            for (int x = 0; x < lod.Height; x++) {
                for (int y = 0; y < lod.Width; y++) {
                    Assert.AreEqual(lod[x, y], savedLOD[x, y], "Values do not match");
                }
            }
        }
    }
}