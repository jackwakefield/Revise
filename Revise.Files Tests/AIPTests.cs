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
    /// Provides testing for the <see cref="Revise.Files.AIP"/> class.
    /// </summary>
    [TestFixture]
    public class AIPTests {
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

            AIP aip = new AIP();
            aip.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            AIP aip = new AIP();
            aip.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            aip.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            AIP savedAIP = new AIP();
            savedAIP.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(aip.Name, savedAIP.Name, "Names do not match");
            Assert.AreEqual(aip.IdleInterval, savedAIP.IdleInterval, "Idle interval values do not match");
            Assert.AreEqual(aip.DamageRate, savedAIP.DamageRate, "Damage rate values do not match");

            Assert.AreEqual(aip.Patterns.Count, savedAIP.Patterns.Count, "Pattern counts do not match");

            for (int j = 0; j < aip.Patterns.Count; j++) {
                Assert.AreEqual(aip.Patterns[j].Name, savedAIP.Patterns[j].Name, "Pattern names do not match");

                Assert.AreEqual(aip.Patterns[j].Events.Count, savedAIP.Patterns[j].Events.Count, "Event counts do not match");

                for (int k = 0; k < aip.Patterns[j].Events.Count; k++) {
                    Assert.AreEqual(aip.Patterns[j].Events[k].Name, savedAIP.Patterns[j].Events[k].Name, "Event names do not match");

                    Assert.AreEqual(aip.Patterns[j].Events[k].Conditions.Count, savedAIP.Patterns[j].Events[k].Conditions.Count, "Condition counts do not match");
                    Assert.AreEqual(aip.Patterns[j].Events[k].Actions.Count, savedAIP.Patterns[j].Events[k].Actions.Count, "Action counts do not match");

                    for (int l = 0; l < aip.Patterns[j].Events[k].Conditions.Count; l++) {
                        Assert.AreEqual(aip.Patterns[j].Events[k].Conditions[l].GetType(), aip.Patterns[j].Events[k].Conditions[l].GetType(), "Condition types do not match");
                    }

                    for (int l = 0; l < aip.Patterns[j].Events[k].Actions.Count; l++) {
                        Assert.AreEqual(aip.Patterns[j].Events[k].Actions[l].GetType(), aip.Patterns[j].Events[k].Actions[l].GetType(), "Action types do not match");
                    }
                }
            }
        }
    }
}