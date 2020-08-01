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
using Revise;
using System.Numerics;

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
    /// Writes the specified string pre-fixed with the string length as a 8-bit integer to the underlying stream.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static void WriteByteString(this BinaryWriter writer, string value) {
        writer.WriteByteString(value, DefaultEncoding);
    }

    /// <summary>
    /// Writes the specified string pre-fixed with the string length as a 8-bit integer to the underlying stream.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <param name="encoding">The character encoding.</param>
    public static void WriteByteString(this BinaryWriter writer, string value, Encoding encoding) {
        writer.Write((byte)encoding.GetByteCount(value));
        writer.WriteString(value, encoding);
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
    /// Writes the specified string pre-fixed with the string length as a 32-bit integer to the underlying stream.
    /// </summary>
    /// <param name="value">The string value.</param>
    public static void WriteIntString(this BinaryWriter writer, string value) {
        writer.WriteIntString(value, DefaultEncoding);
    }

    /// <summary>
    /// Writes the specified string pre-fixed with the string length as a 32-bit integer to the underlying stream.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <param name="encoding">The character encoding.</param>
    public static void WriteIntString(this BinaryWriter writer, string value, Encoding encoding) {
        writer.Write(encoding.GetByteCount(value));
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

    /// <summary>
    /// Writes the specified string to the underlying stream.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <param name="length">The fixed length.</param>
    public static void WriteString(this BinaryWriter writer, string value, int length, char paddingCharacter = '\0') {
        writer.WriteString(value, length, DefaultEncoding, paddingCharacter);
    }

    /// <summary>
    /// Writes the specified string to the underlying stream.
    /// </summary>
    /// <param name="value">The string value.</param>
    /// <param name="length">The fixed length.</param>
    /// <param name="encoding">The character encoding.</param>
    public static void WriteString(this BinaryWriter writer, string value, int length, Encoding encoding, char paddingCharacter = '\0') {
        byte[] values = encoding.GetBytes(value);
        Array.Resize<byte>(ref values, length);

        for (int i = encoding.GetByteCount(value); i < length; i++) {
            values[i] = (byte)paddingCharacter;
        }

        writer.Write(values);
    }

    /// <summary>
    /// Writes the specified matrix to the underlying stream.
    /// </summary>
    /// <param name="value">The matrix value.</param>
    public static void Write(this BinaryWriter writer, Matrix4x4 value) {
        writer.Write(value.M11);
        writer.Write(value.M12);
        writer.Write(value.M13);
        writer.Write(value.M14);
        writer.Write(value.M21);
        writer.Write(value.M22);
        writer.Write(value.M23);
        writer.Write(value.M24);
        writer.Write(value.M31);
        writer.Write(value.M32);
        writer.Write(value.M33);
        writer.Write(value.M34);
        writer.Write(value.M41);
        writer.Write(value.M42);
        writer.Write(value.M43);
        writer.Write(value.M44);
    }

    /// <summary>
    /// Writes the specified vector to the underlying stream.
    /// </summary>
    /// <param name="value">The vector value.</param>
    public static void Write(this BinaryWriter writer, Vector2 value) {
        writer.Write(value.X);
        writer.Write(value.Y);
    }

    /// <summary>
    /// Writes the specified vector to the underlying stream.
    /// </summary>
    /// <param name="value">The vector value.</param>
    public static void Write(this BinaryWriter writer, Vector3 value) {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
    }

    /// <summary>
    /// Writes the specified vector to the underlying stream.
    /// </summary>
    /// <param name="value">The vector value.</param>
    public static void Write(this BinaryWriter writer, Vector4 value) {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
        writer.Write(value.W);
    }

    /// <summary>
    /// Writes the specified vector to the underlying stream.
    /// </summary>
    /// <param name="value">The vector value.</param>
    public static void Write(this BinaryWriter writer, ShortVector3 value) {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
    }

    /// <summary>
    /// Writes the specified vector to the underlying stream.
    /// </summary>
    /// <param name="value">The vector value.</param>
    public static void Write(this BinaryWriter writer, ShortVector4 value) {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
        writer.Write(value.W);
    }

    /// <summary>
    /// Writes the specified vector to the underlying stream.
    /// </summary>
    /// <param name="value">The quaternion value.</param>
    /// <param name="w">if set to <c>true</c> the value is written in the order WXYZ, else XYZW.</param>
    public static void Write(this BinaryWriter writer, Quaternion value, bool w = false) {
        if (w) {
            writer.Write(value.W);
        }

        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);

        if (!w) {
            writer.Write(value.W);
        }
    }
}