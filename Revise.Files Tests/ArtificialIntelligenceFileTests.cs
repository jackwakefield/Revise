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
using Revise.Files.AIP;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="ArtificialIntelligenceFile"/> class.
    /// </summary>
    [TestFixture]
    public class ArtificialIntelligenceFileTests {
        private const string TEST_FILE = "Files/CLAN_BOSS1.AIP";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            ArtificialIntelligenceFile artificialIntelligenceFile = new ArtificialIntelligenceFile();
            artificialIntelligenceFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            ArtificialIntelligenceFile artificialIntelligenceFile = new ArtificialIntelligenceFile();
            artificialIntelligenceFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            artificialIntelligenceFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            ArtificialIntelligenceFile savedArtificialIntelligenceFile = new ArtificialIntelligenceFile();
            savedArtificialIntelligenceFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(artificialIntelligenceFile.Name, savedArtificialIntelligenceFile.Name, "Names do not match");
            Assert.AreEqual(artificialIntelligenceFile.IdleInterval, savedArtificialIntelligenceFile.IdleInterval, "Idle interval values do not match");
            Assert.AreEqual(artificialIntelligenceFile.DamageRate, savedArtificialIntelligenceFile.DamageRate, "Damage rate values do not match");

            Assert.AreEqual(artificialIntelligenceFile.Patterns.Count, savedArtificialIntelligenceFile.Patterns.Count, "Pattern counts do not match");

            for (int j = 0; j < artificialIntelligenceFile.Patterns.Count; j++) {
                Assert.AreEqual(artificialIntelligenceFile.Patterns[j].Name, savedArtificialIntelligenceFile.Patterns[j].Name, "Pattern names do not match");

                Assert.AreEqual(artificialIntelligenceFile.Patterns[j].Events.Count, savedArtificialIntelligenceFile.Patterns[j].Events.Count, "Event counts do not match");

                for (int k = 0; k < artificialIntelligenceFile.Patterns[j].Events.Count; k++) {
                    Assert.AreEqual(artificialIntelligenceFile.Patterns[j].Events[k].Name, savedArtificialIntelligenceFile.Patterns[j].Events[k].Name, "Event names do not match");

                    Assert.AreEqual(artificialIntelligenceFile.Patterns[j].Events[k].Conditions.Count, savedArtificialIntelligenceFile.Patterns[j].Events[k].Conditions.Count, "Condition counts do not match");
                    Assert.AreEqual(artificialIntelligenceFile.Patterns[j].Events[k].Actions.Count, savedArtificialIntelligenceFile.Patterns[j].Events[k].Actions.Count, "Action counts do not match");

                    for (int l = 0; l < artificialIntelligenceFile.Patterns[j].Events[k].Conditions.Count; l++) {
                        Assert.AreEqual(artificialIntelligenceFile.Patterns[j].Events[k].Conditions[l].GetType(), artificialIntelligenceFile.Patterns[j].Events[k].Conditions[l].GetType(), "Condition types do not match");
                    }

                    for (int l = 0; l < artificialIntelligenceFile.Patterns[j].Events[k].Actions.Count; l++) {
                        Assert.AreEqual(artificialIntelligenceFile.Patterns[j].Events[k].Actions[l].GetType(), artificialIntelligenceFile.Patterns[j].Events[k].Actions[l].GetType(), "Action types do not match");
                    }
                }
            }
        }
    }
}