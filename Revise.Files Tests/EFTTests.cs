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
 * MERCHANTABILITY or FITNESS FOR AD:\Code\Revise\Revise.Files Tests\EFTTests.cs PARTICULAR PURPOSE.  See the
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
    /// Provides testing for the <see cref="Revise.Files.EFT"/> class.
    /// </summary>
    [TestFixture]
    public class EFTTests {
        private const string TEST_FILE = "Files/_RUNASTON_01.EFT";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            EFT eft = new EFT();
            eft.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            EFT eft = new EFT();
            eft.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            eft.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            EFT savedEFT = new EFT();
            savedEFT.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(eft.Name, savedEFT.Name, "Name values do not match");
            Assert.AreEqual(eft.SoundEnabled, savedEFT.SoundEnabled, "Sound enable values do not match");
            Assert.AreEqual(eft.SoundFilePath, savedEFT.SoundFilePath, "Sound file path values do not match");
            Assert.AreEqual(eft.LoopCount, savedEFT.LoopCount, "Loop count values do not match");

            Assert.AreEqual(eft.Particles.Count, savedEFT.Particles.Count, "Particle count values do not match");

            for (int i = 0; i < eft.Particles.Count; i++) {
                Assert.AreEqual(eft.Particles[i].Name, savedEFT.Particles[i].Name, "Particle name values do not match");
                Assert.AreEqual(eft.Particles[i].UniqueIdentifier, savedEFT.Particles[i].UniqueIdentifier, "Particle unique identifier values do not match");
                Assert.AreEqual(eft.Particles[i].ParticleIndex, savedEFT.Particles[i].ParticleIndex, "Particle particle index values do not match");
                Assert.AreEqual(eft.Particles[i].FilePath, savedEFT.Particles[i].FilePath, "Particle file path values do not match");
                Assert.AreEqual(eft.Particles[i].AnimationEnabled, savedEFT.Particles[i].AnimationEnabled, "Particle animation enabled values do not match");
                Assert.AreEqual(eft.Particles[i].AnimationName, savedEFT.Particles[i].AnimationName, "Particle animation name values do not match");
                Assert.AreEqual(eft.Particles[i].AnimationLoopCount, savedEFT.Particles[i].AnimationLoopCount, "Particle animation loop count values do not match");
                Assert.AreEqual(eft.Particles[i].AnimationIndex, savedEFT.Particles[i].AnimationIndex, "Particle animation index values do not match");
                Assert.AreEqual(eft.Particles[i].Position, savedEFT.Particles[i].Position, "Particle position values do not match");
                Assert.AreEqual(eft.Particles[i].Rotation, savedEFT.Particles[i].Rotation, "Particle rotation values do not match");
                Assert.AreEqual(eft.Particles[i].Delay, savedEFT.Particles[i].Delay, "Particle delay values do not match");
                Assert.AreEqual(eft.Particles[i].LinkedToRoot, savedEFT.Particles[i].LinkedToRoot, "Particle link to root values do not match");
            }

            Assert.AreEqual(eft.Animations.Count, savedEFT.Animations.Count, "Animation count values do not match");

            for (int i = 0; i < eft.Animations.Count; i++) {
                Assert.AreEqual(eft.Animations[i].EffectName, savedEFT.Animations[i].EffectName, "Animation effect name values do not match");
                Assert.AreEqual(eft.Animations[i].MeshName, savedEFT.Animations[i].MeshName, "Animation mesh name values do not match");
                Assert.AreEqual(eft.Animations[i].MeshIndex, savedEFT.Animations[i].MeshIndex, "Animation mesh index values do not match");
                Assert.AreEqual(eft.Animations[i].MeshFilePath, savedEFT.Animations[i].MeshFilePath, "Animation mesh file path values do not match");
                Assert.AreEqual(eft.Animations[i].AnimationFilePath, savedEFT.Animations[i].AnimationFilePath, "Animation animation file path values do not match");
                Assert.AreEqual(eft.Animations[i].TextureFilePath, savedEFT.Animations[i].TextureFilePath, "Animation texture file path values do not match");
                Assert.AreEqual(eft.Animations[i].AlphaEnabled, savedEFT.Animations[i].AlphaEnabled, "Animation alpha enabled values do not match");
                Assert.AreEqual(eft.Animations[i].TwoSidedEnabled, savedEFT.Animations[i].TwoSidedEnabled, "Animation two sided enabled values do not match");
                Assert.AreEqual(eft.Animations[i].AlphaTestEnabled, savedEFT.Animations[i].AlphaTestEnabled, "Animation alpha test enabled values do not match");
                Assert.AreEqual(eft.Animations[i].DepthTestEnabled, savedEFT.Animations[i].DepthTestEnabled, "Animation depth test enabled values do not match");
                Assert.AreEqual(eft.Animations[i].DepthWriteEnabled, savedEFT.Animations[i].DepthWriteEnabled, "Animation depth write enabled values do not match");
                Assert.AreEqual(eft.Animations[i].SourceBlend, savedEFT.Animations[i].SourceBlend, "Animation source blend values do not match");
                Assert.AreEqual(eft.Animations[i].DestinationBlend, savedEFT.Animations[i].DestinationBlend, "Animation destination blend values do not match");
                Assert.AreEqual(eft.Animations[i].BlendOperation, savedEFT.Animations[i].BlendOperation, "Animation blend operation values do not match");
                Assert.AreEqual(eft.Animations[i].AnimationEnabled, savedEFT.Animations[i].AnimationEnabled, "Animation animation enabled values do not match");
                Assert.AreEqual(eft.Animations[i].AnimationName, savedEFT.Animations[i].AnimationName, "Animation animation name values do not match");
                Assert.AreEqual(eft.Animations[i].AnimationLoopCount, savedEFT.Animations[i].AnimationLoopCount, "Animation animation loop count values do not match");
                Assert.AreEqual(eft.Animations[i].AnimationIndex, savedEFT.Animations[i].AnimationIndex, "Animation animation index values do not match");
                Assert.AreEqual(eft.Animations[i].Position, savedEFT.Animations[i].Position, "Animation position values do not match");
                Assert.AreEqual(eft.Animations[i].Rotation, savedEFT.Animations[i].Rotation, "Animation rotation values do not match");
                Assert.AreEqual(eft.Animations[i].Delay, savedEFT.Animations[i].Delay, "Animation delay values do not match");
                Assert.AreEqual(eft.Animations[i].LoopCount, savedEFT.Animations[i].LoopCount, "Animation loop count values do not match");
                Assert.AreEqual(eft.Animations[i].LinkedToRoot, savedEFT.Animations[i].LinkedToRoot, "Animation link to root values do not match");
            }
        }   
    }
}