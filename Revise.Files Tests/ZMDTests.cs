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
    /// Provides testing for the <see cref="Revise.Files.ZMD"/> class.
    /// </summary>
    [TestFixture]
    public class ZMDTests {
        private const string TEST_FILE = "Files/CART01.ZMD";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            ZMD zmd = new ZMD();
            zmd.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            ZMD zmd = new ZMD();
            zmd.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            zmd.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            ZMD savedZMD = new ZMD();
            savedZMD.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(zmd.Bones.Count, savedZMD.Bones.Count, "Bone counts do not match");
            Assert.AreEqual(zmd.DummyBones.Count, savedZMD.DummyBones.Count, "Dummy bones counts do not match");

            for (int i = 0; i < zmd.Bones.Count; i++) {
                Assert.AreEqual(zmd.Bones[i].Name, savedZMD.Bones[i].Name, "Bone names do not match");
                Assert.AreEqual(zmd.Bones[i].Parent, savedZMD.Bones[i].Parent, "Bone parents do not match");
                Assert.AreEqual(zmd.Bones[i].Translation, savedZMD.Bones[i].Translation, "Bone positions do not match");
                Assert.AreEqual(zmd.Bones[i].Rotation, savedZMD.Bones[i].Rotation, "Bone rotations do not match");
            }

            for (int i = 0; i < zmd.DummyBones.Count; i++) {
                Assert.AreEqual(zmd.DummyBones[i].Name, savedZMD.DummyBones[i].Name, "Dummy bone names do not match");
                Assert.AreEqual(zmd.DummyBones[i].Parent, savedZMD.DummyBones[i].Parent, "Dummy bone parents do not match");
                Assert.AreEqual(zmd.DummyBones[i].Translation, savedZMD.DummyBones[i].Translation, "Dummy bone positions do not match");
                Assert.AreEqual(zmd.DummyBones[i].Rotation, savedZMD.DummyBones[i].Rotation, "Dummy bone rotations do not match");
            }
        }
    }
}