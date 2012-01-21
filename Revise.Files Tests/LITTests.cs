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
            Assert.AreEqual(lit.ObjectCount, OBJECT_COUNT, "Incorrect object count");
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

            Assert.AreEqual(lit.ObjectCount, savedLIT.ObjectCount, "Object counts do not match");

            for (int i = 0; i < lit.ObjectCount; i++) {
                Assert.AreEqual(lit[i].ID, savedLIT[i].ID, "Object IDs do not match");
                Assert.AreEqual(lit[i].PartCount, savedLIT[i].PartCount, "Part counts do not match");

                for (int j = 0; j < lit[i].PartCount; j++) {
                    Assert.AreEqual(lit[i][j].Name, lit[i][j].Name, "Part names do not match");
                    Assert.AreEqual(lit[i][j].ID, lit[i][j].ID, "Part IDs do not match");
                    Assert.AreEqual(lit[i][j].FileName, lit[i][j].FileName, "Part file names do not match");
                    Assert.AreEqual(lit[i][j].PixelsPerObject, lit[i][j].PixelsPerObject, "Part pixel per object values do not match");
                    Assert.AreEqual(lit[i][j].ObjectsPerWidth, lit[i][j].ObjectsPerWidth, "Part objects per width values do not match");
                    Assert.AreEqual(lit[i][j].ObjectPosition, lit[i][j].ObjectPosition, "Part position values do not match");
                }
            }

            Assert.AreEqual(lit.FileCount, savedLIT.FileCount, "File counts do not match");

            for (int i = 0; i < lit.FileCount; i++) {
                Assert.AreEqual(lit.GetFileName(i), savedLIT.GetFileName(i), "File names do not match");
            }
        }
    }
}