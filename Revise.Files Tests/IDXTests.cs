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
    /// Provides testing for the <see cref="Revise.Files.IDX"/> class.
    /// </summary>
    [TestFixture]
    public class IDXTests {
        private const string TEST_FILE = "Files/data.idx";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int FILE_SYSTEM_COUNT = 2;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            IDX idx = new IDX();
            idx.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(FILE_SYSTEM_COUNT, idx.FileSystems.Count, "Incorrect file system count");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            IDX idx = new IDX();
            idx.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            idx.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            IDX savedIDX = new IDX();
            savedIDX.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(idx.BaseVersion, savedIDX.BaseVersion, "Base version values do not match");
            Assert.AreEqual(idx.CurrentVersion, savedIDX.CurrentVersion, "Current version values do not match");
            Assert.AreEqual(idx.FileSystems.Count, savedIDX.FileSystems.Count, "File system counts do not match");

            for (int i = 0; i < idx.FileSystems.Count; i++) {
                Assert.AreEqual(idx.FileSystems[i].FileName, savedIDX.FileSystems[i].FileName, "File names do not match");
                Assert.AreEqual(idx.FileSystems[i].Files.Count, savedIDX.FileSystems[i].Files.Count, "File counts do not match");

                for (int j = 0; j < idx.FileSystems[i].Files.Count; j++) {
                    Assert.AreEqual(idx.FileSystems[i].Files[j].FilePath, idx.FileSystems[i].Files[j].FilePath, "File paths do not match");
                    Assert.AreEqual(idx.FileSystems[i].Files[j].Offset, idx.FileSystems[i].Files[j].Offset, "Offset values do not match");
                    Assert.AreEqual(idx.FileSystems[i].Files[j].Size, idx.FileSystems[i].Files[j].Size, "Size values do not match");
                    Assert.AreEqual(idx.FileSystems[i].Files[j].BlockSize, idx.FileSystems[i].Files[j].BlockSize, "Block size values do not match");
                    Assert.AreEqual(idx.FileSystems[i].Files[j].IsDeleted, idx.FileSystems[i].Files[j].IsDeleted, "Deleted values do not match");
                    Assert.AreEqual(idx.FileSystems[i].Files[j].IsCompressed, idx.FileSystems[i].Files[j].IsCompressed, "Compresed values do not match");
                    Assert.AreEqual(idx.FileSystems[i].Files[j].IsEncrypted, idx.FileSystems[i].Files[j].IsEncrypted, "Encrypted value sdo not match");
                    Assert.AreEqual(idx.FileSystems[i].Files[j].Version, idx.FileSystems[i].Files[j].Version, "Version values do not match");
                    Assert.AreEqual(idx.FileSystems[i].Files[j].Checksum, idx.FileSystems[i].Files[j].Checksum, "Checksum values do not match");
                }
            }
        }
    }
}