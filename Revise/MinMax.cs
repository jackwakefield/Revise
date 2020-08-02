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
using System.Numerics;
using Revise.Types;

namespace Revise {
    /// <summary>
    /// Represents a generic structure for minimum and maximum values.
    /// </summary>
    public struct MinMax<T> {
        private static readonly Random random;

        #region Properties

        /// <summary>
        /// Gets a random number in the range of the minimum and maximum values.
        /// </summary>
        public T Random {
            get {
                if (typeof(T) == typeof(int)) {
                    int minimum = (int)(object)Minimum;
                    int maximum = (int)(object)Maximum;

                    if (minimum == maximum)
                        return (T)(object)minimum;

                    return (T)(object)((random.Next(0, short.MaxValue) % (Math.Abs(maximum - minimum) + 1)) + minimum);
                }

                if (typeof(T) == typeof(float)) {
                    return (T)(object)RandomNumber((float)(object)Minimum, (float)(object)Maximum);
                }

                if (typeof(T) == typeof(Vector2)) {
                    Vector2 minimum = (Vector2)(object)Minimum;
                    Vector2 maximum = (Vector2)(object)Maximum;

                    float x = RandomNumber(minimum.X, maximum.X);
                    float y = RandomNumber(minimum.Y, maximum.Y);

                    return (T)(object)new Vector2(x, y);
                }

                if (typeof(T) == typeof(Vector3)) {
                    Vector3 minimum = (Vector3)(object)Minimum;
                    Vector3 maximum = (Vector3)(object)Maximum;

                    float x = RandomNumber(minimum.X, maximum.X);
                    float y = RandomNumber(minimum.Y, maximum.Y);
                    float z = RandomNumber(minimum.Z, maximum.Z);

                    return (T)(object)new Vector3(x, y, z);
                }

                if (typeof(T) == typeof(Vector4)) {
                    Vector4 minimum = (Vector4)(object)Minimum;
                    Vector4 maximum = (Vector4)(object)Maximum;

                    float x = RandomNumber(minimum.X, maximum.X);
                    float y = RandomNumber(minimum.Y, maximum.Y);
                    float z = RandomNumber(minimum.Z, maximum.Z);
                    float w = RandomNumber(minimum.W, maximum.W);

                    return (T)(object)new Vector4(x, y, z, w);
                }

                if (typeof(T) == typeof(Color4)) {
                    Color4 minimum = (Color4)(object)Minimum;
                    Color4 maximum = (Color4)(object)Maximum;

                    float r = RandomNumber(minimum.Red, maximum.Red);
                    float g = RandomNumber(minimum.Green, maximum.Green);
                    float b = RandomNumber(minimum.Blue, maximum.Blue);
                    float a = RandomNumber(minimum.Alpha, maximum.Alpha);

                    return (T)(object)new Color4(r, g, b, a);
                }

                throw new Exception("Random is not implemented for this type");
            }
        }

        /// <summary>
        /// Gets the range of the minimum and maximum values.
        /// </summary>
        public T Range {
            get {
                if (typeof(T) == typeof(int)) {
                    return (T)(object)Math.Abs((int)(object)Maximum - (int)(object)Minimum);
                }

                if (typeof(T) == typeof(float)) {
                    return (T)(object)Math.Abs((float)(object)Maximum - (float)(object)Minimum);
                }

                if (typeof(T) == typeof(Vector2)) {
                    Vector2 minimum = (Vector2)(object)Minimum;
                    Vector2 maximum = (Vector2)(object)Maximum;

                    float x = Math.Abs(maximum.X - minimum.X);
                    float y = Math.Abs(maximum.Y - minimum.Y);

                    return (T)(object)new Vector2(x, y);
                }

                if (typeof(T) == typeof(Vector3)) {
                    Vector3 minimum = (Vector3)(object)Minimum;
                    Vector3 maximum = (Vector3)(object)Maximum;

                    float x = Math.Abs(maximum.X - minimum.X);
                    float y = Math.Abs(maximum.Y - minimum.Y);
                    float z = Math.Abs(maximum.Z - minimum.Z);

                    return (T)(object)new Vector3(x, y, z);
                }

                if (typeof(T) == typeof(Vector4)) {
                    Vector4 minimum = (Vector4)(object)Minimum;
                    Vector4 maximum = (Vector4)(object)Maximum;

                    float x = Math.Abs(maximum.X - minimum.X);
                    float y = Math.Abs(maximum.Y - minimum.Y);
                    float z = Math.Abs(maximum.Z - minimum.Z);
                    float w = Math.Abs(maximum.W - minimum.W);

                    return (T)(object)new Vector4(x, y, z, w);
                }

                if (typeof(T) == typeof(Color4)) {
                    Color4 minimum = (Color4)(object)Minimum;
                    Color4 maximum = (Color4)(object)Maximum;

                    float r = Math.Abs(maximum.Red - minimum.Red);
                    float g = Math.Abs(maximum.Green - minimum.Green);
                    float b = Math.Abs(maximum.Blue - minimum.Blue);
                    float a = Math.Abs(maximum.Alpha - minimum.Alpha);

                    return (T)(object)new Color4(r, g, b, a);
                }

                throw new Exception("Range is not implemented for this type");
            }
        }

        #endregion

        public T Minimum;
        public T Maximum;

        /// <summary>
        /// Initializes the <see cref="MinMax"/> struct.
        /// </summary>
        static MinMax() {
            random = new Random();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinMax"/> struct.
        /// </summary>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        public MinMax(T minimum, T maximum) {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Randoms the number.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns></returns>
        private static float RandomNumber(float minimum, float maximum) {
            if (minimum == maximum) {
                return minimum;
            }

            return (((float)random.Next(0, short.MaxValue) / (float)short.MaxValue) * Math.Abs(maximum - minimum)) + minimum;
        }
    }
}