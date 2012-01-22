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
    /// Provides testing for the <see cref="Revise.Files.ZCA"/> class.
    /// </summary>
    [TestFixture]
    public class ZCATests {
        private const string TEST_FILE = "Files/CAMERA.ZCA";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            ZCA zca = new ZCA();
            zca.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            ZCA zca = new ZCA();
            zca.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            zca.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            ZCA savedZCA = new ZCA();
            savedZCA.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(zca.ProjectionType, savedZCA.ProjectionType, "Projection types do not match");
            Assert.AreEqual(zca.ModelView, savedZCA.ModelView, "Model view matrices do not match");
            Assert.AreEqual(zca.Projection, savedZCA.Projection, "Projection matrices do not match");
            Assert.AreEqual(zca.FieldOfView, savedZCA.FieldOfView, "Field of view values do not match");
            Assert.AreEqual(zca.AspectRatio, savedZCA.AspectRatio, "Aspect ratio values do not match");
            Assert.AreEqual(zca.NearPlane, savedZCA.NearPlane, "Near plane values do not match");
            Assert.AreEqual(zca.FarPlane, savedZCA.FarPlane, "Far plane values do not match");
            Assert.AreEqual(zca.Eye, savedZCA.Eye, "Eye positions do not match");
            Assert.AreEqual(zca.Center, savedZCA.Center, "Center position do not match");
            Assert.AreEqual(zca.Up, savedZCA.Up, "Up positions do not match");
        }
    }
}