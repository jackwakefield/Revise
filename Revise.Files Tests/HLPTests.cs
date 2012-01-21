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

using System;
using System.IO;
using NUnit.Framework;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="Revise.Files.HLP"/> class.
    /// </summary>
    [TestFixture]
    public class HLPTests {
        private const string TEST_FILE = "Files/HELP.HLP";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int PAGE_COUNT = 69;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            HLP hlp = new HLP();
            hlp.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(streamPosition, fileSize, "Not all of the file was read");
            Assert.AreEqual(hlp.PageCount, PAGE_COUNT, "Incorrect page count");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            HLP hlp = new HLP();
            hlp.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            hlp.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            HLP savedHLP = new HLP();
            savedHLP.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(hlp.PageCount, savedHLP.PageCount, "Page counts do not match");

            TestNodes(hlp.RootNode, savedHLP.RootNode);

            for (int i = 0; i < hlp.PageCount; i++) {
                Assert.AreEqual(hlp[i].Title, savedHLP[i].Title, "Page title values do not match");
                Assert.AreEqual(hlp[i].Content, savedHLP[i].Content, "Page content values do not match");
            }
        }

        /// <summary>
        /// Tests the two nodes by comparing the names and the children.
        /// </summary>
        /// <param name="nodeA">The first node.</param>
        /// <param name="nodeB">The second node.</param>
        private void TestNodes(HelpNode nodeA, HelpNode nodeB) {
            Assert.AreEqual(nodeA.Name, nodeB.Name, "Node name values do not match");
            Assert.AreEqual(nodeA.ChildCount, nodeB.ChildCount, "Child counts do not match");

            for (int i = 0; i < nodeA.ChildCount; i++) {
                TestNodes(nodeA[i], nodeB[i]);
            }
        }

        /// <summary>
        /// Tests the node methods.
        /// </summary>
        [Test]
        public void TestNodeMethods() {
            const string CHILD_NAME_VALUE_1 = "Child Value 1";
            const string CHILD_NAME_VALUE_2 = "Child Value 2";

            HLP hlp = new HLP();

            hlp.RootNode.AddChild();
            hlp.RootNode[0].Name = CHILD_NAME_VALUE_1;

            Assert.AreEqual(hlp.RootNode.ChildCount, 1, "Child count is incorrect");
            Assert.AreEqual(hlp.RootNode[0].Name, CHILD_NAME_VALUE_1, "Child name value is incorrect");

            hlp.RootNode.AddChild(CHILD_NAME_VALUE_2);

            Assert.AreEqual(hlp.RootNode.ChildCount, 2, "Child count is incorrect");
            Assert.AreEqual(hlp.RootNode[1].Name, CHILD_NAME_VALUE_2, "Child name value is incorrect");

            hlp.RootNode.RemoveChild(0);

            Assert.AreEqual(hlp.RootNode.ChildCount, 1, "Child count is incorrect");
            Assert.AreEqual(hlp.RootNode[0].Name, CHILD_NAME_VALUE_2, "Child name value is incorrect");

            hlp.RootNode.RemoveChild(0);

            Assert.Throws(typeof(ArgumentOutOfRangeException), () => {
                hlp.RootNode[0].Name = CHILD_NAME_VALUE_2;
            }, "Child not removed");
        }

        /// <summary>
        /// Tests the page methods.
        /// </summary>
        [Test]
        public void TestPageMethods() {
            const string PAGE_TITLE_VALUE_1 = "Title Value 1";
            const string PAGE_CONTENT_VALUE_1 = "Content Value 1";
            const string PAGE_TITLE_VALUE_2 = "Title Value 2";
            const string PAGE_CONTENT_VALUE_2 = "Content Value 2";

            HLP hlp = new HLP();

            hlp.AddPage();
            hlp[0].Title = PAGE_TITLE_VALUE_1;
            hlp[0].Content = PAGE_CONTENT_VALUE_1;

            Assert.AreEqual(hlp.PageCount, 1, "Page count is incorrect");
            Assert.AreEqual(hlp[0].Title, PAGE_TITLE_VALUE_1, "Page title value is incorrect");
            Assert.AreEqual(hlp[0].Content, PAGE_CONTENT_VALUE_1, "Page content value is incorrect");

            hlp.AddPage(PAGE_TITLE_VALUE_2, PAGE_CONTENT_VALUE_2);

            Assert.AreEqual(hlp.PageCount, 2, "Page count is incorrect");
            Assert.AreEqual(hlp[1].Title, PAGE_TITLE_VALUE_2, "Page title value is incorrect");
            Assert.AreEqual(hlp[1].Content, PAGE_CONTENT_VALUE_2, "Page content value is incorrect");

            hlp.RemovePage(0);

            Assert.AreEqual(hlp.PageCount, 1, "Page count is incorrect");
            Assert.AreEqual(hlp[0].Title, PAGE_TITLE_VALUE_2, "Page title value is incorrect");
            Assert.AreEqual(hlp[0].Content, PAGE_CONTENT_VALUE_2, "Page content value is incorrect");

            hlp.RemovePage(0);

            Assert.Throws(typeof(ArgumentOutOfRangeException), () => {
                hlp[0].Title = PAGE_TITLE_VALUE_2;
            }, "Page not removed");
        }
    }
}