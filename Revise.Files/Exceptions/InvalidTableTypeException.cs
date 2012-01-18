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

namespace Revise.Files.Exceptions {
    /// <summary>
    /// The exception that is thrown when a table type value is invalid.
    /// </summary>
    public class InvalidTableTypeException : Exception {
        /// <summary>
        /// The format of the exception message.
        /// </summary>
        private const string MESSAGE_FORMAT = "Table type '{0}' is invalid";

        #region Properties

        /// <summary>
        /// Gets the table type.
        /// </summary>
        public string Type {
            get; 
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Revise.Files.Exceptions.InvalidTableTypeException"/> class.
        /// </summary>
        public InvalidTableTypeException(string type)
            : base(string.Format(MESSAGE_FORMAT, type)) {
            Type = type;
        }
    }
}