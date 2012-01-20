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

using System.Collections.Generic;
using Revise.Files.Exceptions;

namespace Revise.Files {
    /// <summary>
    /// Represents an HLP node.
    /// </summary>
    public class HelpNode {
        #region Properties

        /// <summary>
        /// Gets or sets the node name.
        /// </summary>
        /// <value>
        /// The node name.
        /// </value>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets the child count.
        /// </summary>
        public int ChildCount {
            get {
                return children.Count;
            }
        }

        #endregion

        private List<HelpNode> children;

        /// <summary>
        /// Initializes a new instance of the <see cref="Revise.Files.HelpNode"/> class.
        /// </summary>
        public HelpNode() {
            Name = string.Empty;
            children = new List<HelpNode>();
        }

        /// <summary>
        /// Gets the specified <see cref="Revise.Files.HelpNode"/> .
        /// </summary>
        /// <exception cref="Revise.Exceptions.ChildOutOfRangeException">Thrown when the specified child is out of range.</exception>
        public HelpNode this[int child] {
            get {
                if (child < 0 || child > children.Count - 1) {
                    throw new ChildOutOfRangeException();
                }

                return children[child];
            }
        }

        /// <summary>
        /// Adds the child node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The added node.</returns>
        public HelpNode AddChild(HelpNode node) {
            children.Add(node);

            return node;
        }

        /// <summary>
        /// Adds a new child node with the optional name;
        /// </summary>
        /// <param name="name">The node name.</param>
        /// <returns>The added node.</returns>
        public HelpNode AddChild(string name = "") {
            HelpNode node = new HelpNode();
            node.Name = name;

            children.Add(node);

            return node;
        }

        /// <summary>
        /// Removes the specified child node.
        /// </summary>
        /// <param name="child">The child node.</param>
        /// <exception cref="Revise.Exceptions.ChildOutOfRangeException">Thrown when the specified child is out of range.</exception>
        public void RemoveChild(int child) {
            if (child < 0 || child > children.Count - 1) {
                throw new ChildOutOfRangeException();
            }

            children.RemoveAt(child);
        }
    }
}