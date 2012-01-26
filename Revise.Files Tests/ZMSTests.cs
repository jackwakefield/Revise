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
    /// Provides testing for the <see cref="Revise.Files.ZMS"/> class.
    /// </summary>
    [TestFixture]
    public class ZMSTests {
        private const string TEST_FILE1 = "Files/CART01_ABILITY01.ZMS";
        private const string TEST_FILE2 = "Files/HEADBAD01.ZMS";
        private const string TEST_FILE3 = "Files/STONE014.ZMS";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        private void TestLoadMethod(string filePath) {
            Stream stream = File.OpenRead(filePath);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            ZMS zms = new ZMS();
            zms.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        private void TestSaveMethod(string filePath) {
            ZMS zms = new ZMS();
            zms.Load(filePath);

            MemoryStream savedStream = new MemoryStream();
            zms.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            ZMS savedZMS = new ZMS();
            savedZMS.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(zms.Pool, savedZMS.Pool, "Pool values do not match");
            Assert.AreEqual(zms.BoneTable.Count, savedZMS.BoneTable.Count, "Bone table counts do not match");

            for (int i = 0; i < zms.BoneTable.Count; i++) {
                Assert.AreEqual(zms.BoneTable[i], savedZMS.BoneTable[i], "Bone table values do not match");
            }

            Assert.AreEqual(zms.Vertices.Count, savedZMS.Vertices.Count, "Vertex counts do not match");

            for (int i = 0; i < zms.Vertices.Count; i++) {
                Assert.AreEqual(zms.Vertices[i].Position, savedZMS.Vertices[i].Position, "Vertex position values do not match");
                Assert.AreEqual(zms.Vertices[i].Normal, savedZMS.Vertices[i].Normal, "Vertex normal values do not match");
                Assert.AreEqual(zms.Vertices[i].Colour, savedZMS.Vertices[i].Colour, "Vertex colour values do not match");
                Assert.AreEqual(zms.Vertices[i].BoneWeights, savedZMS.Vertices[i].BoneWeights, "Vertex bone weight values do not match");
                Assert.AreEqual(zms.Vertices[i].BoneIndices, savedZMS.Vertices[i].BoneIndices, "Vertex bone index values do not match");
                Assert.AreEqual(zms.Vertices[i].TextureCoordinates[0], savedZMS.Vertices[i].TextureCoordinates[0], "Vertex texture coordinate values do not match");
                Assert.AreEqual(zms.Vertices[i].TextureCoordinates[1], savedZMS.Vertices[i].TextureCoordinates[1], "Vertex texture coordinate values do not match");
                Assert.AreEqual(zms.Vertices[i].TextureCoordinates[2], savedZMS.Vertices[i].TextureCoordinates[2], "Vertex texture coordinate values do not match");
                Assert.AreEqual(zms.Vertices[i].TextureCoordinates[3], savedZMS.Vertices[i].TextureCoordinates[3], "Vertex texture coordinate values do not match");
                Assert.AreEqual(zms.Vertices[i].Tangent, savedZMS.Vertices[i].Tangent, "Vertex tangent values do not match");
            }

            Assert.AreEqual(zms.Indices.Count, savedZMS.Indices.Count, "Index counts do not match");

            for (int i = 0; i < zms.Indices.Count; i++) {
                Assert.AreEqual(zms.Indices[i], savedZMS.Indices[i], "Index values do not match");
            }

            Assert.AreEqual(zms.Materials.Count, savedZMS.Materials.Count, "Material counts do not match");

            for (int i = 0; i < zms.Materials.Count; i++) {
                Assert.AreEqual(zms.Materials[i], savedZMS.Materials[i], "Material values do not match");
            }

            Assert.AreEqual(zms.Strips.Count, savedZMS.Strips.Count, "Strip counts do not match");

            for (int i = 0; i < zms.Strips.Count; i++) {
                Assert.AreEqual(zms.Strips[i], savedZMS.Strips[i], "Strip values do not match");
            }
        }

        [Test]
        public void TestLoadMethod1() {
            TestLoadMethod(TEST_FILE1);
        }

        [Test]
        public void TestLoadMethod2() {
            TestLoadMethod(TEST_FILE2);
        }

        [Test]
        public void TestLoadMethod3() {
            TestLoadMethod(TEST_FILE3);
        }

        [Test]
        public void TestSaveMethod1() {
            TestSaveMethod(TEST_FILE1);
        }

        [Test]
        public void TestSaveMethod2() {
            TestSaveMethod(TEST_FILE2);
        }

        [Test]
        public void TestSaveMethod3() {
            TestSaveMethod(TEST_FILE3);
        }
    }
}