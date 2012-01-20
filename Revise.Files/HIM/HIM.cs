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

namespace Revise.Files {
    /// <summary>
    /// Provides the ability to create, open and save HIM files.
    /// </summary>
    public class HIM : FileLoader {
        #region Constants

        private const int DEFAULT_WIDTH = 65;
        private const int DEFAULT_HEIGHT = 65;

        private const int PATCH_GRID_COUNT = 4;
        private const float PATCH_SIZE = 250.0f;

        private const int PATCH_WIDTH = 16;
        private const int PATCH_HEIGHT = 16;
        private const int QUAD_PATCH_COUNT = 85;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the width dimension of the heightmap.
        /// </summary>
        public int Width {
            get {
                return heights.GetLength(1);
            }
        }

        /// <summary>
        /// Gets the height dimension of the heightmap.
        /// </summary>
        public int Height {
            get {
                return heights.GetLength(0);
            }
        }

        /// <summary>
        /// Gets the patches.
        /// </summary>
        public Patch[,] Patches {
            get {
                return patches;
            }
        }

        /// <summary>
        /// Gets the quad patches.
        /// </summary>
        public Patch[] QuadPatches {
            get {
                return quadPatches;
            }
        }

        #endregion

        private float[,] heights;
        private Patch[,] patches;
        private Patch[] quadPatches;

        /// <summary>
        /// Initializes a new instance of the <see cref="Revise.Files.HIM"/> class.
        /// </summary>
        public HIM() {
            Reset();
        }

        /// <summary>
        /// Gets the height value of the specified coordinates.
        /// </summary>
        public float this[int x, int y] {
            get {
                return heights[x, y];
            }
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            heights = new float[height, width];

            int patchGridCount = reader.ReadInt32();
            float patchSize = reader.ReadSingle();

            for (int h = 0; h < height; h++) {
                for (int w = 0; w < width; w++) {
                    heights[h, w] = reader.ReadSingle();
                }
            }

            string name = reader.ReadString();
            int patchCount = reader.ReadInt32();

            for (int h = 0; h < 16; h++) {
                for (int w = 0; w < 16; w++) {
                    patches[h, w].Maximum = reader.ReadSingle();
                    patches[h, w].Minimum = reader.ReadSingle();
                }
            }

            int quadPatchCount = reader.ReadInt32();

            for (int i = 0; i < quadPatchCount; i++) {
                quadPatches[i].Maximum = reader.ReadSingle();
                quadPatches[i].Minimum = reader.ReadSingle();
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(Width);
            writer.Write(Height);

            writer.Write(PATCH_GRID_COUNT);
            writer.Write(PATCH_SIZE);

            for (int h = 0; h < Height; h++) {
                for (int w = 0; w < Width; w++) {
                    writer.Write(heights[h, w]);
                }
            }

            InvalidatePatches();

            writer.Write("quad");
            writer.Write(patches.GetLength(0) * patches.GetLength(1));

            for (int h = 0; h < patches.GetLength(0); h++) {
                for (int w = 0; w < patches.GetLength(1); w++) {
                    writer.Write(patches[h, w].Maximum);
                    writer.Write(patches[h, w].Minimum);
                }
            }

            writer.Write(quadPatches.Length);

            for (int i = 0; i < quadPatches.Length; i++) {
                writer.Write(quadPatches[i].Maximum);
                writer.Write(quadPatches[i].Minimum);
            }
        }

        /// <summary>
        /// Invalidates the patches.
        /// </summary>
        public void InvalidatePatches() {
            for (int h = 0; h < patches.GetLength(0); h++) {
                for (int w = 0; w < patches.GetLength(1); w++) {
                    float maximumHeight = float.MinValue;
                    float minimumHeight = float.MaxValue;

                    for (int vh = 0; vh < 5; vh++) {
                        for (int vw = 0; vw < 5; vw++) {
                            float height = heights[65 - (h * 4 + vh + 1), w * 4 + vw];

                            if (height > maximumHeight) {
                                maximumHeight = height;
                            }

                            if (height < minimumHeight) {
                                minimumHeight = height;
                            }
                        }
                    }

                    patches[h, w].Maximum = maximumHeight;
                    patches[h, w].Minimum = minimumHeight;
                }
            }

            InvalidateQuadPatches(0, 256);
            InvalidateQuadPatches(1, 128);
            InvalidateQuadPatches(5, 64);
            InvalidateQuadPatches(21, 32);
        }

        /// <summary>
        /// Invalidates the quad patches.
        /// TODO: Fix this, it generates the patches in the wrong order.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="size">The size.</param>
        private void InvalidateQuadPatches(int index, int size) {
            int segments = 256 / size;
            int patchCount = size / 16;

            for (int h = 0; h < segments; h++) {
                for (int w = 0; w < segments; w++) {
                    float maximumHeight = float.MinValue;
                    float minimumHeight = float.MaxValue;

                    for (int ph = 0; ph < patchCount; ph++) {
                        for (int pw = 0; pw < patchCount; pw++) {
                            Patch patch = patches[w * patchCount + pw, h * patchCount + ph];

                            if (patch.Maximum > maximumHeight) {
                                maximumHeight = patch.Maximum;
                            }

                            if (patch.Minimum < minimumHeight) {
                                minimumHeight = patch.Minimum;
                            }
                        }
                    }

                    quadPatches[index].Maximum = maximumHeight;
                    quadPatches[index++].Minimum = minimumHeight;
                }
            }
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            heights = new float[DEFAULT_HEIGHT, DEFAULT_WIDTH];
            patches = new Patch[PATCH_HEIGHT, PATCH_WIDTH];
            quadPatches = new Patch[QUAD_PATCH_COUNT];
        }
    }
}