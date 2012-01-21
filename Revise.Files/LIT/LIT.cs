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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Revise.Files {
    /// <summary>
    /// Provides the ability to create, open and save LIT files.
    /// </summary>
    public class LIT : FileLoader {
        #region Properties

        /// <summary>
        /// Gets the object count.
        /// </summary>
        public int ObjectCount {
            get {
                return objects.Count;
            }
        }

        /// <summary>
        /// Gets the file count.
        /// </summary>
        public int FileCount {
            get {
                return files.Count;
            }
        }

        #endregion

        private List<LightObject> objects;
        private List<string> files;

        /// <summary>
        /// Initializes a new instance of the <see cref="Revise.Files.LIT"/> class.
        /// </summary>
        public LIT() {
            objects = new List<LightObject>();
            files = new List<string>();

            Reset();
        }

        /// <summary>
        /// Gets the specified <see cref="Revise.Files.LightObject"/>.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified object does not exist.</exception>
        public LightObject this[int @object] {
            get {
                if (@object < 0 || @object > objects.Count - 1) {
                    throw new ArgumentOutOfRangeException("@object", "Object is out of range");
                }

                return objects[@object];
            }
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            int objectCount = reader.ReadInt32();

            for (int i = 0; i < objectCount; i++) {
                int partCount = reader.ReadInt32();

                LightObject @object = new LightObject();
                @object.ID = reader.ReadInt32() - 1;

                for (int j = 0; j < partCount; j++) {
                    LightPart part = new LightPart();
                    part.Name = reader.ReadString();
                    part.ID = reader.ReadInt32() - 1;
                    part.FileName = reader.ReadString();
                    part.LightmapIndex = reader.ReadInt32();
                    part.PixelsPerObject = reader.ReadInt32();
                    part.ObjectsPerWidth = reader.ReadInt32();
                    part.ObjectPosition = reader.ReadInt32();

                    @object.AddPart(part);
                }

                objects.Add(@object);
            }

            int fileCount = reader.ReadInt32();

            for (int i = 0; i < fileCount; i++) {
                files.Add(reader.ReadString());
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(ObjectCount);

            objects.ForEach(@object => {
                writer.Write(@object.PartCount);
                writer.Write(@object.ID + 1);

                for (int i = 0; i < @object.PartCount; i++) {
                    LightPart part = @object[i];
                    writer.Write(part.Name);
                    writer.Write(part.ID + 1);
                    writer.Write(part.FileName);
                    writer.Write(part.LightmapIndex);
                    writer.Write(part.PixelsPerObject);
                    writer.Write(part.ObjectsPerWidth);
                    writer.Write(part.ObjectPosition);
                }
            });

            writer.Write(FileCount);

            files.ForEach(name => {
                writer.Write(name);
            });
        }

        /// <summary>
        /// Adds a new light object.
        /// </summary>
        /// <returns>The light object added.</returns>
        public LightObject AddObject() {
            return AddObject(new LightObject());
        }

        /// <summary>
        /// Adds the specified light object.
        /// </summary>
        /// <param name="@object">The light object.</param>
        /// <returns>The light object added.</returns>
        public LightObject AddObject(LightObject @object) {
            objects.Add(@object);

            return @object;
        }

        /// <summary>
        /// Removes the specified light object.
        /// </summary>
        /// <param name="@object">The light object to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified light object is out of range.</exception>
        public void RemoveObject(int @object) {
            if (@object < 0 || @object > objects.Count - 1) {
                throw new ArgumentOutOfRangeException("@object", "Object is out of range");
            }

            objects.RemoveAt(@object);
        }

        /// <summary>
        /// Removes the specified light object.
        /// </summary>
        /// <param name="@object">The light object to remove.</param>
        /// <exception cref="System.ArgumentException">Thrown when the file does not contain the specified light object.</exception>
        public void RemoveObject(LightObject @object) {
            if (!objects.Contains(@object)) {
                throw new ArgumentException("@object", "Object is out of range");
            }

            int objectIndex = objects.IndexOf(@object);
            RemoveObject(objectIndex);
        }

        /// <summary>
        /// Gets the name of the specified file.
        /// </summary>
        /// <param name="file">The specified file.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified file is out of range.</exception>
        /// <returns>The file name.</returns>
        public string GetFileName(int file) {
            if (file < 0 || file > files.Count - 1) {
                throw new ArgumentOutOfRangeException("file", "File is out of range");
            }

            return files[file];
        }

        /// <summary>
        /// Sets the name of the specified file.
        /// </summary>
        /// <param name="file">The specified file.</param>
        /// <param name="name">The name of the file.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified file is out of range.</exception>
        public void SetFileName(int file, string name) {
            if (file < 0 || file > files.Count - 1) {
                throw new ArgumentOutOfRangeException("file", "File is out of range");
            }

            files[file] = name;
        }

        /// <summary>
        /// Removes the specified file name.
        /// </summary>
        /// <param name="file">The specified file to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified file is out of range.</exception>
        public void RemoveFileName(int file) {
            if (file < 0 || file > files.Count - 1) {
                throw new ArgumentOutOfRangeException("file", "File is out of range");
            }

            files.RemoveAt(file);
        }

        /// <summary>
        /// Removes all light objects.
        /// </summary>
        public void Clear() {
            objects.Clear();
            files.Clear();
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();
            Clear();
        }
    }
}