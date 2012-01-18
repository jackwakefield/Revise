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

using System.IO;
using System.Text;

/// <summary>
/// A collection of extensions for the <see cref="BinaryWriter"/> class.
/// </summary>
public static class BinaryWriterExtensions {
    /// <summary>
    /// The default encoding to be used when reading strings.
    /// </summary>
    public static Encoding DefaultEncoding;

    /// <summary>
    /// Initializes the <see cref="BinaryReaderExtensions"/> class.
    /// </summary>
    static BinaryWriterExtensions() {
        DefaultEncoding = Encoding.GetEncoding("EUC-KR");
    }

    /// <summary>
    /// Writes the specified string pre-fixed with the string length as a 16-bit integer to the underlying stream.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static void WriteShortString(this BinaryWriter writer, string value) {
        writer.WriteShortString(value, DefaultEncoding);
    }

    /// <summary>
    /// Writes the specified string pre-fixed with the string length as a 16-bit integer to the underlying stream.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <param name="encoding">The character encoding.</param>
    public static void WriteShortString(this BinaryWriter writer, string value, Encoding encoding) {
        writer.Write((short)encoding.GetByteCount(value));
        writer.WriteString(value, encoding);
    }

    /// <summary>
    /// Writes the specified string to the underlying stream.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static void WriteString(this BinaryWriter writer, string value) {
        writer.WriteString(value, DefaultEncoding);
    }

    /// <summary>
    /// Writes the specified string to the underlying stream.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <param name="encoding">The character encoding.</param>
    public static void WriteString(this BinaryWriter writer, string value, Encoding encoding) {
        writer.Write(encoding.GetBytes(value));
    }
}