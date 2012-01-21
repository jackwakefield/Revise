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
    /// Provides testing for the <see cref="Revise.Files.MOV"/> class.
    /// </summary>
    [TestFixture]
    public class MOVTests {
        private const string TEST_FILE = "Files/31_31.MOV";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int HEIGHT = 32;
            const int WIDTH = 32;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            MOV mov = new MOV();
            mov.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(streamPosition, fileSize, "Not all of the file was read");
            Assert.AreEqual(mov.Width, WIDTH, "Incorrect width");
            Assert.AreEqual(mov.Height, HEIGHT, "Incorrect height");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            MOV mov = new MOV();
            mov.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            mov.Save(savedStream);
            mov.Load(TEST_FILE);

            savedStream.Seek(0, SeekOrigin.Begin);

            MOV savedMOV = new MOV();
            savedMOV.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(mov.Width, savedMOV.Width, "Width values do not match");
            Assert.AreEqual(mov.Height, savedMOV.Height, "Height values do not match");

            for (int x = 0; x < mov.Height; x++) {
                for (int y = 0; y < mov.Width; y++) {
                    Assert.AreEqual(mov[x, y], savedMOV[x, y], "Values do not match");
                }
            }
        }
    }
}