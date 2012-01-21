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
    /// Provides testing for the <see cref="Revise.Files.HIM"/> class.
    /// </summary>
    [TestFixture]
    public class HIMTests {
        private const string TEST_FILE = "Files/31_30.HIM";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int HEIGHT = 65;
            const int WIDTH = 65;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            HIM him = new HIM();
            him.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(WIDTH, him.Width, "Incorrect width");
            Assert.AreEqual(HEIGHT, him.Height, "Incorrect height");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            HIM him = new HIM();
            him.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            him.Save(savedStream);
            him.Load(TEST_FILE);

            savedStream.Seek(0, SeekOrigin.Begin);

            HIM savedHIM = new HIM();
            savedHIM.Load(savedStream);

            savedStream.Close();

            for (int x = 0; x < him.Height; x++) {
                for (int y = 0; y < him.Width; y++) {
                    Assert.AreEqual(him[x, y], savedHIM[x, y], "Height values do not match");
                }
            }

            for (int x = 0; x < him.Patches.GetLength(0); x++) {
                for (int y = 0; y < him.Patches.GetLength(1); y++) {
                    Assert.AreEqual(him.Patches[x, y].Minimum, savedHIM.Patches[x, y].Minimum, "Minimum patch values do not match");
                    Assert.AreEqual(him.Patches[x, y].Maximum, savedHIM.Patches[x, y].Maximum, "Maximum patch values do not match");
                }
            }

            for (int i = 0; i < him.QuadPatches.Length; i++) {
                Assert.AreEqual(him.QuadPatches[i].Minimum, savedHIM.QuadPatches[i].Minimum, "Minimum quad patch values do not match");
                Assert.AreEqual(him.QuadPatches[i].Maximum, savedHIM.QuadPatches[i].Maximum, "Maximum quad patch values do not match");
            }
        }
    }
}