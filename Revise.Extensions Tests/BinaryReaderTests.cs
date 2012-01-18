#region License

/**
 * Copyright (C) 2012 Jack Wakefield
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

using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Revise.Extensions.Tests {
    /// <summary>
    /// Provides testing for the <see cref="BinaryReaderExtensions"/> class.
    /// </summary>
    [TestFixture]
    public class BinaryReaderTests {
        /// <summary>
        /// Tests the ReadNullTerminatedString method using a UTF-8 encoded string.
        /// </summary>
        [Test]
        public void TestReadNullTerminatedStringMethodUTF8() {
            const string value = "Null terminated string";
            byte[] valueCharacters = Encoding.UTF8.GetBytes(value);

            byte[] buffer = new byte[valueCharacters.Length + 2];
            Array.Copy(valueCharacters, buffer, valueCharacters.Length);
            buffer[valueCharacters.Length] = 0;
            buffer[valueCharacters.Length + 1] = (byte)'a';

            MemoryStream stream = new MemoryStream(buffer);
            BinaryReader reader = new BinaryReader(stream);
            string nullTerminatedString = reader.ReadNullTerminatedString(Encoding.UTF8);
            reader.Close();

            Assert.AreEqual(value, nullTerminatedString, "Incorrect string value");
        }

        /// <summary>
        /// Tests the ReadNullTerminatedString method using a EUC-KR encoded string.
        /// </summary>
        [Test]
        public void TestReadNullTerminatedStringMethodKorean() {
            Encoding encoding = Encoding.GetEncoding("EUC-KR");

            const string value = "널 종료 문자열";
            byte[] valueCharacters = encoding.GetBytes(value);

            byte[] buffer = new byte[valueCharacters.Length + 2];
            Array.Copy(valueCharacters, buffer, valueCharacters.Length);
            buffer[valueCharacters.Length] = 0;
            buffer[valueCharacters.Length + 1] = (byte)'a';

            MemoryStream stream = new MemoryStream(buffer);
            BinaryReader reader = new BinaryReader(stream);
            string nullTerminatedString = reader.ReadNullTerminatedString();
            reader.Close();

            Assert.AreEqual(value, nullTerminatedString, "Incorrect string value");
        }
    }
}