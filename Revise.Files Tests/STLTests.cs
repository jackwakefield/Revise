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
    /// Provides testing for the <see cref="Revise.Files.STL"/> class.
    /// </summary>
    [TestFixture]
    public class STLTests {
        private const string ITEM_TEST_FILE = "Files/LIST_FACEITEM_S.STL";
        private const string QUEST_TEST_FILE = "Files/LIST_QUEST_S.STL";
        private const string NORMAL_TEST_FILE = "Files/STR_ITEMTYPE.STL";

        /// <summary>
        /// Tests the load method using the specified file path and row count.
        /// </summary>
        private void TestLoadMethod(string filePath, int rowCount) {
            Stream stream = File.OpenRead(filePath);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            STL stl = new STL();
            stl.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(rowCount, stl.RowCount, "Incorrect row count");
        }

        /// <summary>
        /// Tests the save method using the specified file path.
        /// </summary>
        private void TestSaveMethod(string filePath) {
            STL stl = new STL();
            stl.Load(filePath);

            MemoryStream savedStream = new MemoryStream();
            stl.Save(savedStream);
            savedStream.Seek(0, SeekOrigin.Begin);

            STL savedSTL = new STL();
            savedSTL.Load(savedStream);
            savedStream.Close();

            Assert.AreEqual(stl.TableType, stl.TableType, "Table types do not match");
            Assert.AreEqual(stl.RowCount, stl.RowCount, "Row counts do not match");

            for (int i = 0; i < stl.RowCount; i++) {
                for (int j = 0; j < stl.LanguageCount; j++) {
                    TableLanguage language = (TableLanguage)j;

                    Assert.AreEqual(stl[i].GetText(language), savedSTL[i].GetText(language), "Text values do not match");

                    if (stl.TableType == TableType.Item || stl.TableType == TableType.Quest) {
                        Assert.AreEqual(stl[i].GetDescription(language), savedSTL[i].GetDescription(language), "Description values do not match");

                        if (stl.TableType == TableType.Quest) {
                            Assert.AreEqual(stl[i].GetStartMessage(language), savedSTL[i].GetStartMessage(language), "Start message values do not match");
                            Assert.AreEqual(stl[i].GetEndMessage(language), savedSTL[i].GetEndMessage(language), "End message values do not match");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tests the load method using an item file type.
        /// </summary>
        [Test]
        public void TestItemLoadMethod() {
            const int ROW_COUNT = 43;
            TestLoadMethod(ITEM_TEST_FILE, ROW_COUNT);
        }

        /// <summary>
        /// Tests the load method using a quest file type.
        /// </summary>
        [Test]
        public void TestQuestLoadMethod() {
            const int ROW_COUNT = 235;
            TestLoadMethod(QUEST_TEST_FILE, ROW_COUNT);
        }

        /// <summary>
        /// Tests the load method using a normal file type.
        /// </summary>
        [Test]
        public void TestNormalLoadMethod() {
            const int ROW_COUNT = 75;
            TestLoadMethod(NORMAL_TEST_FILE, ROW_COUNT);
        }

        /// <summary>
        /// Tests the save method using an item file type.
        /// </summary>
        [Test]
        public void TestItemSaveMethod() {
            TestSaveMethod(ITEM_TEST_FILE);
        }

        /// <summary>
        /// Tests the save method using a quest file type.
        /// </summary>
        [Test]
        public void TestQuestSaveMethod() {
            TestSaveMethod(QUEST_TEST_FILE);
        }

        /// <summary>
        /// Tests the save method using a normal file type.
        /// </summary>
        [Test]
        public void TestNormalSaveMethod() {
            TestSaveMethod(NORMAL_TEST_FILE);
        }

        /// <summary>
        /// Tests the row methods.
        /// </summary>
        [Test]
        public void TestRowMethods() {
            const string ROW_KEY = "Test Key";
            const int ROW_ID = 1;
            const string ROW_VALUE_1 = "Test Value 1";
            const string ROW_VALUE_2 = "Test Value 2";
            
            STL stl = new STL();
            stl.AddRow(ROW_KEY, ROW_ID);

            Assert.AreEqual(stl.RowCount, 1, "Row count is incorrect");

            stl[0].SetText(ROW_VALUE_1);
            string rowValue = stl[0].GetText();

            Assert.AreEqual(rowValue, ROW_VALUE_1, "Row value is incorrect");

            stl[ROW_KEY].SetText(ROW_VALUE_2);
            rowValue = stl[ROW_KEY].GetText();

            Assert.AreEqual(rowValue, ROW_VALUE_2, "Row value is incorrect");

            stl.RemoveRow(ROW_KEY);

            Assert.AreEqual(stl.RowCount, 0, "Row count is incorrect");
        }
    }
}