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
    /// Provides testing for the <see cref="Revise.Files.CHR"/> class.
    /// </summary>
    [TestFixture]
    public class CHRTests {
        private const string TEST_FILE = "Files/LIST_NPC.CHR";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            CHR chr = new CHR();
            chr.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            CHR chr = new CHR();
            chr.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            chr.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            CHR savedCHR = new CHR();
            savedCHR.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(chr.SkeletonFiles.Count, savedCHR.SkeletonFiles.Count, "Skeleton file counts do not match");

            for (int i = 0; i < chr.SkeletonFiles.Count; i++) {
                Assert.AreEqual(chr.SkeletonFiles[i], savedCHR.SkeletonFiles[i], "Skeleton file names do not match");
            }

            Assert.AreEqual(chr.MotionFiles.Count, savedCHR.MotionFiles.Count, "Motion file counts do not match");

            for (int i = 0; i < chr.MotionFiles.Count; i++) {
                Assert.AreEqual(chr.MotionFiles[i], savedCHR.MotionFiles[i], "Motion file names do not match");
            }

            Assert.AreEqual(chr.EffectFiles.Count, savedCHR.EffectFiles.Count, "Effect file counts do not match");

            for (int i = 0; i < chr.EffectFiles.Count; i++) {
                Assert.AreEqual(chr.EffectFiles[i], savedCHR.EffectFiles[i], "Effect file names do not match");
            }

            Assert.AreEqual(chr.Characters.Count, savedCHR.Characters.Count, "Character counts do not match");

            for (int i = 0; i < chr.Characters.Count; i++) {
                Assert.AreEqual(chr.Characters[i].IsEnabled, savedCHR.Characters[i].IsEnabled, "Character enabled values do not match");

                if (chr.Characters[i].IsEnabled) {
                    Assert.AreEqual(chr.Characters[i].ID, savedCHR.Characters[i].ID, "Character ID values do not match");
                    Assert.AreEqual(chr.Characters[i].Name, savedCHR.Characters[i].Name, "Character name values do not match");

                    Assert.AreEqual(chr.Characters[i].Objects.Count, savedCHR.Characters[i].Objects.Count, "Character object counts do not match");

                    for (int j = 0; j < chr.Characters[i].Objects.Count; j++) {
                        Assert.AreEqual(chr.Characters[i].Objects[j].Object, savedCHR.Characters[i].Objects[j].Object, "Character object values do not match");
                    }

                    Assert.AreEqual(chr.Characters[i].Animations.Count, savedCHR.Characters[i].Animations.Count, "Character animation counts do not match");

                    for (int j = 0; j < chr.Characters[i].Animations.Count; j++) {
                        Assert.AreEqual(chr.Characters[i].Animations[j].Type, savedCHR.Characters[i].Animations[j].Type, "Character animation type values do not match");
                        Assert.AreEqual(chr.Characters[i].Animations[j].Animation, savedCHR.Characters[i].Animations[j].Animation, "Character animation values do not match");
                    }

                    Assert.AreEqual(chr.Characters[i].Effects.Count, savedCHR.Characters[i].Effects.Count, "Character effect counts do not match");

                    for (int j = 0; j < chr.Characters[i].Effects.Count; j++) {
                        Assert.AreEqual(chr.Characters[i].Effects[j].Bone, savedCHR.Characters[i].Effects[j].Bone, "Character effect bone values do not match");
                        Assert.AreEqual(chr.Characters[i].Effects[j].Effect, savedCHR.Characters[i].Effects[j].Effect, "Character effect values do not match");
                    }
                }
            }
        }
    }
}