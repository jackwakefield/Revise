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
    /// Provides testing for the <see cref="Revise.Files.LIT"/> class.
    /// </summary>
    [TestFixture]
    public class LITTests {
        private const string TEST_FILE = "Files/OBJECTLIGHTMAPDATA.LIT";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int OBJECT_COUNT = 266;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            LIT lit = new LIT();
            lit.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(streamPosition, fileSize, "Not all of the file was read");
            Assert.AreEqual(lit.Objects.Count, OBJECT_COUNT, "Incorrect object count");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            LIT lit = new LIT();
            lit.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            lit.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            LIT savedLIT = new LIT();
            savedLIT.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(lit.Objects.Count, savedLIT.Objects.Count, "Object counts do not match");

            for (int i = 0; i < lit.Objects.Count; i++) {
                Assert.AreEqual(lit.Objects[i].ID, savedLIT.Objects[i].ID, "Object IDs do not match");
                Assert.AreEqual(lit.Objects[i].Parts.Count, savedLIT.Objects[i].Parts.Count, "Part counts do not match");

                for (int j = 0; j < lit.Objects[i].Parts.Count; j++) {
                    Assert.AreEqual(lit.Objects[i].Parts[j].Name, lit.Objects[i].Parts[j].Name, "Part names do not match");
                    Assert.AreEqual(lit.Objects[i].Parts[j].ID, lit.Objects[i].Parts[j].ID, "Part IDs do not match");
                    Assert.AreEqual(lit.Objects[i].Parts[j].FileName, lit.Objects[i].Parts[j].FileName, "Part file names do not match");
                    Assert.AreEqual(lit.Objects[i].Parts[j].PixelsPerObject, lit.Objects[i].Parts[j].PixelsPerObject, "Part pixel per object values do not match");
                    Assert.AreEqual(lit.Objects[i].Parts[j].ObjectsPerWidth, lit.Objects[i].Parts[j].ObjectsPerWidth, "Part objects per width values do not match");
                    Assert.AreEqual(lit.Objects[i].Parts[j].ObjectPosition, lit.Objects[i].Parts[j].ObjectPosition, "Part position values do not match");
                }
            }

            Assert.AreEqual(lit.Files.Count, savedLIT.Files.Count, "File counts do not match");

            for (int i = 0; i < lit.Files.Count; i++) {
                Assert.AreEqual(lit.Files[i], savedLIT.Files[i], "File names do not match");
            }
        }
    }
}